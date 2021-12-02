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
    class OutputInfoViewModel:BaseViewModel
    {
        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        private ObservableCollection<OutputInfo> _List;
        public ObservableCollection<OutputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _ListObject;
        public ObservableCollection<Model.Object> ListObject { get => _ListObject; set { _ListObject = value; OnPropertyChanged(); } }

        private ObservableCollection<Customer> _ListCustomer;
        public ObservableCollection<Customer> ListCustomer { get => _ListCustomer; set { _ListCustomer = value; OnPropertyChanged(); } }

        private ThongKe _ThongKe;
        public ThongKe ThongKe { get => _ThongKe; set { _ThongKe = value; OnPropertyChanged(); } }

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer { get => _SelectedCustomer; set { _SelectedCustomer = value; OnPropertyChanged(); } }

        private Model.Object _SelectedObject;
        public Model.Object SelectedObject { 
            get => _SelectedObject;
            set { 
                _SelectedObject = value; 
                OnPropertyChanged(); 
                if(SelectedObject!=null)
                {
                    var priceMax = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == SelectedObject.ID)).Max(x => x.OutputPrice);
                    //foreach(var item in OuputPrice1)
                    //{
                    //    if(item.OutputPrice>max)
                    //    {
                    //        max = (double)item.OutputPrice;
                    //    }    
                    //}
                    var OuputPrice = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == SelectedObject.ID) && (x.OutputPrice == priceMax)).FirstOrDefault();
                    if (OuputPrice != null)
                    {
                        PriceObject = OuputPrice;
                    }
                    else
                    {
                        PriceObject = null;
                    }
                }
            }
        }

        private OutputInfo _SelectedItem;
        public OutputInfo SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedObject = SelectedItem.Object;
                    SelectedCustomer = SelectedItem.Output.Customer;
                    DateOutput = SelectedItem.Output.DateOutput;
                    Count = SelectedItem.Count;
                    Status = SelectedItem.Status;
                    OutputPrice = SelectedItem.InputInfo.OutputPrice;
                    SelectedCustomerText = SelectedItem.Output.Customer.DisplayName;
                    SelectedObjectText = SelectedItem.Object.DisplayName;
                }
            }
        }

        private string _SelectedObjectText;
        public string SelectedObjectText { get => _SelectedObjectText; set { _SelectedObjectText = value; OnPropertyChanged(); } }

        private string _SelectedCustomerText;
        public string SelectedCustomerText { get => _SelectedCustomerText; set { _SelectedCustomerText = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private InputInfo _PriceObject;
        public InputInfo PriceObject { get => _PriceObject; set { _PriceObject = value; OnPropertyChanged(); } }

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private string _Total;
        public string Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }

        private DateTime? _DateOutput;
        public DateTime? DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); }}

        private string _ComboBoxText;
        public string ComboBoxText { get => _ComboBoxText; set { _ComboBoxText = value; OnPropertyChanged(); } }

        private bool _ValidateErrorCount;
        public bool ValidateErrorCount { get => _ValidateErrorCount; set { _ValidateErrorCount = value; OnPropertyChanged(); } }

        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        
        
        public OutputInfoViewModel()
        {
            //Input = new ObservableCollection<Model.Input>(DataProvider.Ins.DB.Inputs);

            var ListOutputInfo = DataProvider.Ins.DB.OutputInfoes;
            if (ListOutputInfo != null)
            {
                List = new ObservableCollection<OutputInfo>(ListOutputInfo);
                
            }

            var ListObjects = DataProvider.Ins.DB.Objects;
            if (ListObjects != null)
            {
                ListObject = new ObservableCollection<Model.Object>(ListObjects);
            }

            var ListCustomers = DataProvider.Ins.DB.Customers;
            if (ListCustomers != null)
            {
                ListCustomer = new ObservableCollection<Customer>(ListCustomers);
            }
            
            AddCommand = new RelayCommand<object>((p) =>
            {

                if (SelectedObject == null||Count == null || ValidateErrorCount == true || Count == 0)
                    return false;

                return true;

            }, (p) =>
            {

                if (CountObject() < Count)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }
                else
                {
                    var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                    var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == dataObjectByID.ID).First();
                    Customer customer;
                    if (SelectedCustomer == null)
                    {
                        string customerName = "";
                        if (string.IsNullOrWhiteSpace(ComboBoxText))
                            customerName = "NoName";
                        else
                            customerName = ComboBoxText;
                        customer = new Customer()
                        {
                            DisplayName = customerName,
                            Type = "Unknow"
                        };
                        DataProvider.Ins.DB.Customers.Add(customer);
                    }
                    else
                    {
                        customer = SelectedCustomer;
                    }
                    
                    var user = DataProvider.Ins.DB.Users.Where(x => x.ID == Globals.IDUser).First();
                    var output = new Output()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Customer = customer,
                        User = user,
                        DateOutput = DateOutput,
                        Total = Count * PriceObject.OutputPrice
                    };
                    var outputInfo = new OutputInfo()
                    {
                        ID = Guid.NewGuid().ToString(),
                        IDOutput = output.ID,
                        Output = output,
                        IDObject = SelectedObject.ID,
                        IDInputInfo = dataInputInfoByObject.ID,
                        Count = Count,
                        Status = Status,
                        Object = SelectedObject,
                        InputInfo = dataInputInfoByObject,
                        SumPrice = Count* PriceObject.OutputPrice,
                    };
                    DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                    DataProvider.Ins.DB.SaveChanges();
                    try
                    {
                        
                        List.Add(outputInfo);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });

            EditCommand = new RelayCommand<ListView>((p) =>
            {
                if (SelectedObject == null || SelectedItem == null || Count==0 || ValidateErrorCount == true)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.IDOutput);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return true;

            }, (p) =>
            {
                if (CountObject() < Count)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }
                else
                {
                    var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                    var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == dataObjectByID.ID).First();
                    var outputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.ID == SelectedItem.ID).FirstOrDefault();
                    outputInfo.Output = SelectedItem.Output;
                    outputInfo.Object = SelectedObject;
                    outputInfo.InputInfo = dataInputInfoByObject;
                    outputInfo.IDObject = SelectedObject.ID;
                    outputInfo.IDInputInfo = dataInputInfoByObject.ID;
                    outputInfo.Count = Count;
                    outputInfo.Status = Status;
                    outputInfo.SumPrice = Count * PriceObject.OutputPrice;
                    outputInfo.Output.DateOutput = DateOutput;
                    DataProvider.Ins.DB.SaveChanges();
                    for (int i = 0; i < List.Count(); i++)
                    {
                        if (List[i].ID == SelectedItem.ID)
                        {
                            List[i] = new OutputInfo()
                            {
                                ID = SelectedItem.ID,
                                Output = List[i].Output,
                                IDOutput = SelectedItem.IDOutput,
                                IDObject = SelectedObject.ID,
                                IDInputInfo = dataInputInfoByObject.ID,
                                Count = Count,
                                Status = Status,
                                Object = SelectedObject,
                                InputInfo = dataInputInfoByObject,
                                SumPrice = PriceObject.OutputPrice * Count
                            };
                            SelectedItem = List[i];
                            p.SelectedItem = SelectedItem;
                            break;
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            });

            DeleteCommand = new RelayCommand<InputInfo>((p) =>
            {
                if (SelectedItem == null || List == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.IDOutput);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return true;

            }, (p) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var outputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var listOutput = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDOutput == SelectedItem.IDOutput).ToList();
                    Output output = new Output();
                    if (listOutput != null && listOutput.Count() != 0)//Xóa ID của Object trong InputInfo
                    {
                        var IDOutput = listOutput[0].IDOutput;
                        output = DataProvider.Ins.DB.Outputs.Where(x => x.ID == IDOutput).First();
                        DataProvider.Ins.DB.OutputInfoes.Remove(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    
                    foreach (var item in List)
                    {
                        if (item.ID == outputInfo.ID)
                        {
                            List.Remove(item);
                            break;
                        }

                    }
                    //List.Remove(outputInfo);
                    if (listOutput.Count() == 1)
                    {
                        DataProvider.Ins.DB.Outputs.Remove(output);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    SelectedItem = null;
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });


        }

        int CountObject()
        {
            InventoryList = new ObservableCollection<Inventory>();
            ThongKe = new ThongKe();
            var dataObjectByDisplayName = DataProvider.Ins.DB.Objects.Where(x => x.DisplayName == SelectedObject.DisplayName).ToList();
            int luongNhap = 0;
            int luongXuat = 0;


            foreach (var item in dataObjectByDisplayName)
            {
                var dataInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == item.ID);
                var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDObject == item.ID);

                int sumInputInfo = 0;
                int sumOutputInfor = 0;


                if (dataInputInfo != null && dataInputInfo.Count() > 0)
                {
                    sumInputInfo = (int)dataInputInfo.Sum(x => x.Count);

                    luongNhap += sumInputInfo;
                }

                if (dataOutputInfo != null && dataOutputInfo.Count() > 0)
                {
                    sumOutputInfor = (int)dataOutputInfo.Sum(x => x.Count);
                    luongXuat += sumOutputInfor;
                }
            }

            ThongKe.LuongTon = luongNhap - luongXuat;
            return ThongKe.LuongTon;
        }
    }
}
