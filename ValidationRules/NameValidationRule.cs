using System.Globalization;
using System.Windows.Controls;

namespace PacientApp1.ValidationRules
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Поле обязательно для заполнения");
            }

            if (input.Length < 2)
            {
                return new ValidationResult(false, "Должно быть не менее 2 символов");
            }

            if (!input.All(c => char.IsLetter(c) || c == '-' || c == ' '))
            {
                return new ValidationResult(false, "Допустимы только буквы, дефисы и пробелы");
            }

            return ValidationResult.ValidResult;
        }
    }
}