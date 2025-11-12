using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace PacientApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Doctor CurrentDoctor { get; set; }
        public static ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();


        private void Application_Startup(object sender, StartupEventArgs e) {}

        public static void LoadPatients()
        {
            try
            {
                Patients.Clear();
                var patientFiles = Directory.GetFiles(".", "P_*.json");

                foreach (var file in patientFiles)
                {
                    string json = File.ReadAllText(file);
                    var patient = JsonSerializer.Deserialize<Patient>(json, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (patient.AppointmentStories == null)
                        patient.AppointmentStories = new ObservableCollection<Appointment>();

                    Patients.Add(patient);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пациентов: {ex.Message}");
            }
        }

        public static void SavePatient(Patient patient)
        {
            try
            {
                string fileName = $"P_{patient.Id}.json";
                string json = JsonSerializer.Serialize(patient, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                File.WriteAllText(fileName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения пациента: {ex.Message}");
            }
        }

        public static void DeletePatient(Patient patient)
        {
            try
            {
                string fileName = $"P_{patient.Id}.json";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Patients.Remove(patient);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления пациента: {ex.Message}");
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemeHelper.ApplySavedTheme();
        }
    }

}
