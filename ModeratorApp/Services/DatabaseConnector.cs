using MySqlConnector;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ModeratorApp.Services {
    static class DatabaseConnector
    {
        static private string connectionString = "Server=localhost;Port=3306;Database=test;User=root;Password=pedro13102007#;";
        static public async Task<DataTable?> ExecuteQueryAsync(string query)
        {
            var dataTable = new DataTable();

            try {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                dataTable.Load(reader);
                return dataTable;
            } catch(Exception ex){
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
