using ModeratorApp.HelperClasses;

namespace ModeratorApp;

public partial class EventPage : ContentPage
{
    public EventPage(EventManager.event_data data)
    {
        InitializeComponent();

        MainText.Text = "Event ID: " + data.event_id.ToString() + "   Number Limit: " + data.number_limit.ToString();
        DescriptionText.Text = data.description;
        Link.Text = "\nLink: " + data.link;
    }
}
