using System.Data;
using System.Diagnostics;
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

        MainText.Text = "Event ID: " + data.event_id.ToString() + "   Number Limit: " + data.number_limit.ToString();
        DescriptionText.Text = data.description;
        Link.Text = "\nLink: " + data.link;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ShowClients();
    }

    private async Task ShowClients()
    {
        string query = "SELECT client_id FROM event_client WHERE event_client.event_id = " + ev_data.event_id + ";";
        var c_manager = new CardManager(ClientStackLayout, ev_data);

        DataTable? table = await DatabaseConnector.ExecuteQueryAsync(query);

        foreach (DataRow row in table.Rows)
        {
            string client_query = "SELECT name FROM clients WHERE client_id = " + row["client_id"] + ";";
            DataTable? client_table = await DatabaseConnector.ExecuteQueryAsync(client_query);

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
