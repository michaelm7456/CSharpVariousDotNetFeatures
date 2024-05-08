using System.Data.SqlClient;

string dataSource = "Michael-Gaming-\\SQLEXPRESS";
string database1 = "Northwind";
string database2 = "AdventureWorks";
string userID = "michael";
string password = "123456";

//'SQL Server Authentication Mode'
//Note, this requires 'Data Source', 'Initial Catalog', the 'SQL Username' and the 'SQL Password'.
List<string> sqlServer_ConnectionStringsList =
[
    $"Data Source={dataSource};Initial Catalog={database1};User ID={userID};Password={password}",
    $"Data Source={dataSource};Initial Catalog={database2};User ID={userID};Password={password}"
];

//'Windows Authentication Mode'
//Note only 'Data Source', 'Initial Catalog' and 'Integrated Security' are required.
List<string> windows_ConnectionStringsList =
[
    $"Data Source={dataSource};Initial Catalog={database1};Integrated Security=true",
    $"Data Source={dataSource};Initial Catalog={database2};Integrated Security=true"
];

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Checking Connection Strings via 'Windows Authentication'.");
CheckConnectionStrings(windows_ConnectionStringsList);

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Checking Connection Strings via 'SQL Server Authentication'.");
CheckConnectionStrings(sqlServer_ConnectionStringsList);

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("Connection Strings checks complete.");
Console.ForegroundColor = ConsoleColor.White;

static void CheckConnectionStrings(List<string> connectionStringsList)
{
    foreach (var connectionString in connectionStringsList)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Connecting to: {connectionString}");

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "select 1";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Executing: {query}");

                var command = new SqlCommand(query, connection);

                connection.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SQL Connection successful.");

                command.ExecuteScalar();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SQL Query execution successful.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failure: {ex.Message}");
        }
    }
}