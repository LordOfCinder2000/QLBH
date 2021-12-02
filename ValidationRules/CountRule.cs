using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class CountRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                if (value.ToString().All(char.IsDigit))
                {
                    if(Double.Parse(value.ToString())==0)
                    {
                        return new ValidationResult(false, "Số lượng phải lớn hơn 0!");
                    }    
                    return new ValidationResult(true, "OK");
                }    
                else
                    return new ValidationResult(false, "Số lượng không được chứa ký tự !");
            else
                return new ValidationResult(false, "Không được để trống !");
        }
    }
}
