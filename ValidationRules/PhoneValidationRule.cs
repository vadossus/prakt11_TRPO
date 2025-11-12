using System.Globalization;
using System.Windows.Controls;

namespace PacientApp1.ValidationRules
{
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Номер телефона обязателен");
            }

            string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length != 10)
            {
                return new ValidationResult(false, "Номер телефона должен содержать 10 цифр (без +7)");
            }

            // 900 000 00 00 - 10 символов



            return ValidationResult.ValidResult;
        }
    }
}