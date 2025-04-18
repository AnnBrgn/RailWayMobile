using Azure;
using RailWayMobile.Behaviors;
using RailWayMobile.ClassesDTO;
using RailWayMobile.Utils;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RailWayMobile.Pages;

public partial class RegistrationPage : ContentPage
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Birthday { get; set; }
    public string NumberPhone { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string SelectedGender { get; set; } = "Male";
    public List<string> Genders { get; } = new List<string> { "Male", "Female"};

    private readonly HttpClient _httpClient;

    public RegistrationPage(HttpClient httpClient)
    {
        InitializeComponent();
        _httpClient = httpClient;
        this.BindingContext = this;
    }

    public async void Register()
    {
        //// Validate input
        if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
        {
            await DisplayAlert("Error", "Login and password are required.", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Name))
        {
            await DisplayAlert("Поле ИМЯ пустое", "Введите ИМЯ", "OK");
            return;
        }
        if (string.IsNullOrEmpty(LastName))
        {
            await DisplayAlert("Поле ФАМИЛИЯ пустое", "Введите ФАМИЛИЮ", "OK");
            return;
        }
        //if (string.IsNullOrEmpty(Patronymic))
        //{
        //    await DisplayAlert("Поле ОТЧЕСТВО пустое", "Введите ОТЧЕСТВО", "OK");
        //    return;
        //}
        if (!DateOnly.TryParse(Birthday, out _))
        {
            await DisplayAlert("Поле ДАТА РОЖДЕНИЯ введено неверно", "Введите ДАТУ РОЖДЕНИЯ", "OK");
            return;
        }
        if (!PhoneNumberEntryBehavior.PhoneRegex.IsMatch(NumberPhone))
        {
            await DisplayAlert("Поле НОМЕР ТЕЛЕФОНА введено неверно", "Введите НОМЕР ТЕЛЕФОНА", "OK");
            return;
        }
        if (!EmailEntryBehavior.EmailRegex.IsMatch(Email))
        {
            await DisplayAlert("Поле ЭЛ. ПОЧТА введено неверно", "Введите ЭЛ. ПОЧТУ", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Login))
        {
            await DisplayAlert("Поле ЛОГИН пустое", "Введите ЛОГИН", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Password))
        {
            await DisplayAlert("Поле ПАРОЛЬ пустое", "Введите ПАРОЛЬ", "OK");
            return;
        }
        // Create the request payload
        var user = new UserDTO
        {
            Birthday = DateOnly.Parse(Birthday),
            Email = Email,
            Lastname = LastName,
            Name = Name,
            Patronymic = Patronymic,
            NumberPhone = NumberPhone,
            Gender = SelectedGender,
            Login = Login,
            Password = Password
        };
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // Send the request to the API
            var response = await _httpClient.PostAsync("https://localhost:7137/api/User/Register", content);

            if (response.IsSuccessStatusCode)
            {
                await AppShell.Current.GoToAsync("LoginPage");
            }
            else
            {
                await DisplayAlert("Error " + response.StatusCode, "Registration failed. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
    private void CreateAccount(object sender, EventArgs e)
    {
        Register();
    }
}