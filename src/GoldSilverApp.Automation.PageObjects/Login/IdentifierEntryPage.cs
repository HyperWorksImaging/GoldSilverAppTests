using GoldSilverApp.Automation.Core;
using Microsoft.Playwright;

namespace GoldSilverApp.Automation.PageObjects.Login;

public class IdentifierEntryPage : BasePage
{
    public override string RelativePath => "/";
    private ILocator IdentifierInput => _page.Locator("[data-cy='input-username']");
    private ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { Name = "CONTINUE" });

    private ILocator ErrorMessage => _page.GetByText("User with provided username or email does not exist");

    public IdentifierEntryPage(IPage page, string appUrl) : base(page, appUrl) { }

    public Task<IdentifierEntryPage> GotoAsync() => NavigateToPageAsync<IdentifierEntryPage>(RelativePath);

    public async Task<IdentifierEntryPage> EnterIdentifierAsync(string identifier)
    {
        await IdentifierInput.FillAsync(identifier);
        return this;
    }

    public async Task<PasswordEntryPage> ContinueAsync()
    {
        await ContinueButton.ClickAsync();
        await _page.WaitForURLAsync(url => url.Contains("/login-user"));
        return new PasswordEntryPage(_page, _appUrl);
    }

    public async Task<IdentifierEntryPage> ContinueExpectingErrorAsync()
    {
        await ContinueButton.ClickAsync();
        return this;
    }

    public async Task<bool> IsErrorMessageVisibleAsync()
    {
        try
        {
            await ErrorMessage.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public async Task<string> GetErrorMessageAsync() => await ErrorMessage.InnerTextAsync();
}