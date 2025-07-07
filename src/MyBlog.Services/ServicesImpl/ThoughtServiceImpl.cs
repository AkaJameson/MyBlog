using Microsoft.EntityFrameworkCore;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Data;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Repository;
using MyBlog.Services.Services;
using MyBlog.Utilites;

namespace MyBlog.Services.ServicesImpl
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class ThoughtServiceImpl : IThoughtService
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;
        private readonly HotMapRepository _hotMapRepository;

        public ThoughtServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork, HotMapRepository hotMapRepository)
        {
            _unitOfWork = unitOfWork;
            _hotMapRepository = hotMapRepository;
        }

        public async Task<OperateResult> AddThoughtAsync(ThoughtAdd thought)
        {
            if (string.IsNullOrWhiteSpace(thought.Content))
            {
                return OperateResult.Failed("内容不能为空");
            }

            var newThought = new Thought()
            {
                Content = thought.Content,
                PublishTime = DateTime.Now
            };

            await _unitOfWork.GetRepository<Thought>().AddAsync(newThought);
            await _hotMapRepository.AddHotMapAsync();
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }

        public async Task<OperateResult> DeleteThoughtAsync(int id)
        {
            var thought = await _unitOfWork.GetRepository<Thought>().GetByIdAsync(id);
            if (thought == null || thought.IsDeleted)
            {
                return OperateResult.Failed("说说不存在或已被删除");
            }

            thought.IsDeleted = true;
            await _unitOfWork.GetRepository<Thought>().UpdateAsync(thought);
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }

        public async Task<OperateResult<ThoughtDto>> QueryThoughtsAsync(ThoughtQuery query)
        {
            var thoughts = await _unitOfWork.GetRepository<Thought>()
                .GetPagedAsync(
                    query.PageIndex,
                    query.PageSize,
                    predicate: t => !t.IsDeleted,
                    orderBy: q => q.PublishTime,
                    false
                );


            var result = new ThoughtDto
            {
                TotalCount = thoughts.TotalCount,
                ThoughtInfos = thoughts.Items.Select(t => new ThoughtInfo
                {
                    Id = t.Id,
                    Content = t.Content,
                    PublishTime = t.PublishTime.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList()
            };

            return OperateResult.Successed(result);
        }

        public async Task<OperateResult> UpdateThoughtAsync(ThoughtEdit thought)
        {
            var thoughtEntity = await _unitOfWork.GetRepository<Thought>().GetByIdAsync(thought.Id);
            if (thoughtEntity == null || thoughtEntity.IsDeleted)
            {
                return OperateResult.Failed("说说不存在或已被删除");
            }

            if (string.IsNullOrWhiteSpace(thought.Content))
            {
                return OperateResult.Failed("内容不能为空");
            }

            thoughtEntity.Content = thought.Content;
            await _unitOfWork.GetRepository<Thought>().UpdateAsync(thoughtEntity);
            await _hotMapRepository.AddHotMapAsync();
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }

        public async Task<OperateResult<List<ThoughtInfo>>> GetThoughtListAsync()
        {
            var thoughts = await _unitOfWork.GetRepository<Thought>()
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.PublishTime)
                .Select(t => new ThoughtInfo
                {
                    Id = t.Id,
                    Content = t.Content,
                    PublishTime = t.PublishTime.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToListAsync();

            return OperateResult.Successed(thoughts);
        }
        /// <summary>
        /// 说说点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> AddLikeAsync(int id)
        {
            var thought = await _unitOfWork.GetRepository<Thought>().GetByIdAsync(id);
            if (thought == null || thought.IsDeleted)
            {
                return OperateResult.Failed("说说不存在或已被删除");
            }
            thought.Like++;
            await _unitOfWork.GetRepository<Thought>().UpdateAsync(thought);
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }
    }
}
