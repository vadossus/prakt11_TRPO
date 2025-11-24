using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PacientApp1
{
    public partial class LoginPage : Page
    {
        public LoginViewModel ViewModel { get; } = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;

            PassBox.PasswordChanged += PassBox_PasswordChanged;
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PassBox.Password;
            HidePasswordError();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(IDTextBox) || string.IsNullOrEmpty(ViewModel.UserId))
            {
                return;
            }

            if (string.IsNullOrEmpty(ViewModel.Password))
            {
                ShowPasswordError("Введите пароль");
                return;
            }
            else
            {
                HidePasswordError();
            }

            if (!int.TryParse(ViewModel.UserId, out int userId))
            {
                return;
            }

            string fileName = $"D_{userId}.json";
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            try
            {
                string json = File.ReadAllText(fileName);
                var user = JsonSerializer.Deserialize<Doctor>(json);

                if (user.Password != ViewModel.Password)
                {
                    MessageBox.Show("Неверный пароль");
                    return;
                }

                App.CurrentDoctor = user;
                App.LoadPatients();
                NavigationService.Navigate(new MainUserPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void ShowPasswordError(string errorMessage)
        {
            PasswordErrorItems.ItemsSource = new[] { errorMessage };
            PasswordErrorItems.Visibility = Visibility.Visible;
        }

        private void HidePasswordError()
        {
            PasswordErrorItems.Visibility = Visibility.Collapsed;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }

    public class LoginViewModel
    {
        public string? UserId { get; set; }
        public string? Password { get; set; }
    }
}