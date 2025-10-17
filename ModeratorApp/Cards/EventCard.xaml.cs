using Microsoft.Data.SqlClient;
using ModeratorApp.Services;
using System.Data;
using System.Threading;
namespace ModeratorApp.Cards;

public partial class EventCard : ContentView
{
    private CardManager.EventData event_data;

    public EventCard(CardManager.EventData e_data) {
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
            if (!event_data.Equals(default(CardManager.EventData)) && btn.BackgroundColor == Colors.Red) {
                string deleteClientsQuery = "DELETE FROM Volunteer_Event WHERE event_ID = @event_id;";
                string deleteRoleQuery = "DELETE FROM Event_Role WHERE event_ID = @event_id;";
                string deleteEventQuery = "DELETE FROM Events WHERE event_ID = @event_id;";

                var deleteClientsCommand = new SqlCommand(deleteClientsQuery);
                var deleteRoleCommand = new SqlCommand(deleteRoleQuery);
                var deleteEventCommand = new SqlCommand(deleteEventQuery);

                deleteClientsCommand.Parameters.AddWithValue("@event_id", event_data.event_id);
                deleteRoleCommand.Parameters.AddWithValue("@event_id", event_data.event_id);
                deleteEventCommand.Parameters.AddWithValue("@event_id", event_data.event_id);

                try {
                    DatabaseConnector.ExecuteNonQuery(deleteClientsCommand);
                    DatabaseConnector.ExecuteNonQuery(deleteRoleCommand);
                    DatabaseConnector.ExecuteNonQuery(deleteEventCommand);

                    btn.BackgroundColor = Colors.Gray;
                    btn.Text = "Removed";
                }
                catch (Exception ex) {
                    Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível remover o evento.", "OK");
                }
            }
        }
    }

}