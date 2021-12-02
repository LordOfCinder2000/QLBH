using System.Globalization;
using System.Windows.Controls;


namespace QLBH.ValidationRules
{
    public class NotEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //return string.IsNullOrWhiteSpace((value ?? "").ToString())
            //    ? new ValidationResult(false, "Không được để trống")
            //    : ValidationResult.ValidResult;
            if(value==null)
                return new ValidationResult(false, "Không được để trống");
            string str = value.ToString();
            if (str == "")
                return new ValidationResult(false, "Không được để trống");
            else
                return new ValidationResult(true, "OK");
        }
    }
}
