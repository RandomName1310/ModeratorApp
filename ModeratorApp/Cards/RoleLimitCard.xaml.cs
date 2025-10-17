using ModeratorApp.Services;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace ModeratorApp.Cards;

public partial class RoleLimitCard : ContentView {
    private CardManager.RoleData role_data;
    private CardManager.EventData event_data;
    public string RoleName { get; private set; }

    public RoleLimitCard(CardManager.RoleData _role_data, CardManager.EventData _event_data) {
        InitializeComponent();
        role_data = _role_data;
        event_data = _event_data;
        BindingContext = role_data;

        // get role limit
        var command = new SqlCommand("SELECT number_limit FROM Event_Role WHERE role_ID = @role_id AND event_ID = @event_id");
        command.Parameters.AddWithValue("@role_id", role_data.role_id);
        command.Parameters.AddWithValue("@event_id", event_data.event_id);

        int r_id = Convert.ToInt32(DatabaseConnector.ExecuteScalarQuery(command));
        NumLimitLabel.Text = $"Vagas disponíveis: {r_id}";
        RoleName = role_data.name;
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}
