using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ModeratorApp;
public partial class LoginPage : ContentPage {
    public LoginPage() {
        InitializeComponent();
    }

    private async void Submited(object sender, EventArgs e) {
        string log = Login.Text;
        string passw = Password.Text;

        if (log == "amo" && passw == "123") {
            await Navigation.PushAsync(new MainPage());
        } else {
            await DisplayAlert("Login Falhou", "Coloque um login ou senha válido", "Tentar novamente");
        }
    }
}
