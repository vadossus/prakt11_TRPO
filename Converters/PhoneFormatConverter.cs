using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PacientApp1.Converters
{
    public class PhoneFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            string phone = value.ToString();

            string cleanedPhone = phone.Trim();

            string digitsOnly = new string(cleanedPhone.Where(char.IsDigit).ToArray());


            if (digitsOnly.Length == 10)
            {
                // 900 000 00 00 -> +7 (900) 000 00 00
                return $"+7 ({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)} {digitsOnly.Substring(6, 2)} {digitsOnly.Substring(8, 2)}";
            }

            return phone;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}