using ExcelFilesReadAndWrite.Helpers;
using OfficeOpenXml;

ExcelHelpers _ExcelHelpers = new();

//We must declare License as 'Non Commercial' to indicate that we're using this for personal projects as opposed to Commercial purposes.
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//Generates Excel file based on our sample data.
await ExcelHelpers.SaveExcelFile(ExcelHelpers.GetSetupData(), _ExcelHelpers.testFile);

List<PersonModel> peopleFromExcel = await ExcelHelpers.LoadExcelFile(_ExcelHelpers.testFile);
foreach (var p in peopleFromExcel)
{
    Console.WriteLine($"{p.Id} {p.FirstName} {p.LastName}");
}

Console.ReadLine();