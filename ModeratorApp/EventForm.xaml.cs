using System;
using System.Data;
using System.Xml.Linq;
using ModeratorApp.Services;

namespace ModeratorApp;

public partial class EventForm : ContentView
{
    VerticalStackLayout layout;

    public EventForm(VerticalStackLayout _layout)
	{
		InitializeComponent();
        layout = _layout;
	}

    private async void AddEventCard(object sender, EventArgs e) {
        string query = "INSERT INTO events(name, description, date_time, link, number_limit) VALUES('"
            + (NameEntry.Text ?? "None").Replace("'", "''") + "', '"
            + (DescriptionEntry.Text ?? "None").Replace("'", "''") + "', '"
            + (DateEntry.Text ?? "None").Replace("'", "''") + "', '"
            + (LinkEntry.Text ?? "None").Replace("'", "''") + "', "
            + 25
            + ");";

        DataTable? table = await DatabaseConnector.ExecuteQueryAsync(query);

        if (table == null) {
            await Application.Current.MainPage.DisplayAlert("Commando SQL não reconhecido", "Coloque dados válidos", "Tentar novamente");
            return;
        }
        var ev_manager = new CardManager(layout);

        var event_data = new CardManager.event_data {
            event_id = 10,
            name = NameEntry.Text ?? "None",
            description = DescriptionEntry.Text ?? "None",
            date_time = DateEntry.Text ?? "None",
            link = LinkEntry.Text ?? "None", 
            number_limit = 25,
            color = GetRandomColor().ToHex()
        };
        ev_manager.add_event(event_data);

        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this); 
        }
    }

    private Color GetRandomColor() {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}