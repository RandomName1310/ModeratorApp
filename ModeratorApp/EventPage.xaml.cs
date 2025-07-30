using System.Data;
using ModeratorApp.HelperClasses;

namespace ModeratorApp;

public partial class EventPage : ContentPage
{
    EventManager.event_data ev_data = new EventManager.event_data();
    public EventPage(EventManager.event_data data)
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
        var db = new DatabaseConnector();
        DataTable table = await db.ExecuteQueryAsync(query);

        foreach (DataRow row in table.Rows)
        {
            string client_query = "SELECT name FROM clients WHERE client_id = " + row["client_id"] + ";";
            DataTable client_table = await db.ExecuteQueryAsync(client_query);

            if (client_table.Rows.Count > 0)
            {
                ShowOnScreen(client_table.Rows[0]["name"].ToString());
            }
        }
    }

    private void ShowOnScreen(string name)
    {
        var label = new Label {
            Text = name,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 30,
        };

        VerticalStackLayout.Children.Add(label);
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}
