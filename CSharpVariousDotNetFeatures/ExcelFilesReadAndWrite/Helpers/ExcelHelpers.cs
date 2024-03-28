using System.Drawing;
using OfficeOpenXml;

namespace ExcelFilesReadAndWrite.Helpers
{
    public class ExcelHelpers
    {
        public FileInfo testFile = new(@"C:\\Repositories\\GitHub Repositories\\CSharpVariousDotNetFeatures\\CSharpVariousDotNetFeatures\\ExcelFilesReadAndWrite\\ExcelFiles\\Excel_Test_File.xlsx");

        public static List<PersonModel> GetSetupData()
        {
            List<PersonModel> output =
            [
                new() { Id = 1, FirstName = "John", LastName = "Smith" },
                new() { Id = 2, FirstName = "Jane", LastName = "Doe" },
                new() { Id = 3, FirstName = "Alan", LastName = "MacMillan" }
            ];

            return output;
        }

        public static async Task<List<PersonModel>> LoadExcelFile(FileInfo file)
        {
            List<PersonModel> output = [];

            using var package = new ExcelPackage(file);

            await package.LoadAsync(file);

            var worksheet = package.Workbook.Worksheets[0];

            int row = 3;
            int col = 1;

            while (string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value?.ToString()) == false)
            {
                PersonModel p = new()
                {
                    Id = int.Parse(worksheet.Cells[row, col].Value.ToString()),
                    FirstName = worksheet.Cells[row, col + 1].Value.ToString(),
                    LastName = worksheet.Cells[row, col + 2].Value.ToString()
                };
                output.Add(p);
                row++;
            }

            return output;
        }

        public static async Task SaveExcelFile(List<PersonModel> people, FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }

            using var package = new ExcelPackage(file);

            //Apply Naming to the First Tab.
            var worksheet = package.Workbook.Worksheets.Add("MainReport");

            //Apply formatting to range of Cells.
            var range = worksheet.Cells["A2"].LoadFromCollection(people, true);
            range.AutoFitColumns();

            //Format header.
            worksheet.Cells["A1"].Value = "Report Heading";
            worksheet.Cells["A1:C1"].Merge = true;
            worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.Font.Size = 24;
            worksheet.Row(1).Style.Font.Color.SetColor(Color.Blue);

            worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Row(2).Style.Font.Bold = true;

            worksheet.Column(3).Width = 20;

            await package.SaveAsync();
        }
    }
}
