using Microsoft.Playwright;
using GoldSilverApp.Automation.Core;
using DotNetEnv;

[TestClass]
public static class GlobalTestFixture
{
    public static IPlaywright? Playwright { get; private set; }
    public static IBrowser? Browser { get; private set; }

    [AssemblyInitialize]
    public static async Task AssemblyInit(TestContext context)
    {
        LoadEnvFile();
        var env = Environment.GetEnvironmentVariable("TEST_ENV") ?? "qa";
        ConfigManager.Load(env);

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
    }

    private static void LoadEnvFile()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".env.local")))
            dir = dir.Parent;

        if (dir != null)
            Env.Load(Path.Combine(dir.FullName, ".env.local"));
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        if (Browser != null) await Browser.CloseAsync();
        Playwright?.Dispose();
    }
}