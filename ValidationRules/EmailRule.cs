using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (value == null)
                return new ValidationResult(true, "OK");
            string str = value.ToString();
            if (str == "")
                return new ValidationResult(true, "OK");
            else
            {
                if(regex.IsMatch(str))
                {
                    return new ValidationResult(true, "OK");
                }
                else
                {
                    return new ValidationResult(false, "Không đúng định dạng email");
                }    
            }    
                
        }
    }
}
