using ModeratorApp.Services;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace ModeratorApp.Cards;

public partial class SubRoleShowCard : ContentView {
    private CardManager.RoleData role_data;
    private CardManager.EventData event_data;
    private CardManager.ClientData client_data;
    public string RoleName { get; private set; }

    public SubRoleShowCard(CardManager.RoleData _role_data, CardManager.EventData _event_data, CardManager.ClientData _client_data) {
        InitializeComponent();
        role_data = _role_data;
        event_data = _event_data;
        client_data = _client_data;
        BindingContext = role_data;

        // get role limit
        var command = new SqlCommand("SELECT * FROM Volunteer_Event WHERE role_ID = @role_id AND event_ID = @event_id AND volunteer_ID = @volunteer_id");
        command.Parameters.AddWithValue("@role_id", role_data.role_id);
        command.Parameters.AddWithValue("@event_id", event_data.event_id);
        command.Parameters.AddWithValue("@volunteer_id", client_data.client_id);

        DataTable? table =DatabaseConnector.ExecuteReadQuery(command);

        if (table != null) {
            DateLabel.Text = $"Data: {DateOnly.FromDateTime(Convert.ToDateTime(table.Rows[0]["date"])).ToString()}";
            TimeBeginLabel.Text = $"Início: {table.Rows[0]["time_begin"]}";
            TimeEndLabel.Text = $"Fim: {table.Rows[0]["time_end"]}";
            RoleName = role_data.name;
        }
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}
