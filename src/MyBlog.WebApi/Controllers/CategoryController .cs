using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace MyBlog.WebApi.Controllers
{
    [ApiController]
    public class CategoryController : DefaultController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "添加分类", Description = "创建新的分类")]
        [Authorize]
        public async Task<OperateResult> AddCategory([FromBody] CategoryAdd category)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的分类数据");
            }
            return await _categoryService.AddCategory(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "删除分类", Description = "删除指定分类")]
        [Authorize]
        public async Task<OperateResult> DeleteCategory([FromQuery] int id)
        {
            if (id <= 0)
            {
                return OperateResult.Failed("无效的分类 ID");
            }
            return await _categoryService.DeleteCategory(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "查询分类", Description = "根据条件查询分类列表")]
        [Authorize]
        public async Task<OperateResult<CategoryDto>> QueryCategory([FromBody] CategoryQuery query)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<CategoryDto>("无效的查询参数");
            }
            return await _categoryService.QueryCategory(query);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "更新分类", Description = "更新指定分类的信息")]
        [Authorize]
        public async Task<OperateResult> UpdateCategory([FromBody] CategoryEdit category)
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed("无效的分类数据");
            }
            return await _categoryService.UpdateCategory(category);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperateResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<OperateResult<List<CategoryInfo>>> GetAllCategory()
        {
            if (!ModelState.IsValid)
            {
                return OperateResult.Failed<List<CategoryInfo>>("无效的分类数据");
            }
            return await _categoryService.GetCategoryList();
        }
    }
}
