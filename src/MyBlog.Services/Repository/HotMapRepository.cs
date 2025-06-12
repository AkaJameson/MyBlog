using Microsoft.EntityFrameworkCore;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Data;
using MyBlog.Utilites;

namespace MyBlog.Services.Repository
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class HotMapRepository
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;

        public HotMapRepository(IUnitOfWork<BlogDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Dictionary<DateTime, int>> GetHotMapAsync(int year)
        {
            var hotMap = new Dictionary<DateTime, int>();
            // 获取这一年所有的日期
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                hotMap[date] = 0;
            }

            // 查询数据库中这一年所有的 HotMap 项
            var items = await _unitOfWork.GetRepository<HotMap>()
                .Where(p => p.Date.Year == year)
                .ToListAsync();

            // 累加每一天的计数
            foreach (var item in items)
            {
                var date = item.Date.Date;
                if (hotMap.ContainsKey(date))
                {
                    hotMap[date] = item.OperateCount;
                }
            }

            return hotMap;
        }
        /// <summary>
        /// 添加热点统计
        /// </summary>
        /// <returns></returns>
        public async Task AddHotMapAsync()
        {
            var Date = DateTime.Now.Date;
            if (await _unitOfWork.GetRepository<HotMap>().ExistsAsync(p => p.Date == Date))
            {
                var hotMap = await _unitOfWork.GetRepository<HotMap>().FirstOrDefaultAsync(p => p.Date == Date);
                hotMap.OperateCount++;
                await _unitOfWork.GetRepository<HotMap>().UpdateAsync(hotMap);
            }
            else
            {
                var hotMap = new HotMap()
                {
                    Date = Date,
                    OperateCount = 1
                };
                await _unitOfWork.GetRepository<HotMap>().AddAsync(hotMap);
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
