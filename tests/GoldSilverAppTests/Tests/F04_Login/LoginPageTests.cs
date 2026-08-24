using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoldSilverApp.Automation.Core;
using GoldSilverApp.Automation.PageObjects.Login;
using GoldSilverApp.Automation.TestData.DataProviders;
using GoldSilverApp.Automation.TestData.Models;
using GoldSilverApp.Automation.Tests.TestHooks;
using Microsoft.Playwright;
using System.Reflection;

namespace GoldSilverApp.Automation.Tests.F04_Login;

[TestClass]
public class LoginTests : BaseTestFixture
{
    private IdentifierEntryPage _identifierPage = null!;

    [TestInitialize]
    public void Setup()
    {
        _identifierPage = new IdentifierEntryPage(Page, ConfigManager.BaseUrl);
    }

    [TestMethod]
    [TestCategory("P0")]
    //[RtmTrace(tcId: "TC_LOGIN_001", featureId: "F-04", priority: "P0")]
    public async Task TC_LOGIN_001_LoginPageLoadsWithRequiredElements()
    {
        await _identifierPage.GotoAsync();
        Assert.IsTrue(await Page.Locator("text=Welcome To GoldSilver").IsVisibleAsync());
    }

    [TestMethod]
    [TestCategory("P0")]
    [RtmTrace(tcId: "TC_LOGIN_002", featureId: "F-04", priority: "P0")]
    public async Task TC_LOGIN_002_IdentifierContinuesToPasswordStep()
    {
        var passwordPage = await (await _identifierPage.GotoAsync())
            .EnterIdentifierAsync(ConfigManager.DefaultTestUserEmail)
            .Result.ContinueAsync();

        Assert.IsTrue(await Page.Locator("[data-cy='input-password']").IsVisibleAsync());
    }

    [TestMethod]
    [TestCategory("P2")]
    [RtmTrace(tcId: "TC_LOGIN_003", featureId: "F-04", priority: "P2")]
    public async Task TC_LOGIN_003_PasswordAppearsMasked()
    {
        var passwordPage = await (await _identifierPage.GotoAsync())
            .EnterIdentifierAsync(ConfigManager.DefaultTestUserEmail)
            .Result.ContinueAsync();

        Assert.IsTrue(await passwordPage.IsPasswordMaskedAsync(), "Password field should be masked by default");
        await passwordPage.TogglePasswordVisibilityAsync();
        Assert.IsFalse(await passwordPage.IsPasswordMaskedAsync(), "Password should be unmasked after toggling");
    }

    [TestMethod]
    [TestCategory("P3")]
    [RtmTrace(tcId: "TC_LOGIN_004", featureId: "F-04", priority: "P3")]
    public async Task TC_LOGIN_004_DifferentEmailLinkResetsIdentifierStep()
    {
        var passwordPage = await (await _identifierPage.GotoAsync())
            .EnterIdentifierAsync(ConfigManager.DefaultTestUserEmail)
            .Result.ContinueAsync();

        await passwordPage.UseDifferentIdentifierAsync();
        await Assertions.Expect(Page.Locator("text=Welcome To GoldSilver")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("P2")]
    [RtmTrace(tcId: "TC_LOGIN_005", featureId: "F-04", priority: "P2")]
    public async Task TC_LOGIN_005_ForgotPasswordLinkRedirectsToReset()
    {        
        var passwordPage = await (await _identifierPage.GotoAsync())
            .EnterIdentifierAsync(ConfigManager.DefaultTestUserEmail)
            .Result.ContinueAsync();

        await passwordPage.ClickForgotPasswordAsync();
        await Page.WaitForURLAsync(url => url.Contains("reset"));
        Assert.IsTrue(Page.Url.Contains("reset"));
    }

    [TestMethod]
    [RtmTrace(tcId: "TC_LOGIN_006, TC_LOGIN_007", featureId: "F-04", priority: "P0")]
    [DynamicData(nameof(LoginDataProvider.GetLoginData), typeof(LoginDataProvider), DynamicDataSourceType.Method)]
    public async Task TC_LOGIN_DDT_ValidatesCredentialOutcomes(LoginTestData data)
    {
        await _identifierPage.GotoAsync();
        await _identifierPage.EnterIdentifierAsync(data.Identifier);

        switch (data.ExpectedOutcome)
        {
            case "InvalidEmail":
            case "InvalidEmailFormat":
                
                await _identifierPage.ContinueExpectingErrorAsync();
                Assert.IsTrue(await _identifierPage.IsErrorMessageVisibleAsync(),
                    $"{data.TestCaseId}: expected identifier-step error");
                return; 
            default:
               
                var passwordPage = await _identifierPage.ContinueAsync();
                await passwordPage.EnterPasswordAsync(data.Password);

                switch (data.ExpectedOutcome)
                {
                    case "Success":
                        var buyListing = await passwordPage.ContinueToSuccessAsync();
                        Assert.IsTrue(await buyListing.IsLoadedAsync(), $"{data.TestCaseId}: expected dashboard load");
                        break;

                    case "InvalidPassword":
                        await passwordPage.ContinueExpectingErrorAsync();
                        Assert.IsTrue(await passwordPage.IsErrorMessageVisibleAsync(),
                            $"{data.TestCaseId}: expected generic auth error");
                        break;

                    case "Lockout":
                        await passwordPage.ContinueExpectingErrorAsync();
                        Assert.IsTrue(await passwordPage.IsErrorMessageVisibleAsync(),
                            $"{data.TestCaseId}: expected lockout/throttle message");
                        break;
                }
                break;
        }
    }

    // [TestMethod]
    // [TestCategory("P1")]
    // //[RtmTrace(tcId: "TC_LOGIN_008", featureId: "F-04", priority: "P1")]
    // public async Task TC_LOGIN_008_AccountLockedOutOnWrongPassword()
    // {
        
    // }

    // [TestMethod]
    // [TestCategory("P0")]
    // //[RtmTrace(tcId: "TC_LOGIN_009", featureId: "F-04", priority: "P0")]
    // public async Task TC_LOGIN_009_LogoutEndsSessionAndRedirectsProtectedRoutes()
    // {
        
    // }

    // [TestMethod]
    // [TestCategory("P1")]
    // //[RtmTrace(tcId: "TC_LOGIN_010", featureId: "F-04", priority: "P1")]
    // public async Task TC_LOGIN_010_LogoutEndsSessionAndRedirectsProtectedRoutes()
    // {
        
    // }

    // [TestMethod]
    // [TestCategory("P2")]
    // //[RtmTrace(tcId: "TC_LOGIN_011", featureId: "F-04", priority: "P2")]
    // public async Task TC_LOGIN_011_EmptyIdentifierInputDisplaysError()
    // {
        
    // }

    // [TestMethod]
    // [TestCategory("P2")]
    // //[RtmTrace(tcId: "TC_LOGIN_012", featureId: "F-04", priority: "P2")]
    // public async Task TC_LOGIN_012_EmptyPasswordInputDisplaysError()
    // {
        
    // }
}