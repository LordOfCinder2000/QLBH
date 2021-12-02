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
using System.Windows.Documents;
using System.Windows.Input;

namespace QLBH.ViewModel
{
    class OutputViewModel:BaseViewModel
    {
        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        private ObservableCollection<Output> _ListOutput;
        public ObservableCollection<Output> ListOutput { get => _ListOutput; set { _ListOutput = value; OnPropertyChanged(); } }

        private ObservableCollection<OutputInfo> _ListOutputInfoTemp;
        public ObservableCollection<OutputInfo> ListOutputInfoTemp { get => _ListOutputInfoTemp; set { _ListOutputInfoTemp = value; OnPropertyChanged(); } }

        private ObservableCollection<OutputInfo> _ListOutputInfo;
        public ObservableCollection<OutputInfo> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<InputInfo> _ListInputInfo;
        public ObservableCollection<InputInfo> ListInputInfo { get => _ListInputInfo; set { _ListInputInfo = value; OnPropertyChanged(); } }

        /*----- get data from other table -----*/
        private ObservableCollection<Customer> _ListCustomer;
        public ObservableCollection<Customer> ListCustomer { get => _ListCustomer; set { _ListCustomer = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _ListObject;
        public ObservableCollection<Model.Object> ListObject { get => _ListObject; set { _ListObject = value; OnPropertyChanged(); } }

        private ObservableCollection<User> _ListUser;
        public ObservableCollection<User> ListUser { get => _ListUser; set { _ListUser = value; OnPropertyChanged(); } }

        private ThongKe _ThongKe;
        public ThongKe ThongKe { get => _ThongKe; set { _ThongKe = value; OnPropertyChanged(); } }

        private PrintViewModel _Print;
        public PrintViewModel Print { get => _Print; set { _Print = value; OnPropertyChanged(); } }

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

        private Output _SelectedItem;
        public Output SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                SelectedObject = null;
                Count = null;
                StatusOfOutputInfo = null;
                PriceObject = null;
                if (SelectedItem != null)
                {
                    if(SelectedItem.Customer.Type == "Unknow")
                    {
                        CheckBoxChecked = true;
                        //CheckBoxEnabled = true;
                        SelectedCustomer = null;
                        CustomerInfoCommand.Execute(((CheckBox)(CheckBoxParameter)).CommandParameter);
                        ComboBoxText = SelectedItem.Customer.DisplayName;
                        SelectedCustomer = SelectedItem.Customer;
                        UnknowCustomerPhone = SelectedCustomer.Phone;
                        UnknowCustomerAddress = SelectedCustomer.Address;
                        DateOutput = SelectedItem.DateOutput;
                        SelectedUser = SelectedItem.User;
                        //SelectedUserText = SelectedItem.User.DisplayName;
                        //Count = SelectedItem.Count;
                        //SelectedInputInfo = SelectedItem.InputInfo;
                        ID = SelectedItem.ID;
                        Status = SelectedItem.Status;
                        Total = SelectedItem.Total;
                    }
                    else
                    {
                        CheckBoxChecked = false;
                        //CheckBoxEnabled = false;
                        CustomerInfoCommand.Execute(((CheckBox)(CheckBoxParameter)).CommandParameter);
                        SelectedCustomer = SelectedItem.Customer;
                        DateOutput = SelectedItem.DateOutput;
                        SelectedUser = SelectedItem.User;
                        SelectedUserText = SelectedItem.User.DisplayName;
                        //Count = SelectedItem.Count;
                        //SelectedInputInfo = SelectedItem.InputInfo;
                        ID = SelectedItem.ID;
                        Status = SelectedItem.Status;
                        Total = SelectedItem.Total;
                    }
                    
                    
                }
                //else
                //{
                //    SelectedCustomer = null;
                //    DateOutput = null;
                //    SelectedUser = null;
                //    //Count = SelectedItem.Count;
                //    //SelectedInputInfo = SelectedItem.InputInfo;
                //    ID = null;
                //    Status = null;
                //}    
            }
        }

