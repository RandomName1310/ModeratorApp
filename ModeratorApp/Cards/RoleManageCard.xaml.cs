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

    private async void OnCloseClicked(object sender, EventArgs e) {
        string query = $"DELETE FROM roles WHERE name = '{role_data.name}';";
        _ = await DatabaseConnector.ExecuteQueryAsync(query);
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}