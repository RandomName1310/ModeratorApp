using System.Data;
using System.Diagnostics;
using TEST_APP;

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
        await ExecuteQuery("SELECT * FROM students");
    }

    private async Task ExecuteQuery(string query)
    {
        var db = new DatabaseConnector();
        DataTable table = await db.ExecuteQueryAsync("SELECT * FROM students");

        foreach (DataRow row in table.Rows)
        {
           AddEvent(
                row["name"].ToString(),
                row["major"].ToString(),
                GetRandomColor().ToHex()
            );
        }
    }

    public void AddEvent(string M_Text, string D_Text, string BorderColor)
    {
        var mainLabel = new Label
        {
            Style = (Style)Resources["MainText"],
            Text = M_Text,
            HorizontalOptions = LayoutOptions.Start
        };

        var dateLabel = new Label
        {
            Style = (Style)Resources["DateText"],
            Text = D_Text,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.End
        };

        var button = new Button
        {
            Style = (Style)Resources["GeneralButtonStyle"],
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
        };
        button.Clicked += ClickAnim;

        var textStack = new VerticalStackLayout();
        textStack.Children.Add(mainLabel);
        textStack.Children.Add(dateLabel);
        textStack.Children.Add(button);


        var border = new Border
        {
            Style = (Style)Resources["BorderStyle"],
            BackgroundColor = Color.FromArgb(BorderColor),
            Content = textStack
        };

        EventStackLayout.Children.Add(border);
    }


    private async void ClickAnim(object sender, EventArgs e)
    {
        var button = (Button)sender;

        await button.ScaleTo(0.8, 60, Easing.Linear);
        await button.ScaleTo(1.0, 60, Easing.Linear);

        //await Navigation.PushAsync(new DescriptPage());
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
    }
}