using Microsoft.Data.SqlClient;
using ModeratorApp.Services;
namespace ModeratorApp.Cards;

public partial class RoleManageCard : ContentView
{
    CardManager.RoleData role_data;

    public RoleManageCard(CardManager.RoleData _role_data)
	{
        InitializeComponent();
        role_data = _role_data;
        BindingContext = _role_data;
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        string query = $"DELETE FROM Roles WHERE name = @role_name;";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@role_name", role_data);
        DatabaseConnector.ExecuteNonQuery(command);

        // destroy this object
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}