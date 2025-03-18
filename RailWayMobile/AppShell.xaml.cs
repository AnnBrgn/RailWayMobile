namespace RailWayMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("BasketPage", typeof(Pages.BasketPage));
            Routing.RegisterRoute("Registration", typeof(Pages.Registration));
            RegisterLogic();
        }

        public async void RegisterLogic()
        {
            if (Preferences.Get("token", null) != null)
            {
                await GoToAsync("Login");
            }
            else
            {
                await GoToAsync("Registration");
            }
        }
        private async void Basket(object sender, EventArgs e)
        {
            await GoToAsync("BasketPage");
        }
    }
}
