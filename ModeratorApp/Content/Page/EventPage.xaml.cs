using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ModeratorApp.Cards;
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
        string query = "SELECT volunteer_ID FROM Volunteer_Event WHERE Volunteer_Event.event_ID = @ev_data";
        var command = new SqlCommand(query);
        command.Parameters.AddWithValue("@ev_data", ev_data.event_id);

        DataTable? table = DatabaseConnector.ExecuteReadQuery(command);

        HashSet<string> seen_volunteers = new HashSet<string>();

        foreach (DataRow row in table.Rows) {
            string client_query = "SELECT * FROM Volunteer WHERE volunteer_ID = @client_id;";
            var client_command = new SqlCommand(client_query);
            client_command.Parameters.AddWithValue("@client_id", row["volunteer_ID"]);

            var client_table = DatabaseConnector.ExecuteReadQuery(client_command);

            foreach (DataRow client_row in client_table.Rows) { 
                //if client doent exist, continue
                if (client_row == null)
                    continue;

                string? client_name = client_row["name"].ToString();

                //if name doent exist, continue
                if (client_name == null)
                    continue;

                //add name to HashSet if its not already there
                if (seen_volunteers.Contains(client_name)) {
                    //if client in Hash set
                    continue;

                }
                else {
                    //if client not in HashSet, add it
                    var client_data = new CardManager.client_data {
                        client_id = Convert.ToInt32(client_row["volunteer_ID"]),
                        name = client_name,
                        color = CardManager.GetRandomColor().ToHex()
                    };
                    ClientCard client_card = CardManager.add_client(client_data, ev_data, ClientStackLayout);
                    seen_volunteers.Add(client_name);

                    // add roles
                    var role_command = new SqlCommand(@"
                                                SELECT R.*
                                                FROM Roles R
                                                INNER JOIN Volunteer_Event VE ON R.role_ID = VE.role_ID
                                                WHERE VE.event_ID = @event_id AND VE.volunteer_ID = @volunteer_id;");

                    role_command.Parameters.AddWithValue("@event_id", ev_data.event_id);
                    role_command.Parameters.AddWithValue("@volunteer_id", client_row["volunteer_ID"]);

                    var role_table = DatabaseConnector.ExecuteReadQuery(role_command);

                    foreach (DataRow role_row in role_table.Rows) {
                        var role_data = new CardManager.RoleData {
                            role_id = Convert.ToInt32(role_row["role_ID"]),
                            name = role_row["name"].ToString() ?? "None",
                            color = CardManager.GetRandomColor().ToHex()
                        };
                        VerticalStackLayout role_stack = client_card.RoleStackLayout;
                        CardManager.add_sub_role_manage(role_data, role_stack);

                        Debug.WriteLine("Added Role to " + client_name);
                    }
                }
            }
        }
    }
}
