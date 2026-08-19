using Microsoft.Playwright;
namespace GoldSilverApp.Automation.Core;
public static class BrowserFactory
{
    public static async Task<(IPlaywright, IBrowser)> LaunchAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        return (playwright, browser);
    }

    public static async Task<IBrowserContext> CreateContextAsync(IBrowser browser, string? storageStatePath = null)
    {
        var options = new BrowserNewContextOptions();
        if (storageStatePath != null && File.Exists(storageStatePath))
            options.StorageStatePath = storageStatePath;

        return await browser.NewContextAsync(options);
    }
}