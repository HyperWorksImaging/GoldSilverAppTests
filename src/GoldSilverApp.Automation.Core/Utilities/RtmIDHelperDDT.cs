namespace GoldSilverApp.Automation.Core.Utilities;

public static class RtmIdHelperDDT
{
    /// <summary>
    /// Strips a numeric DDT-disambiguation suffix (e.g. "TC_LOGIN_008_03" -> "TC_LOGIN_008")
    /// so failure reports and RTM lookups resolve to the real inventory row.
    /// </summary>
    public static string ToRtmId(string testCaseId)
    {
        var parts = testCaseId.Split('_');
        if (parts.Length > 1 && parts[^1].All(char.IsDigit) && parts[^1].Length == 2)
            return string.Join('_', parts[..^1]);

        return testCaseId;
    }
}