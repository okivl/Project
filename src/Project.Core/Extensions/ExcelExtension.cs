using ClosedXML.Excel;

namespace Project.Core.Extensions
{
    public static class ExcelExtension
    {
        public static void AddTableHeaders(this IXLWorksheet worksheet)
        {
            var headers = new[]
            {
            "Id дохода",
            "Наименование дохода",
            "Сумма",
            "Id расхода",
            "Наименование расхода",
            "Сумма"
            };

            var currentRow = 1;

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(currentRow, i + 1).Value = headers[i];
            }
        }
    }
}
