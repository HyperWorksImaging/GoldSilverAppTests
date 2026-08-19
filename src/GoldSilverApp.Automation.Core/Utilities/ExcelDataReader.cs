using ClosedXML.Excel;

namespace GoldSilverApp.Automation.Core.Utilities;

public static class ExcelDataReader
{
    public static List<T> ReadSheet<T>(string filePath, string sheetName) where T : new()
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Test data file not found: {filePath}");

        using var workbook = new XLWorkbook(filePath);
        var sheet = workbook.Worksheet(sheetName);

        var headerRow = sheet.Row(1);
        var headers = headerRow.CellsUsed()
            .Select(c => c.GetString().Trim())
            .ToList();

        var results = new List<T>();
        var properties = typeof(T).GetProperties();

        foreach (var row in sheet.RowsUsed().Skip(1))
        {
            var item = new T();

            for (int i = 0; i < headers.Count; i++)
            {
                var prop = properties.FirstOrDefault(p =>
                    string.Equals(p.Name, headers[i], StringComparison.OrdinalIgnoreCase));
                if (prop == null) continue;

                var cellValue = row.Cell(i + 1).GetString();
                if (string.IsNullOrEmpty(cellValue)) continue;

                var convertedValue = Convert.ChangeType(cellValue, prop.PropertyType);
                prop.SetValue(item, convertedValue);
            }

            results.Add(item);
        }

        return results;
    }
}