using Microsoft.EntityFrameworkCore;
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
    public class ArticalServiceImpl : IArticalService
    {
        private readonly IUnitOfWork<BlogDbContext> _unitOfWork;
        private readonly HotMapRepository hotMapRepository;
        public ArticalServiceImpl(IUnitOfWork<BlogDbContext> unitOfWork, HotMapRepository hotMapRepository)
        {
            _unitOfWork = unitOfWork;
            this.hotMapRepository = hotMapRepository;
        }

        public async Task<OperateResult> AddArticle(ArticalAdd artical)
        {
            if (artical == null || string.IsNullOrEmpty(artical.Title) || string.IsNullOrEmpty(artical.Content))
            {
                return OperateResult.Failed("文章标题或内容不能为空");
            }
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(artical.CategoryId);
            if (category == null)
            {
                return OperateResult.Failed("分类不存在");
            }
            var article = new Article
            {
                Category = category,
                Title = artical.Title,
                Content = artical.Content,
                CategoryId = category.Id,
                IsPublished = artical.IsPublished,
                CreatedDate = DateTime.Now
            };
            if (artical.IsPublished)
            {
                article.PublishDate = DateTime.Now;
            }
            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            await hotMapRepository.AddHotMapAsync();
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> DeleteArtile(int id)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);
            if (article == null)
            {
                return OperateResult.Failed("文章不存在");
            }
            article.IsDeleted = true;
            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="artical"></param>
        /// <returns></returns>
        public async Task<OperateResult> UpdateArtile(ArticleUpdate artical)
        {
            if (artical == null || string.IsNullOrEmpty(artical.Title) || string.IsNullOrEmpty(artical.Content))
            {
                return OperateResult.Failed("文章标题或内容不能为空");
            }
            var dbArtical = await _unitOfWork.GetRepository<Article>().AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == artical.Id);
            if (dbArtical == null)
            {
                return OperateResult.Failed("文章不存在");
            }
            if (!string.IsNullOrWhiteSpace(dbArtical.Title))
            {
                dbArtical.Title = artical.Title;
            }
            if (!string.IsNullOrWhiteSpace(dbArtical.Content))
            {
                dbArtical.Content = artical.Content;
            }
            if (artical.CategoryId.HasValue)
            {
                var category = await _unitOfWork.GetRepository<Category>().FirstOrDefaultAsync(p => p.Id == artical.CategoryId.Value);
                if (category == null)
                {
                    return OperateResult.Failed("分类不存在");
                }
                dbArtical.Category = category;
            }
            if (artical.IsPublished.HasValue)
            {
                dbArtical.IsPublished = artical.IsPublished.Value;
            }

            await _unitOfWork.CommitAsync();
            await hotMapRepository.AddHotMapAsync();
            return OperateResult.Successed();
        }
        public async Task<OperateResult<ArticleInfo>> QuerySingleArticle(int id, bool isHtml = false, bool addViews = true)
        {
            var article = await _unitOfWork.GetRepository<Article>().Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
            if (article == null)
            {
                return OperateResult.Failed<ArticleInfo>("文章不存在");
            }
            if (addViews)
            {
                article.Views++;
                await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                await _unitOfWork.CommitAsync();
            }
            return OperateResult.Successed<ArticleInfo>(new ArticleInfo
            {
                CategoryName = article.Category.CategoryName,
                CategroyId = article.CategoryId,
                Content = isHtml ? article.Content.ConvertMarkdownToHtml() : article.Content,
                CreateTime = article.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Id = article.Id,
                Title = article.Title,
                views = article.Views,
                likes = article.Like
            });
        }
        /// <summary>
        /// 查询文章
        /// </summary>
        /// <param name="articleQuery"></param>
        /// <returns></returns>
        public async Task<OperateResult<ArticleDto>> QueryArticle(ArticleQuery articleQuery)
        {
            if (articleQuery == null)
            {
                return OperateResult.Failed<ArticleDto>("查询条件不能为空");
            }
            var start = articleQuery?.StartTime == null ? DateTime.MinValue : DateTime.TryParse(articleQuery.StartTime, out DateTime startTime) ? startTime : DateTime.MinValue;
            var end = articleQuery?.EndTime == null ? DateTime.MaxValue : DateTime.TryParse(articleQuery.EndTime, out DateTime endTime) ? endTime : DateTime.MaxValue;
            var query = _unitOfWork.GetRepository<Article>().AsNoTracking()
                        .Include(p => p.Category)
                        .Where(p => p.CreatedDate >= start && p.CreatedDate <= end);
            if (!string.IsNullOrWhiteSpace(articleQuery?.Title))
            {
                query = query.Where(p => p.Title.Contains(articleQuery.Title));
            }
            if (!string.IsNullOrEmpty(articleQuery?.CategoryName))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(articleQuery.CategoryName));
            }
            if (articleQuery?.IsPublished == true)
            {
                query = query.Where(p => p.IsPublished);
            }
            query = query.Where(p => !p.IsDeleted);
            var totalCount = await query.CountAsync();
            query.Include(p => p.Category);
            var articles = query.Skip(Math.Max(articleQuery.PageIndex - 1, 0) * Math.Max(articleQuery.PageSize, 1))
                .Take(Math.Max(articleQuery.PageSize, 1));
            var result = await articles.Select(p => new ArticleInfo
            {
                Id = p.Id,
                Title = p.Title,
                CategoryName = p.Category.CategoryName,
                CategroyId = p.CategoryId,
                CreateTime = p.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Content = MarkdownHelper.ExtractPlainTextFromMarkdown(p.Content, 200),
                views = p.Views,
                likes = p.Like
            }).ToListAsync();
            return OperateResult.Successed<ArticleDto>(new ArticleDto
            {
                TotalCount = totalCount,
                articleInfos = result
            });
        }
        /// <summary>
        /// 查询回收站文章
        /// </summary>
        /// <param name="articleQuery"></param>
        /// <returns></returns>
        public async Task<OperateResult<ArticleDto>> QueryGarbageArticle(ArticleQuery articleQuery)
        {

            if (articleQuery == null)
            {
                return OperateResult.Failed<ArticleDto>("查询条件不能为空");
            }
            var start = articleQuery?.StartTime == null ? DateTime.MinValue : DateTime.TryParse(articleQuery.StartTime, out DateTime startTime) ? startTime : DateTime.MinValue;
            var end = articleQuery?.EndTime == null ? DateTime.MaxValue : DateTime.TryParse(articleQuery.EndTime, out DateTime endTime) ? endTime : DateTime.MaxValue;
            var query = _unitOfWork.GetRepository<Article>().AsNoTracking()
                        .Include(p => p.Category)
                        .Where(p => p.CreatedDate >= start && p.CreatedDate <= end);
            if (!string.IsNullOrWhiteSpace(articleQuery?.Title))
            {
                query = query.Where(p => p.Title.Contains(articleQuery.Title));
            }
            if (!string.IsNullOrEmpty(articleQuery?.CategoryName))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(articleQuery.CategoryName));
            }
            if (articleQuery?.IsPublished == true)
            {
                query = query.Where(p => p.IsPublished);
            }
            query = query.Where(p => p.IsDeleted);
            var totalCount = await query.CountAsync();
            query.Include(p => p.Category);
            var articles = query.Skip(Math.Max(articleQuery.PageIndex - 1, 0) * Math.Max(articleQuery.PageSize, 1))
                .Take(Math.Max(articleQuery.PageSize, 1));
            var result = await articles.Select(p => new ArticleInfo
            {
                Id = p.Id,
                Title = p.Title,
                CategoryName = p.Category.CategoryName,
                CategroyId = p.CategoryId,
                CreateTime = p.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Content =MarkdownHelper.ExtractPlainTextFromMarkdown(p.Content, 200),
                views = p.Views,
                likes = p.Like,
            }).ToListAsync();
            return OperateResult.Successed<ArticleDto>(new ArticleDto
            {
                TotalCount = totalCount,
                articleInfos = result
            });
        }
        /// <summary>
        /// 恢复文章状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> RecoverArticle(int id)
        {
            var article = await _unitOfWork.GetRepository<Article>().AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (article == null)
            {
                return OperateResult.Failed("文章不存在");
            }
            article.IsDeleted = false;
            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }
        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> RealDelete(int id)
        {
            await _unitOfWork.GetRepository<Article>().DeleteAsync(id);
            return OperateResult.Successed();
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult> AddLikeAsync(int id)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);
            if (article == null)
            {
                return OperateResult.Failed("文章不存在");
            }
            article.Like++;
            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.CommitAsync();
            return OperateResult.Successed();
        }

        public async Task<OperateResult<List<ArticleInfo>>> QueryMostViewdArticles(int count)
        {
            var articles = await _unitOfWork.GetRepository<Article>().Where(p => !p.IsDeleted && p.IsPublished)
               .OrderByDescending(p => p.Views)
               .ThenBy(p => p.Like)
               .Take(count)
               .ToListAsync();
            var result = articles.Select(p => new ArticleInfo
            {
                Id = p.Id,
                Title = p.Title,
                CategoryName = p.Category.CategoryName,
                CategroyId = p.CategoryId,
                CreateTime = p.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Content = p.Content.ConvertMarkdownToHtml(),
                views = p.Views,
                likes = p.Like
            }).ToList();
            return OperateResult.Successed(result);
        }

    }
}
