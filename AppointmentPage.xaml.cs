using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PacientApp1
{
    public partial class AppointmentPage : Page
    {
        private ObservableCollection<Patient> _patients;
        public Patient CurrentPatient { get; }
        public Appointment NewAppointment { get; } = new Appointment();

        public AppointmentPage(ObservableCollection<Patient> patients, Patient patient)
        {
            InitializeComponent();
            _patients = patients;
            CurrentPatient = patient;
            DataContext = this;

            NewAppointment.Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            NewAppointment.DoctorId = App.CurrentDoctor.Id;
            NewAppointment.DoctorName = $"{App.CurrentDoctor.LastName} {App.CurrentDoctor.Name} {App.CurrentDoctor.MiddleName}";
        }

        private void SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(DiagnosisTextBox))
            {
                MessageBox.Show("Исправьте ошибки в поле диагноза");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewAppointment.Diagnosis))
            {
                MessageBox.Show("Введите диагноз");
                return;
            }

            
            CurrentPatient.AppointmentStories.Add(new Appointment
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                DoctorId = App.CurrentDoctor.Id,
                DoctorName = $"{App.CurrentDoctor.LastName} {App.CurrentDoctor.Name} {App.CurrentDoctor.MiddleName}",
                Diagnosis = NewAppointment.Diagnosis,
                Recommendations = NewAppointment.Recommendations
            });

            SavePatientData();

            NewAppointment.Diagnosis = "";
            NewAppointment.Recommendations = "";

            MessageBox.Show("Прием сохранен");
        }

        private void SavePatientData()
        {
            string fileName = $"P_{CurrentPatient.Id}.json";
            string json = JsonSerializer.Serialize(CurrentPatient, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(fileName, json);
        }

        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPatientPage(_patients, CurrentPatient));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}