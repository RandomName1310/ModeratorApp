using Microsoft.Data.SqlClient; 
using System.Data;
using System.Diagnostics;


namespace ModeratorApp.HelperClasses
{
    class DatabaseConnector
    {
        string connectionString = "Server=DESKTOP-F17KATG\\SQLEXPRESS;Database=Amo_Database;User Id=AmoUser;Password=barbosa20;TrustServerCertificate=True;";

        public DataTable ExecuteQuery(SqlCommand command)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    Debug.WriteLine("Query executed successfully: " + command.CommandText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return dataTable;
        }
    }
}
