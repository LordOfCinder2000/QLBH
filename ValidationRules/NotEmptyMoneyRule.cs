using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class NotEmptyMoneyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //return string.IsNullOrWhiteSpace((value ?? "").ToString())
            //    ? new ValidationResult(false, "Không được để trống")
            //    : ValidationResult.ValidResult;
            if (value == null)
                return new ValidationResult(false, "Chọn sản phẩm");
            string str = value.ToString();
            if (str == "")
                return new ValidationResult(false, "Chọn sản phẩm");
            else
                return new ValidationResult(true, "OK");
        }
    }
}
