using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class StringHasWhiteSpaceRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value.ToString();
            if (str.Split(' ').Length == 1)
                return new ValidationResult(true, "OK");
            else
                return new ValidationResult(false, "Không được để khoảng trắng.");
        }
    }
}
