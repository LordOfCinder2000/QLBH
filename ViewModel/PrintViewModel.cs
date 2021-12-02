using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace QLBH.ViewModel
{
    class PrintViewModel:BaseViewModel
    {
        public ICommand MouseMoveWindowCommand { get; set; }

        private IDocumentPaginatorSource _fixedDocumentSequence;

        public IDocumentPaginatorSource FixedDocumentSequence
        {
            get { return _fixedDocumentSequence; }
            set
            {
                if (_fixedDocumentSequence == value) return;

                _fixedDocumentSequence = value;
                OnPropertyChanged("FixedDocumentSequence");
            }
        }

        private Collection<HoaDon> _List;
        public Collection<HoaDon> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public PrintViewModel()
        {
          
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged();  } }

        private Customer _Customer;
        public Customer Customer { get => _Customer; set { _Customer = value; OnPropertyChanged(); } }

        private string _DateOutput;
        public string DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private string _IDOutput;
        public string IDOutput { get => _IDOutput; set { _IDOutput = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private String _Total;
        public String Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }

        private String _TotalConvert;
        public String TotalConvert { get => _TotalConvert; set { _TotalConvert = value; OnPropertyChanged(); } }

        private String _Test;
        public String Test { get => _Test; set { _Test = value; OnPropertyChanged(); } }

        public string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                   
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            result = result + (suffix ? " đồng chẵn" : "");
            result = char.ToUpper(result[0]) + result.Substring(1,result.Length-1);
            return result;
        }

    }

    class HoaDon
    {
        public int STT { get; set; }
        public string DisplayName { get; set; }
        public string Unit { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double SumPrice { get; set; }

        
    }


}
