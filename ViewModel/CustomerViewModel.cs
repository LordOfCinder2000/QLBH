using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLBH.ViewModel
{
    class CustomerViewModel:BaseViewModel
    {
        private ObservableCollection<Customer> _List;
        public ObservableCollection<Customer> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Customer _SelectedItem;
        public Customer SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Address = SelectedItem.Address;
                    Phone = SelectedItem.Phone;
                    Email = SelectedItem.Email;
                    ContractDate = SelectedItem.ContractDate;
                }
            }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        private string _NoNameCustomerText;
        public string NoNameCustomerText { get => _NoNameCustomerText; set { _NoNameCustomerText = value; OnPropertyChanged(); } }

        private bool _ValidateErrorPhone;
        public bool ValidateErrorPhone { get => _ValidateErrorPhone; set { _ValidateErrorPhone = value; OnPropertyChanged(); } }

        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand NoNameCustomerCommand { get; set; }
        public CustomerViewModel()
        {
            var ListCustomer = DataProvider.Ins.DB.Customers;
            if (ListCustomer != null)
            {
                List = new ObservableCollection<Customer>(ListCustomer);
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].Type == "Unknow")
                    {
                        List.Remove(List[i]);
                        i--;
                    }
                }

            }

            AddCommand = new RelayCommand<object>((p) => {
                if (string.IsNullOrWhiteSpace(DisplayName) || ValidateErrorPhone == true)
                    return false;
                return true;
            }, (p) => {
                var customer = DataProvider.Ins.DB.Customers.Add(new Customer()
                {
                    DisplayName = DisplayName,
                    Address = Address,
                    Phone = Phone,
                    Email = Email,
                    ContractDate = ContractDate
                });
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(customer); ;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            });

            EditCommand = new RelayCommand<ListView>((p) => {
                if (ValidateErrorPhone == true || string.IsNullOrEmpty(DisplayName) || SelectedItem == null)
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Customers.Where(x => x.ID == SelectedItem.ID);
                if (displayList == null || displayList.Count() != 0)
                {
                    return true;
                }
                return false;
            }, (p) => {
                var customer = DataProvider.Ins.DB.Customers.Where(pp => pp.ID == SelectedItem.ID).SingleOrDefault();
                customer.DisplayName = DisplayName;
                customer.Address = Address;
                customer.Phone = Phone;
                customer.Email = Email;
                customer.ContractDate = ContractDate;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.Address = Address;
                SelectedItem.Phone = Phone;
                SelectedItem.Email = Email;
                SelectedItem.ContractDate = ContractDate;
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].ID == SelectedItem.ID)
                    {
                        List[i] = new Customer()
                        {
                            ID = SelectedItem.ID,
                            DisplayName = DisplayName,
                            Address = Address,
                            Phone = Phone,
                            Email = Email,
                            ContractDate = ContractDate
                        };
                        SelectedItem = List[i];
                        p.SelectedItem = SelectedItem;
                        break;
                    }
                }
                MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            DeleteCommand = new RelayCommand<object>((p) => {
                if (SelectedItem == null)
                {
                    return false;
                }
                return true;
            }, (p) => {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var customer = DataProvider.Ins.DB.Customers.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataOuput = DataProvider.Ins.DB.Outputs.Where(x => x.IDCustomer == SelectedItem.ID).ToList();
                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.Output.IDCustomer == SelectedItem.ID).ToList();


                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)//Xóa ID của Output trong OutputInfo
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }


                    if (dataOuput != null && dataOuput.Count() != 0)
                    {
                        foreach (var item in dataOuput)
                        {
                            DataProvider.Ins.DB.Outputs.Remove(item);
                            DataProvider.Ins.DB.SaveChanges();

                        }
                    }

                    foreach (var item in List)
                    {
                        if (item.ID == customer.ID)
                        {
                            List.Remove(item);
                            break;
                        }

                    }

                    DataProvider.Ins.DB.Customers.Remove(customer);
                    DataProvider.Ins.DB.SaveChanges();
                    //List.Remove(customer);
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedItem = null;
                }
            });

            NoNameCustomerText = "Hiển thị khách lẻ";
            NoNameCustomerCommand = new RelayCommand<Button>((p) => { return true; }
            , (p) =>
            {
                List = new ObservableCollection<Customer>(ListCustomer);
                if(NoNameCustomerText == "Hiển thị khách lẻ")
                {
                    for (int i = 0; i < List.Count(); i++)
                    {
                        if (List[i].Type != "Unknow")
                        {
                            List.Remove(List[i]);
                            i--;
                        }
                    }
                    p.Width = 180;
                    NoNameCustomerText = "Hiển thị khách hợp tác";
                }
                else
                {
                    for (int i = 0; i < List.Count(); i++)
                    {
                        if (List[i].Type == "Unknow")
                        {
                            List.Remove(List[i]);
                            i--;
                        }
                    }
                    p.Width = 150;
                    NoNameCustomerText = "Hiển thị khách lẻ";
                }

            });
        }
    }
}
