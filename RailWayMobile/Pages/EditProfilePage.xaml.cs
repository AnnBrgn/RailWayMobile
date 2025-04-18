using RailWayAPI.ClassesDTO;
using RailWayMobile.Behaviors;
using RailWayMobile.ClassesDTO;
using RailWayMobile.Utils;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RailWayMobile.Pages;

public partial class EditProfilePage : ContentPage
{
    public string SelectedGender { get; set; }
    public string Birthday { get; set; }
    public List<string> Genders { get; } = new List<string> { "Male", "Female" };
    private UserDTO user;
    private HttpClient _httpClient;
    public UserDTO User 
    { 
        get 
        {
            return user;
        } 
        set
        {
            user = value;
            OnPropertyChanged();
        }
    }
    public EditProfilePage(HttpClient httpClient)
    {
        InitializeComponent();
        this.BindingContext = this;
        
        User = AppManager.currentUser.Clone() as UserDTO;
        Birthday = User.Birthday.ToString();
        SelectedGender = User.Gender;

        OnPropertyChanged(nameof(SelectedGender));
        OnPropertyChanged(nameof(Birthday));

        _httpClient = httpClient;
    }

    public void LoadImage()
    {

    }

    private async void OnPickImageButtonClicked(object sender, EventArgs e)
    {
        byte[] imageBytes = await PickImageAndGetBytes();
        if (imageBytes != null)
        {
            // Теперь у вас есть массив байтов изображения
            // Можно сохранить в БД, отправить на сервер и т.д.
            var userData = new UpdateUserProfilePictureDTO
            {
                Picture = imageBytes,
                Token = AppManager.currentUser.Token
            };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                // Send the request to the API
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppManager.currentUser.Token);
                var response = await _httpClient.PostAsync("https://localhost:7137/api/UserProfile/UpdateUserProfileImg", content);

                if (response.IsSuccessStatusCode)
                {                              
                    await DisplayAlert("Успех", "Изображение загружено в byte[]", "OK");

                    //await AppShell.Current.GoToAsync("//MainPage");
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

    }

    public async Task<byte[]> PickImageAndGetBytes()
    {
        try
        {
            // Запрашиваем файл у пользователя
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images // Только изображения
            });

            if (result == null)
                return null; // Пользователь отменил выбор

            // Проверяем, что файл существует
            if (!File.Exists(result.FullPath))
                return null;

            // Читаем файл в массив байтов
            byte[] imageBytes = await File.ReadAllBytesAsync(result.FullPath);
            return imageBytes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить изображение: {ex.Message}", "OK");
            return null;
        }
    }

    private async void EditData(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            await DisplayAlert("Поле ИМЯ пустое", "Введите ИМЯ", "OK");
            return;
        }
        if (string.IsNullOrEmpty(user.Lastname))
        {
            await DisplayAlert("Поле ФАМИЛИЯ пустое", "Введите ФАМИЛИЮ", "OK");
            return;
        }
        if (!DateOnly.TryParse(Birthday, out _))
        {
            await DisplayAlert("Поле ДАТА РОЖДЕНИЯ введено неверно", "Введите ДАТУ РОЖДЕНИЯ", "OK");
            return;
        }
        if (!PhoneNumberEntryBehavior.PhoneRegex.IsMatch(user.NumberPhone))
        {
            await DisplayAlert("Поле НОМЕР ТЕЛЕФОНА введено неверно", "Введите НОМЕР ТЕЛЕФОНА", "OK");
            return;
        }
        if (!EmailEntryBehavior.EmailRegex.IsMatch(user.Email))
        {
            await DisplayAlert("Поле ЭЛ. ПОЧТА введено неверно", "Введите ЭЛ. ПОЧТУ", "OK");
            return;
        }
        var usr = new UserDTO
        {
            Birthday = DateOnly.Parse(Birthday),
            Email = user.Email,
            Lastname = user.Lastname,
            Name = user.Name,
            Patronymic = user.Patronymic,
            NumberPhone = user.NumberPhone,
            Gender = SelectedGender,
            Token = user.Token
        };
        var json = JsonSerializer.Serialize(usr);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        try
        {
            // Send the request to the API
            var response = await _httpClient.PostAsync("https://localhost:7137/api/UserProfile/EditUser", content);

            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var httpuser = await JsonSerializer.DeserializeAsync<UserDTO>(await response.Content.ReadAsStreamAsync());
                AppManager.currentUser = httpuser;
                await AppShell.Current.GoToAsync("//ProfilePage");
            }
            else
            {
                await DisplayAlert("Error " + response.StatusCode, "Edit failed. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}