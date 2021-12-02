using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class PhoneRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                if (value.ToString().All(char.IsDigit))
                {
                    if(value.ToString().Length == 10)
                        return new ValidationResult(true, "OK");
                    return new ValidationResult(false, "SĐT phải có 10 chữ số !");
                }   
                else
                    return new ValidationResult(false, "SĐT không được chứa ký tự !");
            else
                return new ValidationResult(true, "OK");
        }
    }
}