        private OutputInfo _SelectedOutputInfo;
        public OutputInfo SelectedOutputInfo
        {
            get => _SelectedOutputInfo; set
            {
                _SelectedOutputInfo = value;
                OnPropertyChanged();
                if (SelectedOutputInfo != null)
                {
                     SelectedObject = SelectedOutputInfo.Object;
                     Count = SelectedOutputInfo.Count;
                     StatusOfOutputInfo = SelectedOutputInfo.Status;
                    //SelectedCustomer = SelectedItem.Customer;

                }
            }
        }

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
                if (SelectedCustomer != null)// && SelectedItem != null
                {
                    //CheckBoxEnabled = false;
                    //SelectedItem.IdCustomer = SelectedCustomer.Id;
                    //MessageBox.Show("khach change");
                }
                
            }
        }

        private User _SelectedUser;
        public User SelectedUser
        {
            get => _SelectedUser;
            set
            {
                _SelectedUser = value;
                OnPropertyChanged();
                if (SelectedUser != null)// && SelectedItem != null
                {
                    //SelectedItem.IdCustomer = SelectedCustomer.Id;
                    //MessageBox.Show("user change");
                }

            }
        }

        private Model.Object _SelectedObject;
        public Model.Object SelectedObject
        {
            get => _SelectedObject;
            set
            {
                _SelectedObject = value;
                OnPropertyChanged();
                if (SelectedObject != null)
                {


                    //double max = -1;
                    var priceMax = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == SelectedObject.ID )).Max(x=>x.OutputPrice);
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

        #region List

        private InputInfo _PriceObject;
        public InputInfo PriceObject { get => _PriceObject; set { _PriceObject = value; OnPropertyChanged(); } }

        private InputInfo _SelectedInputInfo;
        public InputInfo SelectedInputInfo { get => _SelectedInputInfo; set { _SelectedInputInfo = value; OnPropertyChanged(); } }

        //private User _SelectedUsers;
        //public User SelectedUsers { get => _SelectedUsers; set { _SelectedUsers = value; OnPropertyChanged(); } }

        private Promotion _SelectedPromotion;
        public Promotion SelectedPromotion { get => _SelectedPromotion; set { _SelectedPromotion = value; OnPropertyChanged(); } }

        private DateTime? _DateOutput;
        public DateTime? DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private string _ID;
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        //private int? _OutputObjectCount;
        //public int? OutputObjectCount { get => _OutputObjectCount; set { _OutputObjectCount = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _Sum;
        public double? Sum { get => _Sum; set { _Sum = value; OnPropertyChanged(); } }

        private double? _Total;
        public double? Total { 
            get => _Total; 
            set { _Total = value; OnPropertyChanged();
                if (Total != null)
                {
                    TotalString = String.Format("{0:0,0 VNĐ}", Total);
                }
            }
        }

        private string _TotalString;
        public string TotalString { get => _TotalString; set { _TotalString = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private string _StatusOfOutputInfo;
        public string StatusOfOutputInfo { get => _StatusOfOutputInfo; set { _StatusOfOutputInfo = value; OnPropertyChanged(); } }

        private string _SearchObject;
        public string SearchObject { 
            get => _SearchObject; 
            set { 
                _SearchObject = value; 
                OnPropertyChanged(); 
                if(!String.IsNullOrWhiteSpace(SearchObject) && ListObject!=null)
                {
                    bool flag = false;
                    foreach(Model.Object item in ListObject)
                    {
                        if(SearchObject == item.DisplayName)
                        {
                            flag = true;
                            break;
                        }    
                    }
                    if(!flag)
                    {
                        PriceObject = null;
                        Count = null;
                    }    
                }    
            } 
        }

        /*------- check box ---------*/
        private CheckBox _CheckBoxParameter;
        public CheckBox CheckBoxParameter { get => _CheckBoxParameter; set { _CheckBoxParameter = value;} }

        private bool _CheckBoxEnabled;
        public bool CheckBoxEnabled { get => _CheckBoxEnabled; set { _CheckBoxEnabled = value; OnPropertyChanged(); } }

        private bool _CheckBoxChecked;
        public bool CheckBoxChecked { get => _CheckBoxChecked; set { _CheckBoxChecked = value; OnPropertyChanged();} }

        private string _ComboBoxText;
        public string ComboBoxText { get => _ComboBoxText; set { _ComboBoxText = value; OnPropertyChanged(); } }

        private string _SelectedUserText;
        public string SelectedUserText { 
            get => _SelectedUserText; 
            set { 
                _SelectedUserText = value; 
                OnPropertyChanged();
                if (!String.IsNullOrWhiteSpace(SelectedUserText) && ListUser != null)
                {
                    foreach (Model.User item in ListUser)
                    {
                        if (SelectedUserText == item.DisplayName)
                        {
                            break;
                        }
                    }
                }
            } 
        }

        private string _UnknowCustomerPhone;
        public string UnknowCustomerPhone { get => _UnknowCustomerPhone; set { _UnknowCustomerPhone = value; OnPropertyChanged(); } }

        private string _UnknowCustomerAddress;
        public string UnknowCustomerAddress { get => _UnknowCustomerAddress; set { _UnknowCustomerAddress = value; OnPropertyChanged(); } }

        /*------- tinh tien ---------*/
        private string _CustomerMoney;
        public string CustomerMoney { get => _CustomerMoney; set { _CustomerMoney = value; OnPropertyChanged();
            //if(ValidateError == false)
            //    {
            //        MoneyReturn = Double.Parse(CustomerMoney) - Total;
            //    }
            //    if (CustomerMoney == "")
            //        MoneyReturn = null;
            }
        }

        private double? _MoneyReturn;
        public double? MoneyReturn { get => _MoneyReturn; set { _MoneyReturn = value; OnPropertyChanged(); } }


        /*------ validation ---------*/
        private bool _ValidateErrorCustomerMoney;
        public bool ValidateErrorCustomerMoney { get => _ValidateErrorCustomerMoney; set { _ValidateErrorCustomerMoney = value; OnPropertyChanged(); } }

        private bool _ValidateErrorCount;
        public bool ValidateErrorCount { get => _ValidateErrorCount; set { _ValidateErrorCount = value; OnPropertyChanged(); } }

        private bool _ValidateErrorPhone;
        public bool ValidateErrorPhone { get => _ValidateErrorPhone; set { _ValidateErrorPhone = value; OnPropertyChanged(); } }

        #endregion



        #region Command
        public ICommand LoadedWindowCommand { get; set; }

        /*------ chức năng select  -> hien thi  ---------*/
        public ICommand SelectedItemListViewChangedCommand { get; set; }
        public ICommand SelectedOutputInfoListViewChangedCommand { get; set; }


        /*------ chức năng nút cho cửa sổ output ---------*/
        public ICommand AddOutputCommand { get; set; }
        public ICommand EditOutputCommand { get; set; }
        public ICommand DeleteOutputCommand { get; set; }

        /*------ chức năng nút cho cửa sổ outputInfo ---------*/
        public ICommand AddOuputInfoCommand { get; set; }
        public ICommand EditOuputInfoCommand { get; set; }
        public ICommand DeleteOuputInfoCommand { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand PrintCommand { get; set; }

        public ICommand CustomerInfoCommand { get; set; }
        public ICommand PayCommand { get; set; }
        #endregion

        public OutputViewModel()
        {
          
            CheckBoxEnabled = true;
            ValidateErrorCustomerMoney = true;
            ValidateErrorCount = true;
            ValidateErrorPhone = false;
            //ListOutput = new ObservableCollection<Model.Output>(DataProvider.Ins.DB.Outputs);
            //ListOutputInfo = new ObservableCollection<OutputInfo>(DataProvider.Ins.DB.OutputInfo);
            //ListInputInfo = new ObservableCollection<InputInfo>(DataProvider.Ins.DB.InputInfoes);


            var ListCustomers = DataProvider.Ins.DB.Customers;
            if (ListCustomers != null)
            {
                ListCustomer = new ObservableCollection<Customer>(ListCustomers);
                for (int i = 0; i < ListCustomer.Count(); i++)
                {
                    if (ListCustomer[i].Type == "Unknow")
                    {
                        ListCustomer.Remove(ListCustomer[i]);
                        i--;
                    }
                }
            }

            var ListUsers = DataProvider.Ins.DB.Users;
            if (ListUsers != null)
            {
                ListUser = new ObservableCollection<User>(ListUsers);
                for (int i = 0; i < ListUser.Count(); i++)
                {
                    if (ListUser[i].DisplayName == null)
                    {
                        ListUser.Remove(ListUser[i]);
                        i--;
                    }
                }
            }

            var ListObjects = DataProvider.Ins.DB.Objects;
            if (ListObjects != null)
            {
                ListObject = new ObservableCollection<Model.Object>(ListObjects);
            }

            var ListOutputInfos = DataProvider.Ins.DB.OutputInfoes;
            if (ListOutputInfos != null)
            {
                ListOutputInfo = new ObservableCollection<OutputInfo>(ListOutputInfos);
            }

            var ListOutputs = DataProvider.Ins.DB.Outputs;
            if (ListOutputs != null)
            {
                ListOutput = new ObservableCollection<Output>(ListOutputs);
                for (int i = 0; i < ListOutput.Count(); i++)
                {
                    if (ListOutput[i].Total == null && ListOutput[i].OutputInfoes.Count > 0)
                    {
                        ListOutput.Remove(ListOutput[i]);
                        i--;
                    }
                }
            }

            ListOutputInfoTemp = new ObservableCollection<OutputInfo>();

            //LoadTotalPrice();

            LoadedWindowCommand = new RelayCommand<CheckBox>((p) => { return true; }, (p) =>
            {
                CheckBoxParameter = p;
            });

            RefreshCommand = new RelayCommand<List<object>>((p) => { return p == null ? false : true; }, (p) => 
            {
                CheckBoxChecked = false;
                ValidateErrorCustomerMoney = true;
                ValidateErrorPhone = false;
                ValidateErrorCount = true;
                CustomerInfoCommand.Execute(((CheckBox)(p[2])).CommandParameter);
                ((ComboBox)(p[1])).ItemsSource = ListCustomer;
                ((ComboBox)(p[1])).Text = null;
                CheckBoxEnabled = true;
                DateOutput = null;
                SelectedUser = null;
                SelectedObject = null;
                SelectedCustomer = null;
                SelectedItem = null;
                SelectedOutputInfo = null;
                ((ListView)(p[0])).SelectedItem = null;
                ListOutputInfoTemp.Clear();
                ID = Guid.NewGuid().ToString();
                PriceObject = null;
                Count = null;
                StatusOfOutputInfo = null;
                Status = null;
                Total = null;
                TotalString = "";
                CustomerMoney = "";
                MoneyReturn = null;
            });

            //Nhấn vào danh sách hóa đơn => chi tiết hóa đơn thay đổi theo
            SelectedItemListViewChangedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
               
               ListOutputInfoTemp.Clear();

               if (SelectedItem != null)
               {
                   var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDOutput == SelectedItem.ID).ToList();
                   foreach (var item in dataOutputInfo)
                   {
                       ListOutputInfoTemp.Add(item);
                   }
               }

               //    var Output = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();

               //    if (SelectedItem != null)
               //    {
               //         // Tính giá tiền nhập tương ứng với danh sách từng sản phẩm
               //         var collection = DataProvider.Ins.DB.OutputInfo.Where(x => x.IdOutput == SelectedItem.Id).ToList();
               //        foreach (var item in collection)
               //        {

               //            var InputInfo = DataProvider.Ins.DB.InputInfo.Where(x => x.IdInput == item.Id).SingleOrDefault();
               //            if (InputInfo != null)
               //            {
               //                Output.Total += item.Count * InputInfo.OutputPrice;
               //            }
               //            ListOutputInfo.Add(item);
               //            DataProvider.Ins.DB.SaveChanges();
               //        }
               //    }

               //     //DataProvider.Ins.DB.Output.Add(ListOutputInfo);
               //     DataProvider.Ins.DB.SaveChanges();

               //    ICollectionView view = CollectionViewSource.GetDefaultView(ListOutput);
               //    view.Refresh();
           });

            //SelectedOutputInfoListViewChangedCommand = new RelayCommand<object>((p) => true, (p) =>
            //{
            //    //SelectedObject = SelectedOutputInfo.Object;
            //    Count = SelectedOutputInfo.Count;
            //    Status = SelectedOutputInfo.Status;
            //});

            #region OutputCommand
            AddOutputCommand = new RelayCommand<ListView>((p) =>
            {
                if (ValidateErrorPhone == true)
                    return false;
                if ((CheckBoxChecked == true && SelectedCustomer == null && SelectedItem == null && SelectedUser != null && ListOutputInfoTemp.Count != 0))
                    return true;
                if (SelectedItem != null || SelectedUser == null || SelectedCustomer == null || ListOutputInfoTemp.Count == 0)
                    return false;
                return true;

            }, (p) =>
            {
                if((SelectedItem != null && DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID).Count() > 0))
                {
                    MessageBox.Show("Trùng mã hóa đơn, Nhấn \"Làm mới\" để tạo hóa đơn mới.");
                }
                else if ((SelectedCustomer != null && ListOutputInfoTemp.Count != 0 && ListOutputInfoTemp !=null))
                {

                    var output = new Output()
                    {
                        ID = ListOutputInfoTemp[0].IDOutput,
                        IDCustomer = SelectedCustomer.ID,
                        IDUser = SelectedUser.ID,
                        DateOutput = DateOutput,
                        Status = Status,
                       
                    };
                    
                    DataProvider.Ins.DB.Outputs.Add(output);
                    DataProvider.Ins.DB.SaveChanges();

                    

                    foreach (var item in ListOutputInfoTemp)
                    {
                        var outputInfo = new OutputInfo()
                        {
                            ID = item.ID,
                            IDOutput = output.ID,
                            IDObject = item.IDObject,
                            IDInputInfo = item.IDInputInfo,
                            Count = item.Count,
                            Status = item.Status,
                            Object = item.Object,
                            InputInfo = item.InputInfo,
                            SumPrice = item.SumPrice,
                            Output = output
                        };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    output.Total = LoadTotalPrice();
                    Total = output.Total;
                    ListOutput.Add(output);
                    DataProvider.Ins.DB.SaveChanges();


                    ListOutput[ListOutput.Count() - 1] = ListOutput[0];
                    ListOutput[0] = output;
                    SelectedItem = output;
                    p.SelectedItem = SelectedItem;
                    
                    //ListOutputInfo.Clear();
                }
                else //========================================================Khach vang lai
                {
                    //=======tao khach hang van lai==============//
                    if (ComboBoxText == null || ComboBoxText == "")
                    {
                        ComboBoxText = "NoName";
                    }
                    var customer = DataProvider.Ins.DB.Customers.Add(new Customer()
                    {
                        
                        DisplayName = ComboBoxText,
                        Address = UnknowCustomerAddress,
                        Phone = UnknowCustomerPhone,
                        ContractDate = DateTime.Today,
                        Type = "Unknow"
                    });
                    try
                    {
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    //===============them hoa don cho khach van lai =========//
                    var output = new Output()
                    {
                        ID = ListOutputInfoTemp[0].IDOutput,
                        IDCustomer = customer.ID,
                        IDUser = SelectedUser.ID,
                        DateOutput = DateOutput,
                        Status = Status,

                    };

                    DataProvider.Ins.DB.Outputs.Add(output);
                    DataProvider.Ins.DB.SaveChanges();



                    foreach (var item in ListOutputInfoTemp)
                    {
                        var outputInfo = new OutputInfo()
                        {
                            ID = item.ID,
                            IDOutput = output.ID,
                            IDObject = item.IDObject,
                            IDInputInfo = item.IDInputInfo,
                            Count = item.Count,
                            Status = item.Status,
                            Object = item.Object,
                            InputInfo = item.InputInfo,
                            SumPrice = item.SumPrice,
                            Output = output
                        };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    output.Total = LoadTotalPrice();
                    Total = output.Total;
                    ListOutput.Add(output);
                    DataProvider.Ins.DB.SaveChanges();


                    ListOutput[ListOutput.Count() - 1] = ListOutput[0];
                    ListOutput[0] = output;
                    SelectedItem = output;
                    p.SelectedItem = SelectedItem;
                }
                
               

                //else
                //{
                //    var Customer = new Model.Customer() { DisplayName = SelectedCustomer.DisplayName.ToString(), Address = SelectedCustomer.Address, Phone = SelectedCustomer.Phone };
                //    var Output = new Model.Output() { IdCustomer = Customer.Id, IdUser = SelectedUsers.Id, Id = Guid.NewGuid().ToString() };

                //    DataProvider.Ins.DB.Output.Add(Output);
                //    DataProvider.Ins.DB.SaveChanges();
                //    ListOutput.Add(Output);
                //}



                //var Customer = DataProvider.Ins.DB.Customer;
                //var Customer = DataProvider.Ins.DB.Customer.Where(x => x.Id == );
                //if (SelectedCustomer == null)
                //{

                //    foreach (var i in Customer)
                //    {
                //        if (SelectedCustomer.DisplayName != i.DisplayName)
                //        {
                //            //var Customer = new Model.Customer() { Id = SelectedCustomer.Id, DisplayName = SelectedCustomer.DisplayName, Address = SelectedCustomer.Address, Phone = SelectedCustomer.Phone };

                //            var Customer1 = new Model.Customer() { DisplayName = SelectedCustomer.DisplayName, Address = SelectedCustomer.Address, Phone = SelectedCustomer.Phone };

                //            DataProvider.Ins.DB.Customer.Add(Customer1);
                //            DataProvider.Ins.DB.SaveChanges();

                //            ICollectionView view = CollectionViewSource.GetDefaultView(ListOutput);
                //            view.Refresh();

                //            // var displayList = DataProvider.Ins.DB.Customer.Where(x => x.Id == Customer.Id).SingleOrDefault();
                //            //List.Add(Customer);
                //            var Output = new Model.Output() { IdCustomer = Customer1.Id, IdUser = SelectedUsers.Id, DateOutput = DateOutput, Id = Guid.NewGuid().ToString() };

                //            DataProvider.Ins.DB.Output.Add(Output);
                //            DataProvider.Ins.DB.SaveChanges();
                //            ListOutput.Add(Output);
                //        }
                //        else
                //        {
                //            var Output = new Model.Output() { IdCustomer = SelectedCustomer.Id, IdUser = SelectedUsers.Id, DateOutput = DateOutput, Id = Guid.NewGuid().ToString() };

                //            DataProvider.Ins.DB.Output.Add(Output);
                //            DataProvider.Ins.DB.SaveChanges();
                //            ListOutput.Add(Output);
                //        }
                //    }

                //}


                //, IdPromotion = SelectedPromotion.Id, Status = Status



            });

            EditOutputCommand = new RelayCommand<ListView>((p) =>
            {
                if (SelectedItem == null || SelectedUser == null || ListOutputInfoTemp == null || ValidateErrorPhone == true)
                {
                    
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() > 0)
                {
                    
                    return true;
                }    
                    
                return false;
            }, (p) =>
            {
                foreach (var item in ListOutputInfoTemp)
                {
                    var outputInfoEdited = DataProvider.Ins.DB.OutputInfoes.Where(x => x.ID == item.ID).FirstOrDefault();
                    outputInfoEdited.Output = item.Output;
                    outputInfoEdited.Object = item.Object;
                    outputInfoEdited.InputInfo = item.InputInfo;
                    outputInfoEdited.IDObject = item.IDObject;
                    outputInfoEdited.IDInputInfo = item.IDInputInfo;
                    outputInfoEdited.Count = item.Count;
                    outputInfoEdited.Status = item.Status;
                    outputInfoEdited.SumPrice = item.SumPrice;
                    DataProvider.Ins.DB.SaveChanges();
                }

                Total = LoadTotalPrice();
                var dataOutput = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                if(SelectedCustomer == null)
                {
                    dataOutput.IDCustomer = SelectedItem.Customer.ID;
                }
                else
                {
                    dataOutput.IDCustomer = SelectedCustomer.ID;
                }
                dataOutput.IDUser = SelectedUser.ID;
                dataOutput.DateOutput = DateOutput;
                dataOutput.Status = Status;
                dataOutput.Total = Total;
                DataProvider.Ins.DB.SaveChanges();
                if(CheckBoxChecked)
                {
                    var dataCustomerUnknow = DataProvider.Ins.DB.Customers.Where(x => x.ID == SelectedItem.Customer.ID).SingleOrDefault();
                    if (SelectedItem.Customer.Type == "Unknow" && SelectedCustomer !=null)
                    {
                        if (ComboBoxText == null || ComboBoxText == "")
                        {
                            ComboBoxText = "NoName";
                        }
                        dataCustomerUnknow.DisplayName = ComboBoxText;
                        dataCustomerUnknow.Phone = UnknowCustomerPhone;
                        dataCustomerUnknow.Address = UnknowCustomerAddress;
                        SelectedCustomer.Phone = UnknowCustomerPhone;
                        SelectedCustomer.Address = UnknowCustomerAddress;
                    }
                    else
                    {
                        
                        if (ComboBoxText == null|| ComboBoxText == "")
                        {
                            ComboBoxText = "NoName";
                        }
                        var customer = DataProvider.Ins.DB.Customers.Add(new Customer()
                        {

                            DisplayName = ComboBoxText,
                            Address = UnknowCustomerAddress,
                            Phone = UnknowCustomerPhone,
                            ContractDate = SelectedItem.Customer.ContractDate,
                            Type = "Unknow"
                        });
                        DataProvider.Ins.DB.SaveChanges();
                        SelectedItem.Customer = customer;
                        SelectedCustomer = SelectedItem.Customer;
                        SelectedCustomer.Phone = UnknowCustomerPhone;
                        SelectedCustomer.Address = UnknowCustomerAddress;
                        SelectedCustomer.Type = "Unknow";
                    }
                    
                    
                }
                SelectedItem.Customer = SelectedCustomer;
                SelectedItem.User = SelectedUser;
                SelectedItem.DateOutput = DateOutput;
                SelectedItem.Status = Status;
                SelectedItem.Total = Total;
                
                for (int i = 0; i < ListOutput.Count(); i++)
                {
                    if (ListOutput[i].ID == SelectedItem.ID)
                    {
                        ListOutput[i] = new Output()
                        {
                            ID = SelectedItem.ID,
                            Customer = SelectedCustomer,
                            User = SelectedUser,
                            DateOutput = DateOutput,
                            Status = Status,
                            Total = Total
                        };
                        p.SelectedItem = ListOutput[i];
                        break;
                    }
                }
                SelectedObject = null;
                Count = null;
                StatusOfOutputInfo = null;
                PriceObject = null;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            DeleteOutputCommand = new RelayCommand<Button>((p) =>
            {
                if(SelectedItem == null || ListOutput == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    var output = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDOutput == SelectedItem.ID).ToList();
                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)//Xóa ID của output trong OutpuInfo
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    ListOutput.Remove(SelectedItem);
                    DataProvider.Ins.DB.Outputs.Remove(output);
                    DataProvider.Ins.DB.SaveChanges();
                    SelectedItem = null;
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshCommand.Execute(((Button)(p)).CommandParameter);
                }
               
            });
            #endregion



            #region OutputInfoCommand


            AddOuputInfoCommand = new RelayCommand<object>((p) =>
            {
                if(ID == null&& SelectedItem == null)
                {
                    ID = Guid.NewGuid().ToString();
                }
                if (SelectedObject == null || Count == null || PriceObject == null || ValidateErrorCount == true || Count == 0)
                    return false;
                return true;
            }, (p) =>
            {



                /*---------- kiem tra xem hang con khong --------*/

                if (CountObject() < Count)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }
                else//Phan test
                {
                    if (SelectedItem == null)
                    {
                        
                        //foreach (var item in OuputPrice1)
                        //{
                        //    if (item.OutputPrice > max)
                        //    {
                        //        max = (double)item.OutputPrice;
                        //    }
                        //}
                        //var OuputPrice = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == SelectedObject.ID) && (x.OutputPrice == max)).FirstOrDefault();
                        //if (OuputPrice != null)
                        //{
                        //    PriceObject = OuputPrice;
                        //}

                        var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                        var priceMax = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == dataObjectByID.ID)).Max(x=> x.OutputPrice);
                        var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == dataObjectByID.ID)&&(x.OutputPrice==priceMax)).First();
                        var outputInfoTemp = new OutputInfo()
                        {
                            ID = Guid.NewGuid().ToString(),
                            IDOutput = ID,
                            IDObject = SelectedObject.ID,
                            IDInputInfo = dataInputInfoByObject.ID,
                            Count = Count,
                            Status = StatusOfOutputInfo,
                            Object = SelectedObject,
                            InputInfo = dataInputInfoByObject,
                            SumPrice = PriceObject.OutputPrice * Count,
                            
                        };

                        ListOutputInfoTemp.Add(outputInfoTemp);
                    }
                    else
                    {
                        
                        var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                        var priceMax = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == dataObjectByID.ID)).Max(x => x.OutputPrice);
                        var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => (x.IDObject == dataObjectByID.ID) && (x.OutputPrice == priceMax)).First();
                        var outputInfo = new OutputInfo()
                        {
                            ID = Guid.NewGuid().ToString(),
                            IDOutput = SelectedItem.ID,
                            IDObject = SelectedObject.ID,
                            IDInputInfo = dataInputInfoByObject.ID,
                            Count = Count,
                            Status = StatusOfOutputInfo,
                            Object = SelectedObject,
                            InputInfo = dataInputInfoByObject,
                            SumPrice = PriceObject.OutputPrice * Count
                        };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                        ListOutputInfoTemp.Add(outputInfo);
                    } 
                }


                /*Phan goc*/
                //if (SelectedItem != null)
                //{
                //    var dataOutput = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                //    ID = SelectedItem.ID;
                //}
                //else
                //{
                //    ID = Guid.NewGuid().ToString();
                //}
                //var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                //var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == dataObjectByID.ID).First();

                //if (ThongKe.LuongTon < Count)
                //{
                //    MessageBox.Show("Hàng trong kho đã hết");
                //}
                //else
                //{
                //    var outputInfo = new OutputInfo()
                //    {
                //        ID = Guid.NewGuid().ToString(),
                //        IDOutput = ID,
                //        IDObject = SelectedObject.ID,
                //        IDInputInfo = dataInputInfoByObject.ID,
                //        Count = Count,
                //        Status = Status,
                //        //Status = SelectedOutputInfo.Status,
                //    };
                //    //DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                //    //DataProvider.Ins.DB.SaveChanges();
                //    ListOutputInfo.Add(outputInfo);
                //}


                //ICollectionView view2 = CollectionViewSource.GetDefaultView(ListOutput);
                //view2.Refresh();




            });

            EditOuputInfoCommand = new RelayCommand<ListView>((p) =>
            {
                if (SelectedObject == null || SelectedOutputInfo == null || Count == null || ValidateErrorCount == true || Count == 0)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                if (CountObject() < Count)
                {
                    MessageBox.Show("Hàng trong kho đã hết");
                }
                else//Phan test
                {
                    if (SelectedItem == null)
                    {
                        


                        var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                        var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == dataObjectByID.ID).First();

                        for (int i = 0; i < ListOutputInfoTemp.Count(); i++)
                        {
                            if (ListOutputInfoTemp[i].ID == SelectedOutputInfo.ID)
                            {
                                ListOutputInfoTemp[i] = new OutputInfo()
                                {
                                    ID = SelectedOutputInfo.ID,
                                    Output = ListOutputInfoTemp[i].Output,
                                    IDOutput = ID,
                                    IDObject = SelectedObject.ID,
                                    IDInputInfo = dataInputInfoByObject.ID,
                                    Count = Count,
                                    Status = StatusOfOutputInfo,
                                    Object = SelectedObject,
                                    InputInfo = dataInputInfoByObject,
                                    SumPrice = PriceObject.OutputPrice * Count
                                };
                                p.SelectedItem = ListOutputInfoTemp[i];
                                break;
                            }
                        }
                        
                    }
                    else
                    {
                        var dataObjectByID = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedObject.ID).SingleOrDefault();
                        var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == dataObjectByID.ID).First();

                        for (int i = 0; i < ListOutputInfoTemp.Count(); i++)
                        {
                            if (ListOutputInfoTemp[i].ID == SelectedOutputInfo.ID)
                            {
                                ListOutputInfoTemp[i] = new OutputInfo()
                                {
                                    ID = SelectedOutputInfo.ID,
                                    Output = ListOutputInfoTemp[i].Output,
                                    IDOutput = SelectedItem.ID,
                                    IDObject = SelectedObject.ID,
                                    IDInputInfo = dataInputInfoByObject.ID,
                                    Count = Count,
                                    Status = StatusOfOutputInfo,
                                    Object = SelectedObject,
                                    InputInfo = dataInputInfoByObject,
                                    SumPrice = PriceObject.OutputPrice * Count
                                };
                                p.SelectedItem = ListOutputInfoTemp[i];
                                break;
                            }
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            });


            DeleteOuputInfoCommand = new RelayCommand<Output>((p) =>
            {
                if (SelectedOutputInfo == null || ListOutputInfoTemp == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.ID == SelectedOutputInfo.ID).SingleOrDefault();
                    if (dataOutputInfo == null)
                    {
                        ListOutputInfoTemp.Remove(SelectedOutputInfo);
                        SelectedOutputInfo = null;
                    }
                    else
                    {
                        DataProvider.Ins.DB.OutputInfoes.Remove(dataOutputInfo);
                        ListOutputInfo.Remove(dataOutputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                        Total = LoadTotalPrice();
                        ListOutputInfoTemp.Remove(SelectedOutputInfo);
                        //ICollectionView view1 = CollectionViewSource.GetDefaultView(ListOutputInfo);
                        //view1.Refresh();
                        SelectedOutputInfo = null;
                    }
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            });
            #endregion

            //PrintCommand = new RelayCommand<Output>((p) =>
            //{
            //    if (SelectedItem == null)
            //        return false;

            //    var displayList = DataProvider.Ins.DB.Output.Where(x => x.Id == SelectedItem.Id);
            //    if (displayList != null && displayList.Count() != 0)
            //        return true;
            //    return false;

            //}, (p) =>
            //{

            //    // rpw.DocViewer.Document = rpt;
            //    //ICollectionView view = CollectionViewSource.GetDefaultView(ListOutputInfo);
            //    //view.Refresh();

            //});

            /*---------- In hóa đơn --------*/
            PrintCommand = new RelayCommand<Page>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                var displayList = DataProvider.Ins.DB.Outputs.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>
            {
                PrintWindow window = new PrintWindow();
               
                PrintViewModel printViewModel = new PrintViewModel();
                printViewModel.Customer = SelectedItem.Customer;
                if(SelectedItem.DateOutput != null)
                    printViewModel.DateOutput = ((DateTime)SelectedItem.DateOutput).ToString("dd/MM/yyyy");              
                printViewModel.IDOutput = SelectedItem.ID;
                printViewModel.Phone = SelectedItem.Customer.Phone;
                printViewModel.Address = SelectedItem.Customer.Address;
                printViewModel.Total = String.Format("{0:0,0 VNĐ}", SelectedItem.Total);
                printViewModel.TotalConvert = printViewModel.NumberToText((double)SelectedItem.Total);
                var countOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDOutput == SelectedItem.ID).ToList();
                int count = 0;
                printViewModel.List = new Collection<HoaDon>();
                foreach(var item in countOutputInfo)
                {
                    
                    var dataInputInfoByObject = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == item.IDObject).First();
                    printViewModel.List.Add(new HoaDon
                    {
                        STT = count++,
                        DisplayName = item.Object.DisplayName,
                        Unit = item.Object.Unit.DisplayName,
                        Count = (int)item.Count,
                        Price = (double)dataInputInfoByObject.OutputPrice,
                        SumPrice = (double)item.SumPrice,
                        
                    });;

                }
                
                window.DataContext = printViewModel;
                window.ShowDialog();
            });


            //--------------------------Khach hang------------------------------------//
            
            CustomerInfoCommand = new RelayCommand<Grid>((p) => { return true; }, (p) => {
                var CustomerItem = p.Children;
                if (CheckBoxChecked)
                {
                    SelectedCustomer = null;
                    ComboBoxText = "NoName";
                    ((ComboBox)((((Grid)(CustomerItem[0])).Children)[0])).IsEditable = true;
                    ((ComboBox)((((Grid)(CustomerItem[0])).Children)[0])).ItemsSource = null;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[0])).IsReadOnly = false;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[0])).Visibility = Visibility.Hidden;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[1])).Visibility = Visibility.Visible;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[0])).IsReadOnly = false;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[0])).Visibility = Visibility.Hidden;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[1])).Visibility = Visibility.Visible;
                    ((TextBox)((((Grid)(CustomerItem[3])).Children)[0])).Text = null;
                }
                else
                {
                    ((ComboBox)((((Grid)(CustomerItem[0])).Children)[0])).Text = null;
                    ((ComboBox)((((Grid)(CustomerItem[0])).Children)[0])).ItemsSource = ListCustomer;
                    ((ComboBox)((((Grid)(CustomerItem[0])).Children)[0])).IsEditable = false;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[0])).IsReadOnly = true;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[0])).IsReadOnly = true;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[1])).Text = null;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[0])).Text = null;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[1])).Text = null;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[0])).Text = null;
                    ((TextBox)((((Grid)(CustomerItem[3])).Children)[0])).Text = null;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[0])).Visibility = Visibility.Visible;
                    ((TextBox)((((Grid)(CustomerItem[1])).Children)[1])).Visibility = Visibility.Hidden;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[0])).Visibility = Visibility.Visible;
                    ((TextBox)((((Grid)(CustomerItem[2])).Children)[1])).Visibility = Visibility.Hidden;
                }
            });

            //--------------------------Tinh tien------------------------------------//
            PayCommand = new RelayCommand<TextBox>((p) => 
            {
                if (CustomerMoney == null || CustomerMoney == "" || ValidateErrorCustomerMoney == true)
                    return false;
                return true; 
            }, (p) => {
                if(Total == null)
                {
                    MessageBox.Show("Chọn hóa đơn để thanh toán !", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MoneyReturn = Double.Parse(CustomerMoney) - Total;
                    if(MoneyReturn < 0)
                    {
                        MoneyReturn = null;
                        MessageBox.Show("Số tiền khách đưa phải lớn hơn tổng tiền hóa đơn !", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }    
                }    
               

            });
        }

        /*---------- kiem tra xem hang con khong --------*/
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

        /*---------- hàm tính tổng hóa đơn --------*/
        double LoadTotalPrice()
        {
            var outputList = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDOutput == ID).ToList();

            int i = 1;
            double total = 0;

            foreach (var item in outputList)
            {
                total += (double)item.SumPrice;
                //var outputInfoList = DataProvider.Ins.DB.OutputInfo.Where(p => p.IdOutput == item.Id);
                //double tongtien = 0;

                //if (outputInfoList != null && outputInfoList.Count() > 0)
                //{
                //    tongtien = (double)outputInfoList.Sum(p => p.SumPrice);
                //}
                //item.Total = tongtien;

                //i++;
            }
            //DataProvider.Ins.DB.SaveChanges();
            
            return total;
        }


        /*=========== Hàm tính tổng tiền 1 sp ==========*/


    }
}
