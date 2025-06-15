using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Utilites;

namespace MyBlog.Services.Services
{
    public interface IArticalService
    {
        Task<OperateResult> AddArticle(ArticalAdd artical);
        Task<OperateResult> DeleteArtile(int id);
        Task<OperateResult<ArticleDto>> QueryArticle(ArticleQuery articleQuery);
        Task<OperateResult<ArticleDto>> QueryGarbageArticle(ArticleQuery articleQuery);
        Task<OperateResult> RecoverArticle(int id);
        Task<OperateResult> UpdateArtile(ArticleUpdate artical);
        Task<OperateResult<ArticleInfo>> QuerySingleArticle(int id, bool isHtml = false, bool addViews = true);
        Task<OperateResult> RealDelete(int id);
        Task<OperateResult> AddLikeAsync(int id);
        Task<OperateResult<List<ArticleInfo>>> QueryMostViewdArticles(int count);
        Task<OperateResult<BlogDetailCount>> GetBlogDetailCount();
    }
}