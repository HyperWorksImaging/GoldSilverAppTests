using Microsoft.Playwright;

namespace GoldSilverApp.Automation.PageObjects;

public class LoginPage
{
    private readonly IPage _page;

    private ILocator EmailInput => _page.GetByLabel("Email");
    private ILocator PasswordInput => _page.GetByLabel("Password");
    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Log In" });

    public LoginPage(IPage page) => _page = page;

    public async Task<LoginPage> GotoAsync(string baseUrl)
    {
        await _page.GotoAsync($"{baseUrl}/login");
        return this;
    }

    public async Task<LoginPage> EnterEmailAsync(string email)
    {
        await EmailInput.FillAsync(email);
        return this;
    }

    public async Task<LoginPage> EnterPasswordAsync(string password)
    {
        await PasswordInput.FillAsync(password);
        return this;
    }

    // public async Task<DashboardPage> SubmitAsync()
    // {
    //     await LoginButton.ClickAsync();
    //     await _page.WaitForURLAsync(url => url.Contains("/dashboard"));
    //     return new DashboardPage(_page);
    // }

    // public async Task<DashboardPage> LoginAsync(string baseUrl, string email, string password)
    // {
    //     return await (await (await (await GotoAsync(baseUrl))
    //         .EnterEmailAsync(email))
    //         .EnterPasswordAsync(password))
    //         .SubmitAsync();
    // }
}