using GoldSilverApp.Automation.Core.Utilities;
using GoldSilverApp.Automation.TestData.Models;

namespace GoldSilverApp.Automation.TestData.DataProviders;

public static class LoginDataProvider
{
    private const string FilePath = "DataFiles/DDTData.xlsx";
    private const string SheetName = "LoginData";

    public static IEnumerable<object[]> GetLoginData()
    {
        var rows = ExcelDataReader.ReadSheet<LoginTestData>(FilePath, SheetName);
        return rows.Select(r => new object[] { r });
    }
}