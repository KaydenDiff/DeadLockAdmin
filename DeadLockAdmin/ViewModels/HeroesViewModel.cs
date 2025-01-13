using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DeadLockAdmin.Models;

namespace DeadLockAdmin.ViewModels
{
    public class HeroesViewModel : INotifyPropertyChanged
    {
        private const string CharactersApiUrl = "http://deadlock/api/characters";

        private readonly HttpClient _httpClient = new HttpClient(); // Инициализация HTTP клиента

        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>(); // Коллекция персонажей для привязки
        public Character SelectedCharacter { get; set; } // Выбранный персонаж
        public HeroesViewModel()
        {
            _ = LoadCharactersAsync(); // Загружаем персонажей асинхронно при инициализации ViewModel
        }

        // Асинхронный метод для получения списка персонажей из API
        private async Task<List<Character>> FetchCharactersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(CharactersApiUrl, cancellationToken); // Получаем строку ответа от API
                var data = JsonSerializer.Deserialize<ApiResponse>(response); // Десериализуем ответ
                return data?.Персонажи ?? new List<Character>(); // Возвращаем список персонажей, если он есть, или пустой список
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching characters: {ex.Message}"); // Логирование ошибки при получении данных
                return new List<Character>(); // Возвращаем пустой список в случае ошибки
            }
        }

        // Асинхронный метод для загрузки и обработки персонажей
        private async Task LoadCharactersAsync()
        {
            try
            {
                var characters = await FetchCharactersAsync(); // Загружаем персонажей

                if (characters != null && characters.Any()) // Проверка на успешную загрузку персонажей
                {
                    Characters.Clear(); // Очищаем текущий список персонажей
                    foreach (var character in characters)
                    {
                        character.Image = $"http://deadlock/storage/{character.Image}"; // Формируем полный путь для изображения
                        Characters.Add(character); // Добавляем персонажа в коллекцию
                        Debug.WriteLine($"Loaded: {character.Image}"); // Логирование загруженного персонажа
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading characters: {ex.Message}"); // Логирование ошибки при загрузке персонажей
            }
        }

        // Метод для сброса выбранного персонажа
        public void ResetSelectedCharacter()
        {
            SelectedCharacter = null; // Сбрасываем выбор персонажа
        }

        // Событие изменения свойства для привязки данных
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для уведомления об изменении свойства
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызываем событие изменения свойства
        }
       
 

        public async Task LoadCharacters()
        {
            try
            {
                // Вызов метода для получения данных с API
                var characters = await GetCharactersFromApi();
                Characters.Clear(); // Очищаем коллекцию перед загрузкой новых данных

                foreach (var character in characters)
                {
                    Characters.Add(character); // Добавляем персонажей в коллекцию
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка загрузки персонажей: {ex.Message}");
            }
        }

        private async Task<List<Character>> GetCharactersFromApi()
        {
            string url = $"{Data.Host}/characters"; // URL для получения персонажей
            using HttpClient client = new();
            string response = await client.GetStringAsync(url);

            // Парсинг JSON в список персонажей
            return JsonSerializer.Deserialize<List<Character>>(response);
        }
    }
}
