using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PacientApp1.Converters
{
    public class DaysSinceLastAppointmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Appointment> appointments && appointments != null)
            {
                if (appointments.Count == 0)
                    return "Первый прием в клинике";

                var lastAppointment = appointments.OrderByDescending(a =>
                {
                    if (DateTime.TryParse(a.Date, out DateTime date))
                        return date;
                    return DateTime.MinValue;
                }).FirstOrDefault();

                if (lastAppointment != null && DateTime.TryParse(lastAppointment.Date, out DateTime lastDate))
                {
                    int days = (DateTime.Now - lastDate).Days;
                    return $"{days} дней с предыдущего приема";
                }
            }

            return "Первый прием в клинике";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}