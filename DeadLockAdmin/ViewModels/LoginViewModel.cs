using System.Text.Json;
using System.Windows.Input;
using System.Text; // Подключаем пространство имён для Encoding
using System.Net.Http;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Text.Json.Serialization;
namespace DeadLockAdmin.ViewModels
{
    public class LoginViewModel : ItemsViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }


        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                if (_isErrorVisible != value)
                {
                    _isErrorVisible = value;
                    OnPropertyChanged(nameof(IsErrorVisible));
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            IsErrorVisible = false;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Пожалуйста, введите логин и пароль.";
                IsErrorVisible = true;
                return;
            }

            // Пример запроса к API
            var isSuccess = await AuthenticateUserAsync(Username, Password);
            if (isSuccess)
            {
                // Перенаправление на страницу HeroesPage
                await Shell.Current.GoToAsync("//HeroesPage");
                IsErrorVisible = true;
                ErrorMessage = "Успешно вошел";
            }
            else
            {
                IsErrorVisible = true;
            }
        }


        private async Task<bool> AuthenticateUserAsync(string login, string password)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = "http://deadlock/api/login";

                // Формируем тело запроса
                var content = new StringContent(
                    JsonSerializer.Serialize(new { login, password }),
                    Encoding.UTF8,
                    "application/json"
                );

                // Отправляем POST-запрос
                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Логирование для диагностики
                Debug.WriteLine($"Response StatusCode: {response.StatusCode}");
                Debug.WriteLine($"Response ReasonPhrase: {response.ReasonPhrase}");
                Debug.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    // Десериализация ответа
                    var result = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                    // Логирование результата десериализации
                    Debug.WriteLine($"Result: {JsonSerializer.Serialize(result)}");

                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        // Сохраняем токен
                        await SecureStorage.SetAsync("auth_token", result.Token);
                        return true;
                    }
                    else
                    {
                        // Если токен не найден
                        ErrorMessage = "Ошибка: Токен отсутствует в ответе.";
                        Debug.WriteLine("Ошибка: Токен отсутствует в ответе.");
                    }
                }
                else
                {
                    // Если статус ответа не успешный
                    ErrorMessage = "Ошибка: Статус-код ответа не успешный.";
                    Debug.WriteLine("Ошибка: Статус-код ответа не успешный.");
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка подключения: {ex.Message}";
                Debug.WriteLine($"Exception: {ex}");
                return false;
            }
        }

     

        // Класс для десериализации ответа
        public class LoginResponse
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }
        }



    }
}