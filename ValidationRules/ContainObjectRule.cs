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
    class ContainObjectRule : ValidationRule
    {
        private ObservableCollection<Model.Object> ListObject;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var ListObjects = DataProvider.Ins.DB.Objects;
            if (ListObjects != null)
            {
                ListObject = new ObservableCollection<Model.Object>(ListObjects);
            }
            bool flag = false;
            
            foreach (Model.Object item in ListObject)
            {
                if (value.ToString() == item.DisplayName)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                return new ValidationResult(false, "Không có mặt hàng này!");
            }
            return new ValidationResult(true, "OK");
        }
    }
}