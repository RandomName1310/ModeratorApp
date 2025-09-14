namespace ModeratorApp
{
    public partial class AppShell : Shell
    {
        public AppShell() {
            InitializeComponent();

            // Register routes for pages you want to navigate to
            Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

    }
}
