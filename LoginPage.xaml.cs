using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PacientApp1
{
    public partial class LoginPage : Page
    {
        public LoginViewModel ViewModel { get; } = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(IDTextBox))
            {
                return;
            }

            if (string.IsNullOrEmpty(PassBox.Password))
            {
                ShowPasswordError("Введите пароль");
                return;
            }
            else
            {
                HidePasswordError();
            }

            if (string.IsNullOrEmpty(ViewModel.UserId) || !int.TryParse(ViewModel.UserId, out int userId))
            {
                return;
            }

            string fileName = $"D_{userId}.json";
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            string json = File.ReadAllText(fileName);
            var user = JsonSerializer.Deserialize<Doctor>(json);

            if (user.Password != PassBox.Password)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }

            App.CurrentDoctor = user;
            App.LoadPatients();
            NavigationService.Navigate(new MainUserPage());
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
        public string UserId { get; set; }
    }
}