using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PacientApp1
{
    public partial class EditPatientPage : Page
    {
        private ObservableCollection<Patient> _patients;
        private Patient _originalPatient;
        public Patient CurrentPatient { get; }

        public EditPatientViewModel ViewModel { get; } = new EditPatientViewModel();

        public EditPatientPage(ObservableCollection<Patient> patients, Patient patient)
        {
            InitializeComponent();
            _patients = patients;
            _originalPatient = patient;
            CurrentPatient = patient;

            ViewModel.PatientId = patient.Id;
            ViewModel.Name = patient.Name;
            ViewModel.LastName = patient.LastName;
            ViewModel.MiddleName = patient.MiddleName;
            ViewModel.PhoneNumber = patient.PhoneNumber;

            if (!string.IsNullOrEmpty(patient.Birthday) &&
                DateTime.TryParse(patient.Birthday, out DateTime birthday))
            {
                ViewModel.Birthday = birthday;
            }

            DataContext = ViewModel;

            BirthdayDatePicker.SelectedDateChanged += BirthdayDatePicker_SelectedDateChanged;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateDatePicker())
            {
                return;
            }

            if (Validation.GetHasError(NameTextBox) ||
                Validation.GetHasError(LastNameTextBox) ||
                Validation.GetHasError(MiddleNameTextBox) ||
                Validation.GetHasError(PhoneTextBox))
            {
                MessageBox.Show("Исправьте ошибки в полях ввода");
                return;
            }

            _originalPatient.Name = ViewModel.Name;
            _originalPatient.LastName = ViewModel.LastName;
            _originalPatient.MiddleName = ViewModel.MiddleName;
            _originalPatient.Birthday = ViewModel.Birthday?.ToString("dd.MM.yyyy") ?? "";
            _originalPatient.PhoneNumber = ViewModel.PhoneNumber;

            string fileName = $"P_{_originalPatient.Id}.json";
            string json = JsonSerializer.Serialize(_originalPatient, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json);

            MessageBox.Show("Информация о пациенте обновлена");
            NavigationService.GoBack();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BirthdayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayDatePicker.SelectedDate.HasValue)
            {
                CurrentPatient.Birthday = BirthdayDatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");
            }

            ValidateDatePicker();
        }

        private bool ValidateDatePicker()
        {
            DateErrorItems.ItemsSource = null;
            DateErrorItems.Visibility = Visibility.Collapsed;

            if (!BirthdayDatePicker.SelectedDate.HasValue)
            {
                ShowDateError("Дата обязательна для заполнения");
                return false;
            }

            DateTime date = BirthdayDatePicker.SelectedDate.Value;

            if (date > DateTime.Now)
            {
                ShowDateError("Дата рождения не может быть в будущем");
                return false;
            }

            if (date < DateTime.Now.AddYears(-150))
            {
                ShowDateError("Дата рождения неверна");
                return false;
            }

            return true;
        }

        private void ShowDateError(string errorMessage)
        {
            DateErrorItems.ItemsSource = new[] { errorMessage };
            DateErrorItems.Visibility = Visibility.Visible;
        }
    }

    public class EditPatientViewModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhoneNumber { get; set; }
    }
}