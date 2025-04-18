using RailWayMobile.ClassesDTO;
using RailWayMobile.Utils;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RailWayMobile.Pages
{
    public partial class LoginPage : ContentPage
    {
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }


        private readonly HttpClient _httpClient;

        public LoginPage(HttpClient httpClient)
        {
            InitializeComponent();
            _httpClient = httpClient;
            BindingContext = this; // Set the binding context
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                await DisplayAlert("Error", "Login and password are required.", "OK");
                return;
            }

            // Create the request payload
            var user = new { Login, Password };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Send the request to the API
                var response = await _httpClient.PostAsync("https://localhost:7137/api/User/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var usr = await JsonSerializer.DeserializeAsync<UserDTO>(await response.Content.ReadAsStreamAsync());
                    
                    await DisplayAlert($"Hello {usr.Name} {usr.Token}!", "Login successful!", "OK");

                    AppManager.  currentUser = usr;

                    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usr.Token);
                    //response = await _httpClient.GetAsync("https://localhost:7137/WeatherForecast");

                    await AppShell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await DisplayAlert("Error " + response.StatusCode, "Login failed. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void Registration(object sender, EventArgs e)
        {
            await AppShell.Current.GoToAsync("RegistrationPage");
        }
    }
}