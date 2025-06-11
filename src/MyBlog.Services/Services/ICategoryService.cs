using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Utilites;

namespace MyBlog.Services.Services
{
    public interface ICategoryService
    {
        Task<OperateResult> AddCategory(CategoryAdd category);
        Task<OperateResult> DeleteCategory(int id);
        Task<OperateResult<CategoryDto>> QueryCategory(CategoryQuery query);
        Task<OperateResult> UpdateCategory(CategoryEdit category);
        Task<OperateResult<List<CategoryInfo>>> GetCategoryList();
        Task<OperateResult> GetMostViewCategoryType();
    }
}