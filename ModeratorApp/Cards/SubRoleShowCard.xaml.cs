using ModeratorApp.Services;
using System.Diagnostics;

namespace ModeratorApp.Cards;

public partial class SubRoleShowCard : ContentView {
    private CardManager.RoleData role_data;
    public string RoleName { get; private set; }

    public SubRoleShowCard(CardManager.RoleData _role_data) {
        InitializeComponent();
        role_data = _role_data;
        BindingContext = role_data;

        RoleName = role_data.name;
    }

    public int num_limit => Convert.ToInt32(NumLimit.Text);

    private void OnCloseClicked(object sender, EventArgs e) {
        if (this.Parent is Layout parentLayout) {
            parentLayout.Children.Remove(this);
        }
    }
}
