using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PacientApp1
{
    /// <summary>
    /// Логика взаимодействия для MainUserPage.xaml
    /// </summary>
    public partial class MainUserPage : Page
    {
        public ObservableCollection<Patient> Patients => App.Patients;
        public Doctor CurrentDoctor => App.CurrentDoctor;
        public Patient SelectedPatient { get; set; }


        public MainUserPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CreatePatient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreatePatientPage(Patients));
        }

        private void Appointment_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show("Выберите пациента из списка");
                return;
            }
            NavigationService.Navigate(new AppointmentPage(Patients, SelectedPatient));
        }

        private void EditInfo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show("Выберите пациента из списка");
                return;
            }
            NavigationService.Navigate(new EditPatientPage(Patients, SelectedPatient));
        }

        private void PatientsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedPatient != null)
            {
                NavigationService.Navigate(new AppointmentPage(Patients, SelectedPatient));
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeletePatientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var selectedPatient = PatientsListView.SelectedItem as Patient;
                Patients.Remove(selectedPatient);
                App.DeletePatient(selectedPatient);
            }
        }
    }
}
