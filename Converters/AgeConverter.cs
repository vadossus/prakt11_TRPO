using System;
using System.Globalization;
using System.Windows.Data;

namespace PacientApp1.Converters
{
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return "Возраст не указан";

            if (DateTime.TryParse(value.ToString(), out DateTime birthDate))
            {
                int age = CalculateAge(birthDate);
                string adultStatus = age >= 18 ? "совершеннолетний" : "несовершеннолетний";
                return $"{age} лет ({adultStatus})";
            }

            return "Неверная дата";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}