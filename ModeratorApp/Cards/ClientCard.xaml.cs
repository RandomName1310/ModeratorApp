namespace ModeratorApp.Cards;

using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using ModeratorApp.Services;
using System.Diagnostics;

public partial class ClientCard : ContentView {
    private CardManager.client_data client_data;
    private CardManager.event_data event_data;

    private bool _isExpanded = false;
    private double _initialHeight;
    private double expansion_size = 200;

    public ClientCard(CardManager.client_data c_data, CardManager.event_data e_data) {
        InitializeComponent();
        client_data = c_data;
        event_data = e_data;

        BindingContext = c_data;
        _initialHeight = RootBorder.HeightRequest > 0 ? RootBorder.HeightRequest : RootBorder.Height;
    }
    private void OnExpandClicked(object sender, EventArgs e) {
        double from, to;

        if (!_isExpanded) {
            // Expand
            from = _initialHeight;
            to = _initialHeight + expansion_size; 
            _isExpanded = true;
        } else {
            // Retract
            from = _initialHeight + expansion_size;
            to = _initialHeight;
            _isExpanded = false;
        }

        var animation = new Animation(v => RootBorder.HeightRequest = v, from, to);
        animation.Commit(RootBorder, "Expand", 16, 700, Easing.CubicOut);
    }



    private void OnRemoveClicked(object sender, EventArgs e) {
        if (sender is Button btn) {
            if (!event_data.Equals(default(CardManager.event_data)) && btn.BackgroundColor == Colors.Red) {
                string query = "DELETE FROM Volunteer_Event WHERE client_ID = @client_id AND event_ID = @event_id;";
                var command = new SqlCommand(query);
                command.Parameters.AddWithValue("@client_id", client_data.client_id);
                command.Parameters.AddWithValue("@event_id", event_data.event_id);
                DatabaseConnector.ExecuteNonQuery(command);

                btn.BackgroundColor = Colors.Gray;
                btn.Text = "Removed";
            }
        }
    }
}
