using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;


namespace ModeratorApp.Services {
    static class DatabaseConnector {
        static string connectionString = "Server=DESKTOP-F17KATG\\SQLEXPRESS;Database=Amo_Database;User Id=AmoUser;Password=barbosa20;TrustServerCertificate=True;";

        public static DataTable ExecuteReadQuery(SqlCommand command) {
            var dataTable = new DataTable();

            try {
                using (var connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader()) {
                        dataTable.Load(reader);
                    }
                    Debug.WriteLine("Query executed successfully: " + command.CommandText);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return dataTable;
        }
        public static int ExecuteNonQuery(SqlCommand command) {
            int rowsAffected = 0;
            try {
                using (var connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    command.Connection = connection;
                    rowsAffected = command.ExecuteNonQuery();
                    Debug.WriteLine("query executed successfully: " + command.CommandText);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return rowsAffected;
        }

        // query that returns an object that is then converted to a type
        public static object? ExecuteScalarQuery(SqlCommand command) {
            object? result = null;
            try {
                using (var connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    command.Connection = connection;
                    result = command.ExecuteScalar();
                    Debug.WriteLine("Scalar query executed successfully: " + command.CommandText);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return result;
        }
    }
}