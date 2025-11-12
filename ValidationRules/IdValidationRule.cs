using System.Globalization;
using System.Windows.Controls;

namespace PacientApp1.ValidationRules
{
    public class IdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Идентификатор обязателен");
            }

            if (!int.TryParse(input, out int id))
            {
                return new ValidationResult(false, "Идентификатор должен быть числом");
            }

            if (id < 10000 || id > 99999)
            {
                return new ValidationResult(false, "Идентификатор должен быть от 10000 до 99999");
            }

            return ValidationResult.ValidResult;
        }
    }
}