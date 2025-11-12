using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PacientApp1
{
    public partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel { get; } = new RegisterViewModel();

        public RegisterPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            bool hasErrors = false;

            if (Validation.GetHasError(NameTextBox) ||
                Validation.GetHasError(LastNameTextBox) ||
                Validation.GetHasError(MiddleNameTextBox))
            {
                hasErrors = true;
            }

            if (SpecComboBox.SelectedItem == null)
            {
                ShowSpecError("Выберите специализацию");
                hasErrors = true;
            }
            else
            {
                HideSpecError();
            }

            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                ShowPasswordError("Пароль обязателен");
                hasErrors = true;
            }
            else if (PasswordBox.Password.Length < 6)
            {
                ShowPasswordError("Пароль должен быть не менее 6 символов");
                hasErrors = true;
            }
            else
            {
                HidePasswordError();
            }

            if (string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                ShowConfirmPasswordError("Подтверждение пароля обязательно");
                hasErrors = true;
            }
            else if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                ShowConfirmPasswordError("Пароли не совпадают");
                hasErrors = true;
            }
            else
            {
                HideConfirmPasswordError();
            }

            if (hasErrors)
            {
                return;
            }

            string specialisation = (SpecComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            Random random = new Random();
            int newId;
            bool idExists;

            do
            {
                newId = random.Next(10000, 100000);
                idExists = File.Exists($"D_{newId}.json");
            } while (idExists);

            var doctor = new Doctor
            {
                Id = newId,
                Name = ViewModel.Name.Trim(),
                LastName = ViewModel.LastName.Trim(),
                MiddleName = ViewModel.MiddleName.Trim(),
                Specialisation = specialisation,
                Password = ViewModel.Password
            };

            string fileName = $"D_{doctor.Id}.json";
            string json = JsonSerializer.Serialize(doctor, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(fileName, json);

            App.CurrentDoctor = doctor;
            MessageBox.Show($"Регистрация успешна! Ваш ID: {doctor.Id}");

            App.LoadPatients();

            NavigationService.Navigate(new MainUserPage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ShowSpecError(string errorMessage)
        {
            SpecErrorItems.ItemsSource = new[] { errorMessage };
            SpecErrorItems.Visibility = Visibility.Visible;
        }

        private void HideSpecError()
        {
            SpecErrorItems.Visibility = Visibility.Collapsed;
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

        private void ShowConfirmPasswordError(string errorMessage)
        {
            ConfirmPasswordErrorItems.ItemsSource = new[] { errorMessage };
            ConfirmPasswordErrorItems.Visibility = Visibility.Visible;
        }

        private void HideConfirmPasswordError()
        {
            ConfirmPasswordErrorItems.Visibility = Visibility.Collapsed;
        }
    }

    public class RegisterViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public object SelectedItem { get; set; }
    }
}