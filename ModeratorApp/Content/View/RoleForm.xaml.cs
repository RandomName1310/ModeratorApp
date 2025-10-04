using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Storage;
using ModeratorApp.Services;

namespace ModeratorApp;

public partial class RoleForm : ContentView
{
	public RoleForm()
	{
		InitializeComponent();
		ShowRoles();
	}

	async void ShowRoles() {
        // remove all current elements
        RoleStack.Children.Clear();

        string query = "SELECT * FROM roles";
        var command = new SqlCommand(query);
        DataTable? table = DatabaseConnector.ExecuteReadQuery(command);

        foreach (DataRow row in table.Rows) {
            var role_manager = new CardManager(RoleStack);
            var role_data = new CardManager.RoleData {
                role_id = Convert.ToInt32(row["role_id"]),
                name = row["name"].ToString() ?? "None",
                color = GetRandomColor().ToHex()
            };

            role_manager.add_role_manage(role_data);
        }
	}

    async void AddRole(object sender, EventArgs e) {
        string query = $"INSERT INTO Roles(name) VALUES(@role_name);";
        var client_command = new SqlCommand(query);
        client_command.Parameters.AddWithValue("@role_name", RoleEntry.Text);
        DatabaseConnector.ExecuteNonQuery(client_command);
        ShowRoles();
    }
    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }

    private Color GetRandomColor() {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}