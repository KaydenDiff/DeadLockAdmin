
using Microsoft.Maui.Storage;
using System.IO;
using System.Text.Json;
using DeadLockAdmin.Models;
using DeadLockAdmin.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Text;
namespace DeadLockAdmin;

public partial class CreateCharacterPage : ContentPage
{
    private FileResult selectedImage;
    private HeroesViewModel ViewModel => BindingContext as HeroesViewModel;
    public CreateCharacterPage()
    {
        InitializeComponent();
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            // Выбор изображения
            selectedImage = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение"
            });

            if (selectedImage != null)
            {
                // Отображаем выбранное изображение
                var imageStream = await selectedImage.OpenReadAsync();
                SelectedImage.Source = ImageSource.FromStream(() => imageStream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Произошла ошибка при выборе изображения: {ex.Message}", "OK");
        }
    }
    private async void OnCreateCharacterClicked(object sender, EventArgs e)
    {
        try
        {
            // Извлекаем имя персонажа
            string characterName = CharacterNameEntry.Text;

            // Проверяем, что имя персонажа введено
            if (string.IsNullOrEmpty(characterName) || selectedImage == null)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите имя персонажа и выберите изображение.", "OK");
                return;
            }

            // Чтение изображения в base64
            byte[] imageBytes;
            using (var stream = await selectedImage.OpenReadAsync())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            string base64Image = Convert.ToBase64String(imageBytes);

            // Создание объекта для отправки на сервер
            var newCharacter = new
            {
                name = characterName,
                image = base64Image
            };

            // Сериализация данных в JSON
            string json = JsonSerializer.Serialize(newCharacter);

            // Отправка данных на сервер
            using (HttpClient client = new HttpClient())
            {
                var url = $"{Data.Host}/characters/create"; // Адрес вашего API

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Отправка POST-запроса
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Проверка успешности запроса
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Успешно", "Персонаж создан!", "OK");
                    // Обновление списка персонажей
                    ViewModel.LoadCharacters();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Ошибка", $"Не удалось создать персонажа. Код ошибки: {response.StatusCode}. {error}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "OK");
        }
    }
}
