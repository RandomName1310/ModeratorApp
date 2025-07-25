using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModeratorApp.HelperClasses;

namespace ModeratorApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ExecuteQuery("SELECT * FROM events");
    }

    private async Task ExecuteQuery(string query)
    {
        var db = new DatabaseConnector();
        var ev_manager = new EventManager(Resources, EventStackLayout, Navigation);
        DataTable table = await db.ExecuteQueryAsync(query);

        foreach (DataRow row in table.Rows)
        {
            var event_data = new EventManager.event_data
            {
                event_id = Convert.ToInt32(row["event_id"]),
                name = row["name"].ToString(),
                description = row["description"].ToString(),
                date_time = row["date_time"].ToString(),
                link = row["link"].ToString(),
                number_limit = Convert.ToInt32(row["number_limit"]),
                color = GetRandomColor().ToHex()
            };

           ev_manager.add_event(event_data);
        }
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}