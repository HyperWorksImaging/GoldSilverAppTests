using Microsoft.Playwright;

namespace GoldSilverApp.Automation.PageObjects
{
    public abstract class BasePage
    {
        protected readonly IPage _page;

        protected readonly String _appUrl;
        public abstract string RelativePath { get; }  

        public BasePage(IPage page, string appUrl)
        {
            _page = page;
            _appUrl=appUrl;

        }
        
        protected async Task<T> NavigateToPageAsync<T>(string RelativePath) where T: BasePage
        {
            string url = _appUrl + RelativePath;
            await _page.GotoAsync(url);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            return (T)this;
        }
    }

}