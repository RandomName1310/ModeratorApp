using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ModeratorApp.Services;

namespace ModeratorApp;

public partial class EventPage : ContentPage
{
    CardManager.event_data ev_data = new CardManager.event_data();
    public EventPage(CardManager.event_data data)
    {
        InitializeComponent();
        // get data from specific event
        ev_data = data;

        MainText.Text = "Event ID: " + data.event_id.ToString();
        DescriptionText.Text = data.description;
        Link.Text = "\nLink: " + data.link;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ShowClients();
    }

    private void ShowClients()
    {
        var c_manager = new CardManager(ClientStackLayout);
        string query = "SELECT volunteer_ID FROM Volunteer_Event WHERE Volunteer_Event.event_ID = @ev_data";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@ev_data", ev_data.event_id);

        DataTable? table = DatabaseConnector.ExecuteReadQuery(command);

        foreach (DataRow row in table.Rows)
        {
            string client_query = "SELECT name FROM Volunteers WHERE volunteer_ID = @client_id;";
            var client_command = new SqlCommand(client_query);
            client_command.Parameters.AddWithValue("@client_id", row["volunteer_ID"]);

            DataTable? client_table = DatabaseConnector.ExecuteReadQuery(client_command);

            if (client_table.Rows.Count > 0)
            {
                foreach (DataRow client_row in client_table.Rows) {
                    var client_data = new CardManager.client_data {
                        client_id = Convert.ToInt32(row["client_id"]),
                        name = client_row["name"].ToString() ?? "None",
                        color = GetRandomColor().ToHex()
                    };
                    c_manager.add_client(client_data, ev_data);
                }
            }
        }
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}
