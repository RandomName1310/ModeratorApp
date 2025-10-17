using System;
using System.Data;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using ModeratorApp.Services;
using ModeratorApp.Cards;

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

        string query = "SELECT * FROM Roles";
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
        var role_data = new CardManager.RoleData {
            role_id = 1,
            name = RolePicker.SelectedItem?.ToString() ?? "None",
            color = GetRandomColor().ToHex()
        };

        CardManager.add_role(role_data, RoleStack);
    }

    private async void AddEvent(object sender, EventArgs e) {
        // creating event on database
        string query = @"INSERT INTO Events(name, description, date, time_begin, time_end, link) 
                        VALUES(@name, @description, @date, @time_begin, @time_end, @link)
                        SELECT SCOPE_IDENTITY()";

        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@name", NameEntry.Text ?? "None");
        command.Parameters.AddWithValue("@description", DescriptionEntry.Text ?? "None");
        command.Parameters.AddWithValue("@date", DateEntry.Date.ToString() ?? "None");
        command.Parameters.AddWithValue("@time_begin", TimeBegin.Time.ToString() ?? "None");
        command.Parameters.AddWithValue("@time_end", TimeEnd.Time.ToString() ?? "None");
        command.Parameters.AddWithValue("@link", LinkEntry.Text ?? "None");
        int? event_id = Convert.ToInt32(DatabaseConnector.ExecuteScalarQuery(command));

        if (event_id == null) {
            await Application.Current.MainPage.DisplayAlert("Commando SQL não reconhecido", "Coloque dados válidos", "Tentar novamente");
            return;
        }

        // connect roles to event
        foreach(var child in RoleStack.Children) {
            if(child is RoleReadCard r_card) {
                // get role id
                var roleID_command = new SqlCommand("SELECT role_ID FROM Roles WHERE name = @role_name");
                roleID_command.Parameters.AddWithValue("@role_name", r_card.RoleName);
                int? role_id = Convert.ToInt32(DatabaseConnector.ExecuteScalarQuery(roleID_command));

                string role_query = @"INSERT INTO Event_Role(event_ID, role_ID, number_limit) 
                                      VALUES(@event_id, @role_id, @number_limit)";
                var role_command = new SqlCommand(role_query);
                role_command.Parameters.AddWithValue("@event_id", event_id.Value);
                role_command.Parameters.AddWithValue("@role_id", role_id.Value);
                role_command.Parameters.AddWithValue("@number_limit", r_card.num_limit);

                DatabaseConnector.ExecuteNonQuery(role_command);
            }
        }
        var event_data = new CardManager.EventData {
            event_id = event_id.Value,
            name = NameEntry.Text ?? "None",
            description = DescriptionEntry.Text ?? "None",
            date = DateEntry.Date.ToString() ?? "None",
            time_begin = TimeBegin.Time.ToString() ?? "None",
            time_end = TimeEnd.Time.ToString() ?? "None",
            link = LinkEntry.Text ?? "None", 
            color = GetRandomColor().ToHex()
        };
        CardManager.add_event(event_data, layout);

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