using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLBH.ValidationRules
{
    class ContainUnitRule : ValidationRule
    {
        private ObservableCollection<Unit> ListUnit;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var ListUnits = DataProvider.Ins.DB.Units;
            if (ListUnits != null)
            {
                ListUnit = new ObservableCollection<Unit>(ListUnits);
            }
            bool flag = false;

            foreach (Unit item in ListUnit)
            {
                if (value.ToString() == item.DisplayName)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                return new ValidationResult(false, "Đã có đơn vị tính này!");
            }
            return new ValidationResult(true, "OK");
        }
    }
}