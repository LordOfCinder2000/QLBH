using QLBH.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.Model
{
    public class Inventory: BaseViewModel
    {
        private Object _Object;
        public Object Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private int _STT;
        public int STT { get => _STT; set { _STT = value; OnPropertyChanged(); } }

        private int _CountInput;
        public int CountInput { get => _CountInput; set { _CountInput = value; OnPropertyChanged(); } }

        private int _CountOutput;
        public int CountOutput { get => _CountOutput; set { _CountOutput = value; OnPropertyChanged(); } }

        private int _CountInventory;
        public int CountInventory { get => _CountInventory; set { _CountInventory = value; OnPropertyChanged(); } }

        private double _MoneyInventory;
        public double MoneyInventory { get => _MoneyInventory; set { _MoneyInventory = value; OnPropertyChanged(); } }

        private double _MoneyOutput;
        public double MoneyOutput { get => _MoneyOutput; set { _MoneyOutput = value; OnPropertyChanged(); } }

        private double _MoneyInput;
        public double MoneyInput { get => _MoneyInput; set { _MoneyInput = value; OnPropertyChanged(); } }

        private double _MoneyEarn;
        public double MoneyEarn { get => _MoneyEarn; set { _MoneyEarn = value; OnPropertyChanged(); } }

    }
}
