using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBlog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThoughtController : DefaultController
    {
        private readonly IThoughtService _thoughtService;

        public ThoughtController(IThoughtService thoughtService)
        {
            _thoughtService = thoughtService;
        }

        [HttpPost("AddThought")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "添加说说", Description = "添加一条新的说说")]
        [Authorize]
        public async Task<OperateResult> AddThought([FromBody] ThoughtAdd thought)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的说说数据");
            }
            return await _thoughtService.AddThoughtAsync(thought);
        }

        [HttpPost("DeleteThought")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "删除说说", Description = "删除指定说说")]
        [Authorize]
        public async Task<OperateResult> DeleteThought([FromQuery] int id)
        {
            if (id <= 0)
            {
                return OperateResult.Failed("无效的说说ID");
            }
            return await _thoughtService.DeleteThoughtAsync(id);
        }

        [HttpPost("QueryThoughts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<ThoughtDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "查询说说", Description = "分页查询说说列表")]
        public async Task<OperateResult<ThoughtDto>> QueryThoughts([FromBody] ThoughtQuery query)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<ThoughtDto>("无效的查询参数");
            }
            return await _thoughtService.QueryThoughtsAsync(query);
        }

        [HttpPost("UpdateThought")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "更新说说", Description = "更新指定说说的内容")]
        [Authorize]
        public async Task<OperateResult> UpdateThought([FromBody] ThoughtEdit thought)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的说说数据");
            }
            return await _thoughtService.UpdateThoughtAsync(thought);
        }

        [HttpGet("GetThoughtList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<List<ThoughtInfo>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "获取说说列表", Description = "获取所有说说（不分页）")]
        public async Task<OperateResult<List<ThoughtInfo>>> GetThoughtList()
        {
            return await _thoughtService.GetThoughtListAsync();
        }
    }
}