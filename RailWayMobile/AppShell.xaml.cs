using RailWayMobile.Pages;

namespace RailWayMobile
{
    public partial class AppShell : Shell
    {
        public Visibility ShellVisible
        {
            get
            {
                return Visibility.Visible;
                //return Preferences.Get("token", null) != null ? Visibility.Hidden : Visibility.Visible;
            }
            set
            {

            }
        }
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("BasketPage", typeof(Pages.BasketPage));
            Routing.RegisterRoute("LoginPage", typeof(Pages.LoginPage));
            Routing.RegisterRoute("RegistrationPage", typeof(Pages.RegistrationPage));
            Routing.RegisterRoute("EditProfilePage", typeof(Pages.EditProfilePage));
            RegisterLogic();
        }

        public async void RegisterLogic()
        {
            if (Preferences.Get("token", null) != null)
            {
                await GoToAsync("//MainPage");
            }
            else
            {
                var registrationPage = App.Current.Handler.MauiContext.Services.GetService<LoginPage>();
                await Navigation.PushAsync(registrationPage);
                //await GoToAsync("Registration");
            }
        }
        private async void Basket(object sender, EventArgs e)
        {
            await GoToAsync("BasketPage");
        }
    }
}
