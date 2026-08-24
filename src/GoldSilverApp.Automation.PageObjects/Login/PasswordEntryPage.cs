using Microsoft.Playwright;
//using GoldSilverApp.Automation.PageObjects.Components;

namespace GoldSilverApp.Automation.PageObjects.Login;

public class PasswordEntryPage : BasePage
{
    public override string RelativePath => "/login-user";
    private ILocator PasswordInput => _page.GetByLabel("Password:");
    private ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { Name = "CONTINUE" });
    private ILocator UseDifferentIdentifierLink => _page.GetByText("Use different email address or username");
    private ILocator ForgotPasswordLink => _page.GetByText("Forgot your password?");
    private ILocator EyeIcon => _page.Locator("#password").Locator("xpath=following-sibling::*").Locator("button, svg, [role='button']").First;
    private ILocator ErrorMessage=> _page.GetByText("Invalid username or password.");

    //public ErrorBannerComponent ErrorBanner { get; }

    public PasswordEntryPage(IPage page, string appUrl) : base(page, appUrl)
    {
       // ErrorBanner = new ErrorBannerComponent(page);
    }

    public async Task<PasswordEntryPage> EnterPasswordAsync(string password)
    {
        await PasswordInput.FillAsync(password);
        return this;
    }

    public async Task<BuyListingPage> ContinueToSuccessAsync()
    {
        await ContinueButton.ClickAsync();
        await _page.WaitForURLAsync(url => url.Contains("/buy"));
        return new BuyListingPage(_page, _appUrl);
    }

    public async Task<PasswordEntryPage> ContinueExpectingErrorAsync()
    {
        await ContinueButton.ClickAsync();
        return this;
    }

    public async Task<IdentifierEntryPage> UseDifferentIdentifierAsync()
    {
        await UseDifferentIdentifierLink.ClickAsync();
        return new IdentifierEntryPage(_page, _appUrl);
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

    public async Task<bool> IsPasswordMaskedAsync() =>
    await _page.Locator("#password").GetAttributeAsync("type") == "password";

    public async Task TogglePasswordVisibilityAsync() => await EyeIcon.ClickAsync();

    public async Task ClickForgotPasswordAsync() => await ForgotPasswordLink.ClickAsync();

}