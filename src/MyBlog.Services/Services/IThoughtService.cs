using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Utilites;

namespace MyBlog.Services.Services
{
    public interface IThoughtService
    {
        Task<OperateResult> AddThoughtAsync(ThoughtAdd thought);
        Task<OperateResult> DeleteThoughtAsync(int id);
        Task<OperateResult<ThoughtDto>> QueryThoughtsAsync(ThoughtQuery query);
        Task<OperateResult> UpdateThoughtAsync(ThoughtEdit thought);
        Task<OperateResult<List<ThoughtInfo>>> GetThoughtListAsync();
        Task<OperateResult> AddLikeAsync(int id);
    }
}
