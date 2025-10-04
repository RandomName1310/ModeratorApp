using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ModeratorApp.Services;

namespace ModeratorApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        ExecuteQuery();
    }

    private void ExecuteQuery()
    {
        var ev_manager = new CardManager(EventStackLayout);

        var command = new SqlCommand("SELECT * FROM Events");
        DataTable table = DatabaseConnector.ExecuteReadQuery(command);

        foreach (DataRow row in table.Rows)
        {
            var event_data = new CardManager.event_data
            {
                event_id = Convert.ToInt32(row["event_id"]),
                name = row["name"].ToString() ?? "None",
                description = row["description"].ToString() ?? "None",
                date = row["date"].ToString() ?? "None",
                time_begin = row["time_begin"].ToString() ?? "None",
                time_end = row["time_end"].ToString() ?? "None",
                link = row["link"].ToString() ?? "None",
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
    private async void AddEvent(object sender, EventArgs e) {
        var button = (Button)sender;

        await button.ScaleTo(0.8, 60, Easing.Linear);
        await button.ScaleTo(1.0, 60, Easing.Linear);

        EventForm ev_form = new EventForm(EventStackLayout);
        MainGrid.Add(ev_form);
    }

    private async void ManageRole(object sender, EventArgs e) {
        var button = (Button)sender;

        await button.ScaleTo(0.8, 60, Easing.Linear);
        await button.ScaleTo(1.0, 60, Easing.Linear);

        RoleForm role_form = new RoleForm();
        MainGrid.Add(role_form);
    }
}