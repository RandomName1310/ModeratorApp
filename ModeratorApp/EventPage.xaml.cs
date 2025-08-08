using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ShowClients();
    }

    private void ShowClients()
    {
        var db = new DatabaseConnector();
        SqlCommand query = new SqlCommand("SELECT volunteer_id FROM Volunteer_Event WHERE Volunteer_Event.event_id = @ev_id;");
        query.Parameters.Add("@ev_id", SqlDbType.Int).Value = ev_data.event_id;

        DataTable table = db.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            SqlCommand client_query = new SqlCommand("SELECT name FROM Volunteer WHERE volunteer_ID = @client_id;");
            client_query.Parameters.Add("@client_id", SqlDbType.Int).Value = row["volunteer_ID"];

            DataTable client_table = db.ExecuteQuery(client_query);

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
