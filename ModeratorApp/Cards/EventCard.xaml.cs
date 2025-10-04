using Microsoft.Data.SqlClient;
using ModeratorApp.Services;
using System.Data;
namespace ModeratorApp.Cards;

public partial class EventCard : ContentView
{
    private CardManager.event_data event_data;

    public EventCard(CardManager.event_data e_data) {
        InitializeComponent();
        event_data = e_data;

        BindingContext = e_data;
    }

    public async void ViewEvent(object sender, EventArgs e) {
        var button = (Button)sender;

        await button.ScaleTo(0.8, 60, Easing.Linear);
        await button.ScaleTo(1.0, 60, Easing.Linear);

        // goto next page
        await Navigation.PushAsync(new EventPage(event_data));

    }
    public void RemoveEvent(object sender, EventArgs e) {
        if (sender is Button btn) {
            if (!event_data.Equals(default(CardManager.event_data)) && btn.BackgroundColor == Colors.Red) {
                string deleteClientsQuery = "DELETE FROM Volunteer_Event WHERE event_ID = @event_id";
                string deleteEventQuery = "DELETE FROM Events WHERE event_ID = @event_id;";


                var deleteClientscommand = new SqlCommand(deleteClientsQuery);
                var deleteEventcommand = new SqlCommand(deleteEventQuery);

                deleteClientscommand.Parameters.AddWithValue("@event_id", event_data.event_id);
                deleteEventcommand.Parameters.AddWithValue("@event_id", event_data.event_id);

                DatabaseConnector.ExecuteNonQuery(deleteClientscommand);
                DatabaseConnector.ExecuteNonQuery(deleteEventcommand);

                btn.BackgroundColor = Colors.Gray;
                btn.Text = "Removed";
            }
        }
    }
}