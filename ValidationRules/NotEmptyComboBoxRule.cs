using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class NotEmptyComboBoxRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(true, "OK");
            else
                return new ValidationResult(false, "Không được để trống !");
        }
    }
}