using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using ModeratorApp.Cards;

namespace ModeratorApp.Services {
    public class CardManager {
        private readonly Layout _eventStackLayout;
        private event_data _ev_data;

        public struct event_data {
            public int event_id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string date_time { get; set; }
            public string link { get; set; }
            public int number_limit { get; set; }
            public string color { get; set; }
        }

        public struct client_data {
            public int client_id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
        }

        public CardManager(Layout eventStackLayout, event_data ev_data = new event_data()) {
            _eventStackLayout = eventStackLayout;
            _ev_data = ev_data;
        }

        public void add_event(event_data e_data) {
            var card = new EventCard(e_data);
            _eventStackLayout.Children.Add(card);
        }

        public void add_client(client_data c_data, event_data e_data) {
            var card = new ClientCard(c_data, e_data);
            _eventStackLayout.Children.Add(card);
        }

        public static void RemoveDuplicates(Layout layout) {
            var seenId = new HashSet<string>();
            var toRemove = new List<object>();

            foreach (var item in layout.Children.ToList()) {
                if (seenId.Contains(item.AutomationId)) {
                    toRemove.Add(item);
                } else {
                    seenId.Add(item.AutomationId);
                }
            }

            foreach (var item in toRemove) {
                layout.Children.Remove((IView)item);
            }
        }
    }
}
