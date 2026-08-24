using Microsoft.Playwright;
using  GoldSilverApp.Automation.Core.Utilities;


namespace GoldSilverApp.Automation.PageObjects;

public class RegistrationPage : BasePage
{
    private ILocator EmailInput => _page.GetByPlaceholder("Enter your email");
    private ILocator PasswordInput => _page.GetByPlaceholder("Enter your password");
    private ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { Name = "Continue" });
    public override string RelativePath => "/registration";

    public ILocator ErrorMessages => _page.Locator("p");   
    public ILocator EmailError => _page.GetByText("Email address is required");
    public ILocator PasswordError => _page.GetByText("Your password doesn’t meet our security requirements");

    private ILocator FirstNameInput => _page.GetByPlaceholder("Enter First name");
    private ILocator LastNameInput => _page.GetByPlaceholder("Enter Last name");
    private ILocator CountryDropdown => _page.Locator(".selected-flag");
    private ILocator CountryOption(string countryCode) => _page.Locator($"li.country[data-dial-code='{countryCode}']");

    private ILocator TelephoneInput => _page.Locator("#PhoneNumber");

    private ILocator CreateAccountButton => _page.GetByRole(AriaRole.Button, new() { Name = "Create My Free Account"});

    public ILocator FirstNameError => _page.GetByText("Legal First Name is required");
    public ILocator LastNameError => _page.GetByText("Legal Last Name is required.");
    public ILocator PhoneNumberError => _page.GetByText("Phone Number is required.");


    public RegistrationPage(IPage page, string appUrl) : base(page, appUrl) {  }

    public async Task RegisterAsync(string email, string password)
    {
        await EmailInput.FillAsync(email);
        await PasswordInput.FillAsync(password);
        await PasswordInput.PressAsync("Tab");
    }

    public async Task ClickContinueButtonAsync()
    {
        await ContinueButton.ClickAsync();
    }

    public async Task<bool> IsContinueButtonEnabledAsync() => await ContinueButton.IsEnabledAsync();

    public async Task<bool> HasErrorContainingAsync(string expectedText)
    {
        return await _page.GetByText(expectedText, new() { Exact = false })
            .IsVisibleAsync();
    }

    public async Task FillPersonalDataAsync(string firstName, string lastName, string countryCode, string mobileNumber)
    {
        await FirstNameInput.FillAsync(firstName);
        await LastNameInput.FillAsync(lastName);
        await CountryDropdown.ClickAsync();
        await CountryOption(countryCode).First.ClickAsync();
        await TelephoneInput.FillAsync(mobileNumber);
        await TelephoneInput.PressAsync("Tab");
    }

    public async Task<bool> IsCreateAccountButtonEnabledAsync() => await CreateAccountButton.IsEnabledAsync();

    public async Task CompleteRegistrationAsync() => await CreateAccountButton.ClickAsync();
}