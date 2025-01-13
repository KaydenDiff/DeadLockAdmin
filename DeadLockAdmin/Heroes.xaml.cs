using DeadLockAdmin.Models;
using DeadLockAdmin.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Text;
using System.Text.Json;

namespace DeadLockAdmin
{
    public partial class Heroes : ContentPage
    {
        // Свойство, которое предоставляет доступ к ViewModel
        private HeroesViewModel ViewModel => BindingContext as HeroesViewModel;

        // Конструктор страницы, в котором инициализируется ViewModel
        public Heroes()
        {
            InitializeComponent(); // Инициализируем компоненты страницы
            BindingContext = new HeroesViewModel(); // Привязываем ViewModel к странице
        }

        // Обработчик выбора персонажа из коллекции
        private async void OnCharacterSelected(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, выбран ли персонаж
            if (e.CurrentSelection.FirstOrDefault() is Character selectedCharacter)
            {
                // Устанавливаем выбранного персонажа в глобальный объект Data (для дальнейшего использования)
                Data.CurrentCharacter = selectedCharacter;

                // Пересоздаем или сбрасываем источник данных для CollectionView
                // Это делаем для того, чтобы данные обновились, если коллекция изменится
                charactersCollectionView.ItemsSource = null;
                charactersCollectionView.ItemsSource = ViewModel.Characters;

                // Переходим на страницу с билдом выбранного персонажа
                // Передаем параметры через URL для динамического отображения нужных данных
                await Shell.Current.GoToAsync($"{nameof(BuildsPage)}?characterId={selectedCharacter.Id}&name={selectedCharacter.Name}&timestamp={DateTime.Now.Ticks}");
            }
        }

        // Этот метод вызывается, когда страница появляется на экране
        // Здесь мы сбрасываем и обновляем источник данных для CollectionView, чтобы гарантировать, что отображаются актуальные данные
        protected override void OnAppearing()
        {
            base.OnAppearing(); // Вызываем метод базового класса для нормальной работы жизненного цикла страницы

            // Проверяем, если CollectionView существует, то сбрасываем и снова привязываем источник данных
            if (charactersCollectionView != null)
            {
                charactersCollectionView.ItemsSource = null; // Очищаем источник данных
                charactersCollectionView.ItemsSource = ViewModel.Characters; // Привязываем обновленные данные из ViewModel
            }
        }

        // Этот метод вызывается, когда страница исчезает с экрана
        // Мы сбрасываем выделение и очищаем источник данных, чтобы избежать утечек памяти или некорректного состояния
        protected override void OnDisappearing()
        {
            base.OnDisappearing(); // Вызываем метод базового класса

            // Проверяем, если CollectionView существует, сбрасываем выделение и очищаем источник данных
            if (charactersCollectionView != null)
            {
                charactersCollectionView.SelectedItem = null; // Сбрасываем выделение текущего элемента
                charactersCollectionView.ItemsSource = null; // Очищаем источник данных
            }
        }
        private async void OnCreateCharacterClicked(object sender, EventArgs e)
        {
            try
            {
                // Перенаправление на страницу CreateCharacterPage
                await Navigation.PushAsync(new CreateCharacterPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "OK");
            }
        }
    }
}
