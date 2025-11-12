using System.Globalization;
using System.Windows.Controls;

namespace PacientApp1.ValidationRules
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Поле обязательно для заполнения");
            }

            return ValidationResult.ValidResult;
        }
    }
}