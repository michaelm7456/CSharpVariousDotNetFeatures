using FileImports;
using System.Text.Json;

var fileName = "samplepayload.json";

var payloadFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"Importing sample payload at path: CSharpVariousDotNetFeatures\\CSharpVariousDotNetFeatures\\FileImports\\samplepayload.json");

var payload = File.ReadAllText(payloadFilePath);
var dto = JsonSerializer.Deserialize<CustomerDTO>(payload);

Console.WriteLine();
Console.WriteLine("Payload contents are:");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine(payload);
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Payload has been deserialised successfully into DTO.");
Console.ForegroundColor = ConsoleColor.White;

Console.ReadLine();