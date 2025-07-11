﻿using Microsoft.EntityFrameworkCore;
using MyBlog.DataAccessor.EFCore;
using MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions;
using MyBlog.Models.Data;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Services.Repository;
using MyBlog.Services.Services;
using MyBlog.Services.ServicesHelper;
using MyBlog.Utilites;

namespace MyBlog.Services.ServicesImpl
{
    [Api(ServiceLifetimeOption.Scoped)]
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;
        private readonly HotMapRepository hotMapRepository;
        public CategoryServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork, HotMapRepository hotMapRepository)
        {
            _unitOfWork = unitOfWork;
            this.hotMapRepository = hotMapRepository;
        }
        /// <summary>
        /// 添加一个新分类
        /// </summary>
        /// <param name="category"></param>
        public async Task<OperateResult> AddCategory(CategoryAdd category)
        {
            if (await _unitOfWork.GetRepository<Category>().ExistsAsync(c => c.CategoryName == category.CategoryName))
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

                await _unitOfWork.GetRepository<Category>().DeleteAsync(category);
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
            var categorys = await _unitOfWork.GetRepository<Category>().AsNoTracking().Include(p => p.Articles)
                                .Skip(Math.Max(0, query.PageIndex - 1) * Math.Max(1, query.PageSize))
                                .Take(Math.Max(1, query.PageSize)).ToListAsync();
            var totalCount = await _unitOfWork.GetRepository<Category>().CountAsync();
            var result = new CategoryDto
            {
                TotalCount = totalCount,
                CategoryInfos = categorys.Select(c => new CategoryInfo
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    ArticleCount = c.Articles.Count,
                 
                    CreateTime = c.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                }).ToList()
            };
            return OperateResult.Successed<CategoryDto>(result);
        }
        /// <summary>
        /// 更新类型
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
                if (await _unitOfWork.GetRepository<Category>().ExistsAsync(c => c.CategoryName == category.CategoryName && c.Id != category.Id))
                {
                    return OperateResult.Failed("分类已存在");
                }
                else
                {
                    categoryEntity.CategoryName = category.CategoryName;
                    await _unitOfWork.GetRepository<Category>().UpdateAsync(categoryEntity);
                    await hotMapRepository.AddHotMapAsync();
                    await _unitOfWork.CommitAsync();
                    return OperateResult.Successed();
                }
            }

        }

        public async Task<OperateResult<List<CategoryInfo>>> GetCategoryList()
        {
            var categorys = await _unitOfWork.GetRepository<Category>().AsNoTracking()
                .Include(p => p.Articles)
                .Select(p => new CategoryInfo
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName,
                    ArticleCount = p.Articles.Count,
                    CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                }).ToListAsync();
            return OperateResult.Successed(categorys);

        }
        /// <summary>
        /// 获取最受欢迎的分类
        /// </summary>
        /// <returns></returns>
        public async Task<OperateResult> GetMostViewCategoryType()
        {
            var result = await _unitOfWork.GetRepository<Category>()
                         .AsNoTracking()
                         .Include(c => c.Articles)
                         .OrderByDescending(p => p.Articles.Count())
                         .Take(5)
                         .Select(p => new
                         {
                             p.Id,
                             p.CategoryName,
                             num = p.Articles.Count()
                         }).ToListAsync();
            return OperateResult.Successed(result);
        }


    }
}
