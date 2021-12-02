using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                if (value.ToString().All(char.IsDigit))
                    return new ValidationResult(true, "OK");
                else
                    return new ValidationResult(false, "Số tiền không được chứa ký tự !");
            else
                return new ValidationResult(false, "Không được để trống !");
        }
    }
}
