using Microsoft.AspNetCore.Components;

namespace MyBlog.Web.Client.Shared
{
    public class FooterBase: ComponentBase
    {
        protected DateTime CurrentTime = DateTime.Now;
        private Timer? timer;

        protected override void OnInitialized()
        {
            timer = new Timer(UpdateTime, null, 0, 1000); // 每秒更新
        }

        private void UpdateTime(object? state)
        {
            CurrentTime = DateTime.Now;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
