using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Logging;


namespace ModeratorApp.HelperClasses
{
    public class EventManager
    {
        private readonly ResourceDictionary _resources;
        private readonly Layout _eventStackLayout;
        private readonly INavigation _navigation;

        public struct event_data
        {
            public int event_id;
            public string name;
            public string description;
            public string date_time;
            public string link;
            public int number_limit;
            public string color;
        }

        public EventManager(ResourceDictionary resources, Layout eventStackLayout, INavigation navigation)
        {
            _resources = resources;
            _eventStackLayout = eventStackLayout;
            _navigation = navigation;
        }

        public void add_event(event_data ev_data)
        {
            var mainLabel = new Label
            {
                Style = (Style)_resources["MainText"],
                Text = ev_data.name,
                HorizontalOptions = LayoutOptions.Start
            };

            var dateLabel = new Label
            {
                Style = (Style)_resources["DateText"],
                Text = ev_data.date_time,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End
            };

            var button = new Button
            {
                Style = (Style)_resources["GeneralButtonStyle"],
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
            };
            button.Clicked += (s, e) => ClickAnim(s, e, ev_data);


            var textStack = new VerticalStackLayout();
            textStack.Children.Add(mainLabel);
            textStack.Children.Add(dateLabel);
            textStack.Children.Add(button);


            var border = new Border
            {
                Style = (Style)_resources["BorderStyle"],
                AutomationId = ev_data.name,
                BackgroundColor = Color.FromArgb(ev_data.color),
                Content = textStack
            };

            _eventStackLayout.Children.Add(border);
            remove_duplicates();
        }


        private void remove_duplicates()
        {
            var seenId = new HashSet<string>();
            var toRemove = new List<object>();

            foreach (var item in _eventStackLayout.Children.ToList())
            {
                if (seenId.Contains(item.AutomationId))
                {
                    toRemove.Add(item);
                }
                else
                {
                    seenId.Add(item.AutomationId);
                }
            }

            foreach (var item in toRemove)
            {
                _eventStackLayout.Children.Remove((IView)item);
            }
        }

        private async void ClickAnim(object sender, EventArgs e, event_data data)
        {
            var button = (Button)sender;

            await button.ScaleTo(0.8, 60, Easing.Linear);
            await button.ScaleTo(1.0, 60, Easing.Linear);

            // go to next page
            await _navigation.PushAsync(new EventPage(data));
        }
    }
}
