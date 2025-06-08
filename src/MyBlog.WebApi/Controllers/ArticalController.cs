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
    public class ArticleController : DefaultController
    {
        private readonly IArticalService _articleService;

        public ArticleController(IArticalService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "添加文章", Description = "创建一篇新文章")]
        [Authorize]
        public async Task<OperateResult> AddArticle([FromBody] ArticalAdd article)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的文章数据");
            }
            return await _articleService.AddArticle(article);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "删除文章", Description = "将指定文章移到回收站")]
        [Authorize]
        public async Task<OperateResult> DeleteArticle([FromQuery] int id)
        {
            if (id <= 0)
            {
                return OperateResult.Failed("无效的文章 ID");
            }
            return await _articleService.DeleteArtile(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<ArticleDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "查询文章", Description = "根据条件查询文章列表")]
        public async Task<OperateResult<ArticleDto>> QueryPublishArticle([FromBody] ArticleQuery articleQuery)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<ArticleDto>("无效的查询参数");
            }
            articleQuery.IsPublished = true;
            return await _articleService.QueryArticle(articleQuery);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<ArticleDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "查询文章", Description = "查询所有文章列表")]
        [Authorize]
        public async Task<OperateResult<ArticleDto>> QueryArticle([FromBody] ArticleQuery articleQuery)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<ArticleDto>("无效的查询参数");
            }
            return await _articleService.QueryArticle(articleQuery);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<ArticleDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "查询回收站文章", Description = "查询已删除的文章列表")]
        [Authorize]
        public async Task<OperateResult<ArticleDto>> QueryGarbageArticle([FromBody] ArticleQuery articleQuery)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<ArticleDto>("无效的查询参数");
            }
            return await _articleService.QueryGarbageArticle(articleQuery);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "恢复文章", Description = "从回收站恢复指定文章")]
        [Authorize]
        public async Task<OperateResult> RecoverArticle([FromQuery] int id)
        {
            if (id <= 0)
            {
                return OperateResult.Failed("无效的文章 ID");
            }
            return await _articleService.RecoverArticle(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "更新文章", Description = "更新指定文章的内容")]
        [Authorize]
        public async Task<OperateResult> UpdateArticle([FromBody] ArticleUpdate article)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的文章数据");
            }
            return await _articleService.UpdateArtile(article);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "更新文章", Description = "更新指定文章的内容")]
        public async Task<OperateResult<ArticleInfo>> QuerySingle([FromQuery] int id, [FromQuery] bool addViews)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<ArticleInfo>("无效的请求格式");
            }
            return await _articleService.QuerySingleArticle(id, addViews);
        }
    }
}
