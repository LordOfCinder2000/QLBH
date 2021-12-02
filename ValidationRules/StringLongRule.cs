using System.Globalization;
using System.Linq;
using System.Windows.Controls;


namespace QLBH.ValidationRules
{
    class StringLongRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                if (value.ToString().Length >= 5)
                    return new ValidationResult(true, "OK");
                else
                    return new ValidationResult(false, "Mật khẩu phải từ 5 ký tự trở lên.");
            else
                return new ValidationResult(false, "Không được để trống.");
        }
    }
}
