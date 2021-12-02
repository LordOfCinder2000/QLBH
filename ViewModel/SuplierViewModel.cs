using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBH.ViewModel
{
    public class SuplierViewModel:BaseViewModel
    {
        private ObservableCollection<Suplier> _List;
        public ObservableCollection<Suplier> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        
        private Suplier _SelectedItem;
        public Suplier SelectedItem
        {
            get => _SelectedItem; 
            set
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

        private bool _ValidateErrorPhone;
        public bool ValidateErrorPhone { get => _ValidateErrorPhone; set { _ValidateErrorPhone = value; OnPropertyChanged(); } }

        private bool _ValidateErrorDisplayName;
        public bool ValidateErrorDisplayName { get => _ValidateErrorDisplayName; set { _ValidateErrorDisplayName = value; OnPropertyChanged(); } }

        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public SuplierViewModel()
        {
            var ListSuplier = DataProvider.Ins.DB.Supliers;
            if (ListSuplier != null)
            {
                List = new ObservableCollection<Suplier>(ListSuplier);
            }

            
            AddCommand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(DisplayName)|| ValidateErrorPhone == true || DisplayName == "" || DisplayName == null || ValidateErrorDisplayName == true)
                {
                    return false;
                }
                return true;
            }, (p) => {
                var suplier = DataProvider.Ins.DB.Supliers.Add(new Suplier() { 
                    DisplayName = DisplayName,
                    Address = Address,
                    Phone = Phone,
                    Email = Email,
                    ContractDate = ContractDate
                });
                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(suplier);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });

            EditCommand = new RelayCommand<ListView>((p) => {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null || ValidateErrorPhone == true || ValidateErrorDisplayName == true)
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Supliers.Where(x => x.ID == SelectedItem.ID);
                if (displayList == null || displayList.Count() != 0)
                {
                    return true;
                }
                return false;
            }, (p) => {
                var suplier = DataProvider.Ins.DB.Supliers.Where(pp => pp.ID == SelectedItem.ID).SingleOrDefault();
                suplier.DisplayName = DisplayName;
                suplier.Address = Address;
                suplier.Phone = Phone;
                suplier.Email = Email;
                suplier.ContractDate = ContractDate;
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
                        List[i] = new Suplier() { 
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

                    var suplier = DataProvider.Ins.DB.Supliers.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataObject = DataProvider.Ins.DB.Objects.Where(x => x.IDSuplier == SelectedItem.ID).ToList();

                    var dataInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.Object.IDSuplier == SelectedItem.ID).ToList();

                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.Object.IDSuplier == SelectedItem.ID).ToList();


                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)//Xóa ID của object trong Out
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    var dataInOut = DataProvider.Ins.DB.OutputInfoes.Where(x => x.InputInfo.Object.IDSuplier == SelectedItem.ID).ToList();
                    if (dataInOut != null && dataInOut.Count() != 0)//Xóa ID của In trong Out
                    {
                        foreach (var item in dataInOut)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    if (dataInputInfo != null && dataInputInfo.Count() != 0)//Xóa ID của object trong In
                    {
                        foreach (var item in dataInputInfo)
                        {
                            DataProvider.Ins.DB.InputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    if (dataObject != null && dataObject.Count() != 0)//Xóa ID của suplier trong object
                    {
                        foreach (var item in dataObject)
                        {
                            DataProvider.Ins.DB.Objects.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    foreach (var item in List)
                    {
                        if (item.ID == suplier.ID)
                        {
                            List.Remove(item);
                            break;
                        }

                    }
                    DataProvider.Ins.DB.Supliers.Remove(suplier);//xóa suplier
                    DataProvider.Ins.DB.SaveChanges();
                    //List.Remove(suplier);
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedItem = null;
                }            
            });
        }
    }
}
