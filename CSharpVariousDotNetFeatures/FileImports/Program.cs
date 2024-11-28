var fileName = "samplepayload.json";

var payloadFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
Console.WriteLine($"Importing sample payload at path: CSharpVariousDotNetFeatures\\CSharpVariousDotNetFeatures\\FileImports\\samplepayload.json");

var payload = File.ReadAllText(payloadFilePath);
Console.WriteLine();
Console.WriteLine("Payload contents are:");
Console.WriteLine();
Console.WriteLine(payload);

Console.ReadLine();