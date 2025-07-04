﻿using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyBlog.Web.Client.Api.Apis
{
    [Api]
    public class ArticleApiService
    {
        private readonly HttpClient _httpClient;

        public ArticleApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("client");
        }

        public async Task<OperateResult> AddArticleAsync(ArticalAdd article)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Article/AddArticle", article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult> DeleteArticleAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Article/DeleteArticle?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult<ArticleDto>> QueryPublishArticleAsync(ArticleQuery articleQuery)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Article/QueryPublishArticle", articleQuery);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<ArticleDto>>();
        }

        public async Task<OperateResult<ArticleDto>> QueryArticleAsync(ArticleQuery articleQuery)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Article/QueryArticle", articleQuery);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<ArticleDto>>();
        }

        public async Task<OperateResult<ArticleDto>> QueryGarbageArticleAsync(ArticleQuery articleQuery)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Article/QueryGarbageArticle", articleQuery);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<ArticleDto>>();
        }

        public async Task<OperateResult> RecoverArticleAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Article/RecoverArticle?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }

        public async Task<OperateResult> UpdateArticleAsync(ArticleUpdate article)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Article/UpdateArticle", article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult>();
        }
        public async Task<OperateResult<ArticleInfo>> QuerySingleArticle(int id, bool isHtml, bool addViews = true)
        {
            var response = await _httpClient.GetAsync($"/api/Article/QuerySingle?id={id}&isHtml={isHtml}&addViews={addViews}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<ArticleInfo>>();
        }
        public async Task<OperateResult<List<ArticleInfo>>> QueryPopularArticle()
        {
            var response = await _httpClient.GetAsync($"/api/Article/QueryPopularArticle?count=5");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<List<ArticleInfo>>>();
        }
        public async Task<OperateResult<BlogDetailCount>> GetBlogDetailCount()
        {
            var response = await _httpClient.GetAsync($"/api/Article/GetBlogDetailCount");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<BlogDetailCount>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult<LikeState>> CheckLikeState(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Article/CheckLikeState?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<LikeState>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperateResult<LikeState>> AddLike(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Article/AddLike?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OperateResult<LikeState>>();
        }
    }
}