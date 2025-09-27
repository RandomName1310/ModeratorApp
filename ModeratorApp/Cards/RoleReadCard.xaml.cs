using ModeratorApp.Services;
using System.Diagnostics;   
namespace ModeratorApp.Cards;

public partial class RoleReadCard : ContentView
{
    private CardManager.RoleData role_data;
    public RoleReadCard(CardManager.RoleData _role_data)
	{
		InitializeComponent();
		role_data = _role_data;
		BindingContext = role_data;
    }

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }

}