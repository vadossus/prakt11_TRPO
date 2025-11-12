using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace PacientApp1
{
    public class Patient : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _lastName;
        private string _middleName;
        private string _birthday;
        private string _phoneNumber;
        private ObservableCollection<Appointment> _appointmentStories = new ObservableCollection<Appointment>();

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string MiddleName
        {
            get => _middleName;
            set { _middleName = value; OnPropertyChanged(); }
        }

        public string Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Appointment> AppointmentStories
        {
            get => _appointmentStories;
            set { _appointmentStories = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Appointment : INotifyPropertyChanged
    {
        private string _date;

        private int _doctorId;

        [JsonIgnore]
        private string _fio;

        private string _diagnosis;

        private string _recommendations;

        public string Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public int DoctorId
        {
            get => _doctorId;
            set { _doctorId = value; OnPropertyChanged(); }
        }

        public string DoctorName
        {
            get => _fio;
            set { _fio = value; OnPropertyChanged(); }
        }

        public string Diagnosis
        {
            get => _diagnosis;
            set { _diagnosis = value; OnPropertyChanged(); }
        }

        public string Recommendations
        {
            get => _recommendations;
            set { _recommendations = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}