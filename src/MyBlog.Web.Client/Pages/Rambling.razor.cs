using Microsoft.AspNetCore.Components;
using MyBlog.Models.Dto;
using MyBlog.Models.Models;
using MyBlog.Web.Client.Api.Apis;

namespace MyBlog.Web.Client.Pages
{
    public class RamblingBase : ComponentBase
    {
        [Inject]
        public ThoughtApiService ThoughtApiService { get; set; }

        protected List<ThoughtInfo> Thoughts { get; set; } = new();
        protected int CurrentPage { get; set; } = 1;
        protected int PageSize { get; set; } = 10;
        protected int TotalCount { get; set; }
        protected int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        protected override async Task OnInitializedAsync()
        {
            await LoadThoughts();
        }

        private async Task LoadThoughts()
        {
            var query = new ThoughtQuery
            {
                PageIndex = CurrentPage,
                PageSize = PageSize
            };

            var result = await ThoughtApiService.QueryThoughtsAsync(query);
            if (result.Succeeded && result.Data != null)
            {
                Thoughts = result.Data.ThoughtInfos;
                TotalCount = result.Data.TotalCount;
            }
        }

        protected async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage = page;
            await LoadThoughts();
        }
    }
}
