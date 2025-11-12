using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PacientApp1
{
    public partial class CreatePatientPage : Page
    {
        private ObservableCollection<Patient> _patients;
        public Patient CurrentPatient { get; } = new Patient();

        public CreatePatientViewModel ViewModel { get; } = new CreatePatientViewModel();

        public CreatePatientPage(ObservableCollection<Patient> patients)
        {
            InitializeComponent();
            _patients = patients;
            DataContext = ViewModel;
            CurrentPatient.AppointmentStories = new ObservableCollection<Appointment>();

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

            var patient = new Patient
            {
                Name = ViewModel.Name,
                LastName = ViewModel.LastName,
                MiddleName = ViewModel.MiddleName,
                Birthday = ViewModel.Birthday?.ToString("dd.MM.yyyy") ?? "",
                PhoneNumber = ViewModel.PhoneNumber,
                AppointmentStories = new ObservableCollection<Appointment>()
            };

            Random random = new Random();
            int patientId;
            bool idExists;

            do
            {
                patientId = random.Next(1000000, 10000000);
                idExists = File.Exists($"P_{patientId}.json");
            } while (idExists);

            patient.Id = patientId;

            string fileName = $"P_{patient.Id}.json";
            string json = JsonSerializer.Serialize(patient, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json);
            _patients.Add(patient);

            MessageBox.Show($"Пациент создан! ID: {patient.Id}");
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

    public class CreatePatientViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhoneNumber { get; set; }
    }
}