using System;
using System.Data;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using ModeratorApp.Services;

namespace ModeratorApp;

public partial class EventForm : ContentView
{
    VerticalStackLayout layout;


    public EventForm(VerticalStackLayout _layout)
	{
		InitializeComponent();
        InitEventPicker();
        layout = _layout;
	}

    private void InitEventPicker() {
        // clear picker before init
        RolePicker.Items.Clear();

        string query = "SELECT * FROM roles";
        var command = new SqlCommand(query);
        DataTable? table = DatabaseConnector.ExecuteReadQuery(command);

        if (table == null) return;
        foreach (DataRow row in table.Rows) {
            if (row != null) {
                RolePicker.Items.Add(row["name"].ToString());
            }
        }
    }

    private void OnRolePickerChange(object sender, EventArgs e) {
        var role_manager = new CardManager(RoleStack);
        var role_data = new CardManager.RoleData {
            role_id = 1,
            name = RolePicker.SelectedItem?.ToString() ?? "None",
            color = GetRandomColor().ToHex()
        };

        role_manager.add_role(role_data);
    }

    private async void AddEventCard(object sender, EventArgs e) {
        string query = @"INSERT INTO events(name, description, date, time_begin, time_end, link) 
                        VALUES(@name, @description, @date, @time_begin, @time_end, @link)";

        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@name", NameEntry.Text ?? "None");
        command.Parameters.AddWithValue("@description", DescriptionEntry.Text ?? "None");
        command.Parameters.AddWithValue("@date", DateEntry.Date.ToString() ?? "None");
        command.Parameters.AddWithValue("@time_begin", TimeBegin.Time.ToString() ?? "None");
        command.Parameters.AddWithValue("@time_end", TimeEnd.Time.ToString() ?? "None");
        command.Parameters.AddWithValue("@link", LinkEntry.Text ?? "None");
        int roles_affected = DatabaseConnector.ExecuteNonQuery(command);

        if (roles_affected == 0) {
            await Application.Current.MainPage.DisplayAlert("Commando SQL não reconhecido", "Coloque dados válidos", "Tentar novamente");
            return;
        }
        var ev_manager = new CardManager(layout);

        var event_data = new CardManager.event_data {
            event_id = 10,
            name = NameEntry.Text ?? "None",
            description = DescriptionEntry.Text ?? "None",
            date = DateEntry.Date.ToString() ?? "None",
            time_begin = TimeBegin.Time.ToString() ?? "None",
            time_end = TimeEnd.Time.ToString() ?? "None",
            link = LinkEntry.Text ?? "None", 
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