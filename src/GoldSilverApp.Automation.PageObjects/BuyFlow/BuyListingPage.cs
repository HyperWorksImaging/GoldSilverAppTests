using System.Reflection.Metadata;
using Microsoft.Playwright;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using GoldSilverApp.Automation.PageObjects.Components;

namespace GoldSilverApp.Automation.PageObjects.Login;

public class BuyListingPage : BasePage
{
    public override string RelativePath => "/buy";
    private ILocator PasswordInput => _page.GetByLabel("Password:");
    private ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { Name = "CONTINUE" });
    private ILocator UseDifferentIdentifierLink => _page.GetByText("Use different email address or username");
    private ILocator ForgotPasswordLink => _page.GetByText("Forgot your password?");

    //public ErrorBannerComponent ErrorBanner { get; }

    public BuyListingPage(IPage page, string appUrl) : base(page, appUrl)
    {
       // ErrorBanner = new ErrorBannerComponent(page);
    }

    public async Task<bool> IsLoadedAsync()
    {
        try 
        {
            await _page.WaitForFunctionAsync("() => document.readyState === 'interactive' || document.readyState === 'complete'");
            return true;
        } catch (Exception) {
            return false;
        }
    }
}