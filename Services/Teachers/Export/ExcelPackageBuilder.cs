using System.Linq;
using OfficeOpenXml;

namespace Api.Services.Teachers
{
    public class ExcelPackageBuilder
    {
        private readonly ExcelPackage _excelPackage;

        public ExcelPackageBuilder()
        {
            _excelPackage = new ExcelPackage();
        }

        public ExcelPackageBuilder WithTitle(string title)
        {
            _excelPackage.Workbook.Properties.Title = title;

            return this;
        }

        public ExcelPackageBuilder WithWorkSheet(string title, string[] headers, object[][] body)
        {
            _excelPackage.Workbook.Worksheets.Add(title);
            var worksheet = _excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == title);

            var data = body.Prepend(headers).ToArray();
            for (var row = 1; row <= data.Count(); row++)
            {
                for (var column = 1; column <= data[0].Count(); column++)
                {
                    worksheet.Cells[row, column].Value = data[row-1][column-1];
                }
            }

            return this;
        }

        public ExcelPackage Build() => _excelPackage;
    }
}
