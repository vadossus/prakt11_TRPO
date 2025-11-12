using System.Globalization;
using System.Windows.Controls;

namespace PacientApp1.ValidationRules
{
    public class ComboBoxRequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Выберите специализацию");
            }

            return ValidationResult.ValidResult;
        }
    }
}