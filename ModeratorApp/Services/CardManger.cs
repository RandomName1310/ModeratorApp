﻿using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using ModeratorApp.Cards;

namespace ModeratorApp.Services {
    public static class CardManager {
        public struct event_data {
            public int event_id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string date { get; set; }
            public string time_begin { get; set; }
            public string time_end { get; set; }
            public string link { get; set; }
            public string color { get; set; }
        }

        public struct client_data {
            public int client_id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
        }

        public struct RoleData {
            public int role_id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
        }

        public static EventCard add_event(event_data e_data, Layout stackLayout) {
            var card = new EventCard(e_data);
            stackLayout.Children.Add(card);
            return card;
        }

        public static ClientCard add_client(client_data c_data, event_data e_data, Layout stackLayout) {
            var card = new ClientCard(c_data, e_data);
            stackLayout.Children.Add(card);
            return card;
        }

        public static RoleReadCard add_role(RoleData role_data, Layout stackLayout) {
            var card = new RoleReadCard(role_data);
            stackLayout.Children.Add(card);
            return card;
        }

        public static RoleManageCard add_role_manage(RoleData role_data, Layout stackLayout) {
            var card = new RoleManageCard(role_data);
            stackLayout.Children.Add(card);
            return card;
        }
        public static SubRoleShowCard add_sub_role_manage(RoleData role_data, Layout stackLayout) {
            var card = new SubRoleShowCard(role_data);
            stackLayout.Children.Add(card);
            return card;
        }
        public static Color GetRandomColor() {
            var random = new Random();
            return Color.FromRgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
        }
    }
}
