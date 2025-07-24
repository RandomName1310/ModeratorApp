using MySqlConnector;
using System.Data;
using System.Diagnostics;

namespace TEST_APP
{
    class DatabaseConnector
    {
        private string connectionString = "Server=localhost;Port=3306;Database=test_db;User=root;Password=pedro13102007#;";
        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            var dataTable = new DataTable();

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            Debug.WriteLine("Conexão criada");


            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            Debug.WriteLine("Leitor criado");

            dataTable.Load(reader);
            return dataTable;
        }
    }
}
