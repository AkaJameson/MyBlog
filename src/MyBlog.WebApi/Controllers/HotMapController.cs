using Microsoft.AspNetCore.Mvc;
using MyBlog.Services.Repository;
using MyBlog.Utilites;

namespace MyBlog.WebApi.Controllers
{
    [ApiController]

    public class HotMapController : DefaultController
    {
        private readonly HotMapRepository _hotMapRepository;

        public HotMapController(HotMapRepository hotMapRepository)
        {
            _hotMapRepository = hotMapRepository;
        }
        [HttpGet]
        public async Task<OperateResult> GetHotMap([FromQuery] int year)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("参数错误");
            }
            var hotMap = await _hotMapRepository.GetHotMapAsync(year);
            var stringMap = hotMap.ToDictionary(kv => kv.Key.ToString("yyyy-MM-dd"), kv => kv.Value);

            return OperateResult.Successed(stringMap);
        }
    }
}
