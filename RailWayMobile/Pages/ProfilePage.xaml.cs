using RailWayMobile.ClassesDTO;
using RailWayMobile.Utils;

namespace RailWayMobile.Pages;

public partial class ProfilePage : ContentPage
{
    public UserDTO User {
        get 
        {
            return _user;
        }
        set
        {
            _user = value;
            OnPropertyChanged();
        }
    }
    private UserDTO _user;
    public ProfilePage()
    {
        InitializeComponent();
        this.BindingContext = this;
        User = AppManager.currentUser;
    }
    protected override void OnAppearing()
    {
        User = AppManager.currentUser;
    }
    private async void EditProfile(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("EditProfilePage");
    }
}