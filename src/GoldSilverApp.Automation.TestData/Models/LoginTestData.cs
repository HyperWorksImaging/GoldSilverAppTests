namespace GoldSilverApp.Automation.TestData.Models;

public class LoginTestData
{
    public string TestCaseId { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;   // email or username
    public string Password { get; set; } = string.Empty;
    public string ExpectedOutcome { get; set; } = string.Empty; // Success | InvalidPassword | Lockout
    public string Priority { get; set; } = string.Empty;
}