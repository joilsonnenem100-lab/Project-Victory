using Microsoft.Data.Sqlite;

var databasePath =
    Path.GetFullPath(
        Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "src",
            "ProjectVictory.API",
            "projectvictory.db"
        )
    );


var connection =
    new SqliteConnection(
        $"Data Source={databasePath}"
    );


connection.Open();


var command =
    connection.CreateCommand();


command.CommandText =
    "SELECT Id, Name, Description FROM Items";


using var reader =
    command.ExecuteReader();


while (reader.Read())
{
    Console.WriteLine(
        $"{reader.GetString(0)} | {reader.GetString(1)} | {reader.GetString(2)}"
    );
}