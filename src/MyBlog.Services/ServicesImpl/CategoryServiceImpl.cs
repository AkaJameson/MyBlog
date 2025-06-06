﻿using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Data;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Services;
using MyBlog.Utilites;

namespace MyBlog.Services.ServicesImpl
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;
        public CategoryServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 添加一个新分类
        /// </summary>
        /// <param name="category"></param>
        public async Task<OperateResult> AddCategory(CategoryAdd category)
        {
            if (await _unitOfWork.GetRepository<Category>().ExistsAsync(c => c.CategoryName == category.CategoryName && !c.IsDeleted))
            {
                return OperateResult.Failed("分类已存在");
            }
            else
            {
                var newCategory = new Category()
                {
                    CategoryName = category.CategoryName,
                    CreateTime = DateTime.Now,
                };
                await _unitOfWork.GetRepository<Category>().AddAsync(newCategory);
                await _unitOfWork.CommitAsync();
                return OperateResult.Successed();
            }
        }
        /// <summary>
        /// 删除一个分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                return OperateResult.Failed("分类不存在");
            }
            else
            {
                if (await _unitOfWork.GetRepository<Article>().ExistsAsync(a => a.CategoryId == id && !a.IsDeleted))
                {
                    return OperateResult.Failed("分类下存在文章，不能删除");
                }
                category.IsDeleted = true;
                await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                await _unitOfWork.CommitAsync();
                return OperateResult.Successed();
            }
        }
        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<OperateResult<CategoryDto>> QueryCategory(CategoryQuery query)
        {
            var categorys = await _unitOfWork.GetRepository<Category>()
                                .GetPagedAsync(query.PageIndex, query.PageSize, p => !p.IsDeleted);
            var result = new CategoryDto
            {
                TotalCount = categorys.TotalCount,
                CategoryInfos = categorys.Items.Select(c => new CategoryInfo
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    CreateTime = c.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                }).ToList()
            };
            return OperateResult.Successed<CategoryDto>(result);
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<OperateResult> UpdateCategory(CategoryEdit category)
        {
            var categoryEntity = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.Id);
            if (categoryEntity == null)
            {
                return OperateResult.Failed("分类不存在");
            }
            else
            {
                if (await _unitOfWork.GetRepository<Category>().ExistsAsync(c => c.CategoryName == category.CategoryName && c.Id != category.Id && !c.IsDeleted))
                {
                    return OperateResult.Failed("分类已存在");
                }
                else
                {
                    categoryEntity.CategoryName = category.CategoryName;
                    await _unitOfWork.GetRepository<Category>().UpdateAsync(categoryEntity);
                    await _unitOfWork.CommitAsync();
                    return OperateResult.Successed();
                }
            }

        }
    }
}
