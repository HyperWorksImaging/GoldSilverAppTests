using Microsoft.Playwright;

[TestClass]
public static class GlobalTestFixture
{
    public static IPlaywright? Playwright { get; private set; }
    public static IBrowser? Browser { get; private set; }

    [AssemblyInitialize]
    public static async Task AssemblyInit(TestContext context)
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        if (Browser != null) await Browser.CloseAsync();
        Playwright?.Dispose();
    }
}