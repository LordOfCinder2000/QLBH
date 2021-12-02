using LiveCharts;
using LiveCharts.Wpf;
using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QLBH.ViewModel
{
    class MainViewModel:BaseViewModel
    {
        //xử lý trong này, là datacontext của main
        public bool isLoaded = false;
        public bool hasDate = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitWindowCommand { get; set; }
        public ICommand SuplierWindowCommand { get; set; }
        public ICommand CustomerWindowCommand { get; set; }
        public ICommand ObjectWindowCommand { get; set; }
        public ICommand UserWindowCommand { get; set; }
        public ICommand InputInfoWindowCommand { get; set; }
        public ICommand OutputWindowCommand { get; set; }
        public ICommand OutputInfoWindowCommand { get; set; }
        public ICommand ThongKeCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand MouseEnterButton { get; set; }
        public ICommand MouseLeaveButton { get; set; }
        public ICommand MouseLeftButtonDown { get; set; }

        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList { get => _InventoryList; set { _InventoryList = value; OnPropertyChanged(); } }

        private ThongKe _ThongKe;
        public ThongKe ThongKe { get => _ThongKe; set { _ThongKe = value; OnPropertyChanged(); } }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _UserRole;
        public string UserRole { get => _UserRole; set { _UserRole = value; OnPropertyChanged(); } }

        private int _IDUserRole;
        public int IDUserRole { get => _IDUserRole; set { _IDUserRole = value; OnPropertyChanged(); } }

        private int _IDUser;
        public int IDUser { get => _IDUser; set { _IDUser = value; OnPropertyChanged(); } }

        private string _Avatar;
        public string Avatar { get => _Avatar; set { _Avatar = value; OnPropertyChanged(); } }

        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                IDUserRole = 0;
                isLoaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DataContext == null)
                    return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                //hien thi tai khoan
                
                //========//
                if(loginVM.IsLogin)
                {
                    UserName = loginVM.UserName;
                    UserRole = DataProvider.Ins.DB.UserRoles.Where(x => x.ID == loginVM.IDUserRole).First().DisplayName;
                    IDUser = DataProvider.Ins.DB.Users.Where(x => (x.UserName == UserName) && (x.DisplayName !=null)).First().ID;
                    Avatar = DataProvider.Ins.DB.Users.Where(x => x.ID == IDUser).First().Image;
                    if (Avatar == null)
                        Avatar = "/Images/userdefault.jpg";
                    IDUserRole = loginVM.IDUserRole;
                    p.Show();
                    
                    LoadInventoryData(false);
                }    
                else 
                {
                    p.Close();
                }    
            });

            

            UnitWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("unit"))
                    return false;
                return true; 
            }, (p) => {
                UnitWindow window = new UnitWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            SuplierWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("suplier"))
                    return false;
                return true;
            }, (p) => {
                SuplierWindow window = new SuplierWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            CustomerWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("customer"))
                    return false;
                return true;
            }, (p) => {
                CustomerWindow window = new CustomerWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            ObjectWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("object"))
                    return false;
                return true; 
            }, (p) => {
                ObjectWindow window = new ObjectWindow();
                
                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            UserWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("user"))
                    return false;
                return true; 
            }, (p) => {
                Globals.IDUserRole = IDUserRole;
                UserWindow window = new UserWindow();
                
                
                //((UserViewModel)(window.DataContext)).CurrentRole = IDUserRole;
                //window.ShowDialog();
                //LoadInventoryData();
                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            InputInfoWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("inputInfo"))
                    return false;
                return true; 
            }, (p) => {
                InputInfoWindow window = new InputInfoWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            OutputWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("output"))
                    return false;
                return true; 
            }, (p) => {
                OutputWindow window = new OutputWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            OutputInfoWindowCommand = new RelayCommand<Window>((p) => {
                if (IDUserRole == 0 || !AccessRole(IDUserRole).Contains("outputInfo"))
                    return false;
                return true;
            }, (p) => {

                Globals.IDUser = IDUser;
                OutputInfoWindow window = new OutputInfoWindow();

                //window.ShowDialog();
                //LoadInventoryData();

                p.Hide();
                window.ShowDialog();
                p.Show();
                LoadInventoryData(false);
            });

            LogOutCommand = new RelayCommand<List<object>>((p) => { return true; }, (p) => {
                //p.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
                //LoadedWindowCommand.Execute(p);
                //this.LoadedWindowCommand.Execute(p[0]);
                //((Button)p[1]).Visibility = Visibility.Collapsed;
                //((Button)p[2]).Visibility = Visibility.Visible;
                //cho dù close() nhưng vẫn giữ datacontext cũ
                var mainWindow = new MainWindow();
                mainWindow.DataContext = new MainViewModel();
                ((Window)p[0]).Close();
                mainWindow.Show();
                
            });

            RefreshCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadInventoryData(true); });

            MouseEnterButton = new RelayCommand<Button>((p) => { return true; }, (p) => {
                
                //p.Background = Brushes.Red;
                if(p.Name == "LogOutButton")
                {
                    p.Background = Brushes.Red;
                    p.Foreground = Brushes.White;
                    p.BorderThickness = new Thickness(5, 0, 0, 0);
                    p.BorderBrush = Brushes.White;
                }
                else
                {
                    p.BorderThickness = new Thickness(5, 0, 0, 0);
                    var bc = new BrushConverter();
                    p.BorderBrush = (Brush)bc.ConvertFrom("#FFAEEA00");
                }
                
            });

            MouseLeaveButton = new RelayCommand<Button>((p) => { return true; }, (p) => {
                //p.Background = Brushes.Green;
                if (p.Name == "LogOutButton")
                {
                    var bc = new BrushConverter();
                    p.Background = (Brush)bc.ConvertFrom("#FFAEEA00");
                    p.Foreground = Brushes.Red;
                    p.BorderBrush = null;
                    p.BorderThickness = new Thickness(0, 0, 0, 0);
                    
                }
                else
                {
                    p.BorderBrush = null;
                    p.BorderThickness = new Thickness(0, 0, 0, 0);
                } 
            });

            MouseLeftButtonDown = new RelayCommand<Button>((p) => { return true; }, (p) => {
                //p.Visibility = Visibility.Visible;
                
            });

            ThongKeCommand = new RelayCommand<Axis>(
                (p) => 
                {
                    if (SelectedKindDate == null||SelectedDate==null)
                        return false;
                    return true; 
                }, (p) => {
                    switch (SelectedKindDate)
                    {
                        case "Năm":
                            p.MaxValue = 12;
                            LoadChartByYear();
                            break;
                        case "Tháng":
                            p.MaxValue = LoadChartByMonth();
                            p.LabelsRotation = 40;
                            break;
                        case "Tuần":
                            p.MaxValue = 7;
                            LoadChartByWeek();
                            break;
                        default:
                            LoadChartByDay();
                            break;
                    }
                });

            FilterCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedKindObject == null && Filter == null)
                        return false;
                    return true;
                }, (p) => {
                    
                    LoadFilter(SelectedKindObject);
               
                });


            


            ListKindDate = new List<string>() { };
            ListKindDate.Add("Năm");
            ListKindDate.Add("Tháng");
            ListKindDate.Add("Tuần");
            ListKindDate.Add("Ngày");

            ListKindObject = new List<string>() { };
            ListKindObject.Add("Không chọn");
            ListKindObject.Add("Hàng bán chạy nhất");
            ListKindObject.Add("Hàng bán ế nhất");
        }

        //==================================== Phân quyền ========================//
        List<string> AccessRole(int IDRole)
        {
            List<string> canAccess = new List<string>();
            switch (IDRole)
            {
                case 1:
                    canAccess.Add("unit");
                    canAccess.Add("suplier");
                    canAccess.Add("customer");
                    canAccess.Add("object");
                    canAccess.Add("user");
                    canAccess.Add("inputInfo");
                    canAccess.Add("outputInfo");
                    canAccess.Add("output");
                    return canAccess;
                case 2:
                    canAccess.Add("object");
                    canAccess.Add("customer");
                    canAccess.Add("outputInfo");
                    canAccess.Add("output");
                    return canAccess;
                case 3:
                    canAccess.Add("unit");
                    canAccess.Add("suplier");
                    canAccess.Add("customer");
                    canAccess.Add("object");
                    canAccess.Add("inputInfo");
                    canAccess.Add("outputInfo");
                    canAccess.Add("output");
                    return canAccess;
                case 4:
                    canAccess.Add("unit");
                    canAccess.Add("suplier");
                    canAccess.Add("customer");
                    canAccess.Add("object");
                    canAccess.Add("user");
                    canAccess.Add("inputInfo");
                    canAccess.Add("outputInfo");
                    canAccess.Add("output");
                    return canAccess;
            }
            return null;
        }


        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnPropertyChanged(); } }

        private List<DataForCharts> _DataForChart;
        public List<DataForCharts> DataForChart { get => _DataForChart; set { _DataForChart = value; OnPropertyChanged(); } }

        private SeriesCollection _PieChartSeriesCollection;
        public SeriesCollection PieChartSeriesCollection { get => _PieChartSeriesCollection; set { _PieChartSeriesCollection = value; OnPropertyChanged(); } }

        private SeriesCollection _PieChartSeriesCollectionForSP;
        public SeriesCollection PieChartSeriesCollectionForSP { get => _PieChartSeriesCollectionForSP; set { _PieChartSeriesCollectionForSP = value; OnPropertyChanged(); } }


        private SeriesCollection _CartesianChartSeriesCollection;
        public SeriesCollection CartesianChartSeriesCollection { get => _CartesianChartSeriesCollection; set { _CartesianChartSeriesCollection = value; OnPropertyChanged(); } }


        private SeriesCollection _AxisY;
        public SeriesCollection AxisY { get => _AxisY; set { _AxisY = value; OnPropertyChanged(); } }

        private String[] _Labels;
        public String[] Labels { get => _Labels; set { _Labels = value; OnPropertyChanged(); } }

        private LegendLocation _Legend;
        public LegendLocation Legend { get => _Legend; set { _Legend = value; OnPropertyChanged(); } }

        private Visibility _VisibilityOfTitle;
        public Visibility VisibilityOfTitle { get => _VisibilityOfTitle; set { _VisibilityOfTitle = value; OnPropertyChanged(); } }

        private DateTime _DateBeginInventory;
        public DateTime DateBeginInventory { get => _DateBeginInventory; set { _DateBeginInventory = value; OnPropertyChanged(); } }

        //private ChartValues<int> _a;
        //public ChartValues<int> a { get => _a; set { _a = value; OnPropertyChanged(); } }
       
        public Dictionary<string, int> DictionaryOfData { get; private set; }


        #region hàm cho chart
        public void LoadChart(ThongKe thongKe)
        {
            /*================================Test chart====================================*/
            PieChartSeriesCollection = new SeriesCollection();
            DictionaryOfData = new Dictionary<string, int>();
            float tong = (thongKe.LuongTon + thongKe.LuongXuat);
            float phanTranHT = (thongKe.LuongTon / tong) * 100;
            float phanTranHB = 100 - phanTranHT;
            DictionaryOfData.Add("Hàng tồn kho", thongKe.LuongTon);
            DictionaryOfData.Add("Hàng đã bán", thongKe.LuongXuat);
            foreach (KeyValuePair<string, int> pair in DictionaryOfData)
            {
                
                PieChartSeriesCollection.Add(
                    new PieSeries
                    {
                        Title = $"{(pair.Value/tong)*100}% ({pair.Key})",
                        Values = new ChartValues<int> { pair.Value },
                        DataLabels = false
                    });
            }
            DictionaryOfData.Clear();
            if(InventoryList != null)
            {
                
                float tongHangCon = thongKe.LuongTon;
                PieChartSeriesCollectionForSP = new SeriesCollection();
                foreach (Inventory item in InventoryList)
                {
                    DictionaryOfData.Add(item.Object.DisplayName, item.CountInventory);
                }
                foreach (KeyValuePair<string, int> pair in DictionaryOfData)
                {

                    PieChartSeriesCollectionForSP.Add(
                        new PieSeries
                        {
                            Title = $"{(pair.Value / tongHangCon) * 100}% ({pair.Key})",
                            Values = new ChartValues<int> { pair.Value },
                            DataLabels = false

                        });
                }
                VisibilityOfTitle = Visibility.Collapsed;
                Legend = LegendLocation.Bottom;
                if (PieChartSeriesCollectionForSP.Count() > 3)
                {
                    Legend = LegendLocation.None;
                    VisibilityOfTitle = Visibility.Visible;
                }
            }



            /*============================bieu do cot======================================*/

            MaxValue = 7;
            LoadChartByWeek();

            /*=============================================================================*/

        }

        private int _MaxValue;
        public int MaxValue { get => _MaxValue; set { _MaxValue = value; OnPropertyChanged(); } }

        private List<String> _ListKindDate;
        public List<String> ListKindDate { get => _ListKindDate; set { _ListKindDate = value; OnPropertyChanged(); } }

        private DateTime? _SelectedDate;
        public DateTime? SelectedDate { get => _SelectedDate; set { _SelectedDate = value; OnPropertyChanged(); } }

        private String _SelectedKindDate;
        public String SelectedKindDate { get => _SelectedKindDate; set { _SelectedKindDate = value; OnPropertyChanged(); } }


        public void LoadChartByYear()
        {
            MaxValue = 12;
            DictionaryOfData = new Dictionary<string, int>();
            CartesianChartSeriesCollection = new SeriesCollection();
            CartesianChartSeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Tổng hàng bán được",
                    Values = new ChartValues<int> {}
                });

            if (SelectedDate != null)
            {
                var groupByYear = DataProvider.Ins.DB.OutputInfoes.Where(p => p.Output.DateOutput.Value.Year == SelectedDate.Value.Year).ToList();
                var groupByMonth = groupByYear.GroupBy(p => p.Output.DateOutput.Value.Month).
                          Select(pp => new
                          {
                              pp.Key,
                              SUM = pp.Sum(ppp => ppp.Count)
                          }).ToList();
                if(groupByMonth.Count!=0)
                {
                    for (int i = 1; i <= MaxValue; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < groupByMonth.Count(); j++)
                        {
                            if (groupByMonth[j].Key == i)
                            {
                                CartesianChartSeriesCollection[0].Values.Add(groupByMonth[j].SUM);
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        if (flag == false)
                        {
                            CartesianChartSeriesCollection[0].Values.Add(0);
                        }
                    }
                }
            }
            Labels = new[] {"Tháng 1","Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
           
        }



        public int LoadChartByMonth()
        {
             
            DictionaryOfData = new Dictionary<string, int>();
            CartesianChartSeriesCollection = new SeriesCollection();
            CartesianChartSeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Tổng hàng bán được",
                    Values = new ChartValues<int> { }
                });

            #region tinh day of month
            Labels = new string[32];
            
            for (int i = 0; i < 31; i++)
            {
                Labels[i] = ("Ngày " + (i+1));
            }
            if (DateTime.DaysInMonth(SelectedDate.Value.Year,SelectedDate.Value.Month).Equals(31))
            {
                MaxValue = 31;
            }    
            else if(DateTime.DaysInMonth(SelectedDate.Value.Year, SelectedDate.Value.Month).Equals(30))
            {
                MaxValue = 30;
            }    
            else if(DateTime.DaysInMonth(SelectedDate.Value.Year, SelectedDate.Value.Month).Equals(29))
            {
                MaxValue = 29;
            }
            else
            {
                MaxValue = 28;
            }
            #endregion

            if (SelectedDate != null)
            {
                var groupByYear = DataProvider.Ins.DB.OutputInfoes.Where(p => p.Output.DateOutput.Value.Year == SelectedDate.Value.Year).ToList();
                var groupByMonth = groupByYear.Where(p => p.Output.DateOutput.Value.Month == SelectedDate.Value.Month).ToList();
                var groupByDay = groupByMonth.GroupBy(p => p.Output.DateOutput.Value.Day).
                          Select(pp => new
                          {
                              pp.Key,
                              SUM = pp.Sum(ppp => ppp.Count)
                          }).ToList();
                if(groupByDay.Count != 0)
                {
                    for (int i = 1; i <= MaxValue; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < groupByDay.Count(); j++)
                        {
                            if (groupByDay[j].Key == i)
                            {
                                CartesianChartSeriesCollection[0].Values.Add(groupByDay[j].SUM);
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        if (flag == false)
                        {
                            CartesianChartSeriesCollection[0].Values.Add(0);
                        }
                    }
                }    
                


            }
            return MaxValue;
            //Labels = new[] { "Ngày 1", "Ngày 2", "Ngày 3", "Ngày 4", "Ngày 5", "Ngày 6", "Ngày 7", "Ngày 8", "Ngày 9", "Ngày 10", "Ngày 11", "Ngày 12", "Ngày 13", "Ngày 14", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12", "Ngày 12" };
        }

        //public static bool swap<T>(this T[] objectArray, int x, int y)
        //{

        //    if (objectArray.Length <= y || objectArray.Length <= x) return false;
        //    T buffer = objectArray[x];
        //    objectArray[x] = objectArray[y];
        //    objectArray[y] = buffer;


        //    return true;
        //}

        public void LoadChartByWeek()
        {
            SelectedDate = DateTime.Now;
            SelectedKindDate = "Tuần";
            //string[] weekDays = new CultureInfo("en-us").DateTimeFormat.DayNames.;
            //swap<string>(weekDays, 0, weekDays.Length - 1);
            //swap<string>(weekDays, 0, weekDays.Length - 1);
            string[] weekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            DictionaryOfData = new Dictionary<string, int>();
            CartesianChartSeriesCollection = new SeriesCollection();
            CartesianChartSeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Tổng hàng bán được",
                    Values = new ChartValues<int> { }
                });

            if (SelectedDate != null)
            {
                var groupByYear = DataProvider.Ins.DB.OutputInfoes.Where(p => p.Output.DateOutput.Value.Year == SelectedDate.Value.Year).ToList();
                var groupByMonth = groupByYear.Where(p => p.Output.DateOutput.Value.Month == SelectedDate.Value.Month).ToList();        

                var monday = SelectedDate.Value.AddDays(-(int)SelectedDate.Value.DayOfWeek + (int)DayOfWeek.Monday);
                var sunday = SelectedDate.Value.AddDays(7- (int)SelectedDate.Value.DayOfWeek);

                var groupByDay = groupByMonth.Where(p => (p.Output.DateOutput.Value >=monday) && (p.Output.DateOutput.Value <= sunday)).ToList();
                var groupByWeek = groupByDay.GroupBy(p => p.Output.DateOutput.Value.DayOfWeek).
                Select(pp => new
                {
                    pp.Key,
                    SUM = pp.Sum(ppp => ppp.Count)
                }).ToList();
                if (groupByWeek.Count != 0)
                {
                    for (int i=0;i<weekDays.Length;i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < groupByWeek.Count(); j++)
                        {
                            if (groupByWeek[j].Key.ToString() == weekDays[i])
                            {
                                CartesianChartSeriesCollection[0].Values.Add(groupByWeek[j].SUM);
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        if (flag == false)
                        {
                            CartesianChartSeriesCollection[0].Values.Add(0);
                        }
                    }
                }
                Labels = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            }
        }


        public void LoadChartByDay()
        {
            string[] weekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            DictionaryOfData = new Dictionary<string, int>();
            CartesianChartSeriesCollection = new SeriesCollection();

            if (SelectedDate != null)
            {
                var groupByYear = DataProvider.Ins.DB.OutputInfoes.Where(p => p.Output.DateOutput.Value.Year == SelectedDate.Value.Year).ToList();
                var groupByMonth = groupByYear.Where(p => p.Output.DateOutput.Value.Month == SelectedDate.Value.Month).ToList();
                var groupByDay = groupByMonth.Where(p => p.Output.DateOutput.Value.Day == SelectedDate.Value.Day).ToList();
                var groupByObjectName = groupByDay.GroupBy(p => p.Object.DisplayName).
                Select(pp => new
                {
                    pp.Key,
                    SUM = pp.Sum(ppp => ppp.Count)
                }).ToList();
                Labels = new String[groupByObjectName.Count];
                groupByObjectName = groupByObjectName.OrderBy(o => o.SUM).ToList();
                if (groupByObjectName.Count != 0)
                {
                    for(int i=0;i< groupByObjectName.Count;i++)
                    {
                        CartesianChartSeriesCollection.Add(
                        new ColumnSeries
                        {
                            Title = groupByObjectName[i].Key,
                            Values = new ChartValues<int> { }
                        });
                        CartesianChartSeriesCollection[i].Values.Add(groupByObjectName[i].SUM);
                        //Labels[i] = groupByObjectName[i].Key;
                    }    
                }
                
            }
        }
        #endregion


        #region Lọc
        private bool _ReadOnly;
        public bool ReadOnly { get => _ReadOnly; set { _ReadOnly = value; OnPropertyChanged(); } }

        private List<String> _ListKindObject;
        public List<String> ListKindObject { get => _ListKindObject; set { _ListKindObject = value; OnPropertyChanged(); } }

        private String _SelectedKindObject;
        public String SelectedKindObject { 
            get => _SelectedKindObject; 
            set {
                _SelectedKindObject = value; 
                OnPropertyChanged();
                if (SelectedKindObject != null && SelectedKindObject != "Không chọn")
                {
                    Filter = "";
                    ReadOnly = true;
                }
                else
                {
                    ReadOnly = false;
                }
            } }

        public void LoadFilter(String kindObject)
        {
            if (SelectedKindObject != null && SelectedKindObject != "Không chọn")
            {
                LoadInventoryData(true);
                
                if (kindObject == "Hàng bán chạy nhất")
                {
                    var Max = InventoryList.Max(p => p.CountOutput);
                    var ListFilter = InventoryList.Where(p => p.CountOutput == Max).ToList();
                    InventoryList.Clear();
                    foreach (Inventory item in ListFilter)
                    {
                        InventoryList.Add(item);
                    }
                }
                else if(kindObject == "Hàng bán ế nhất")
                {
                    var Min = InventoryList.Min(p => p.CountOutput);
                    var ListFilter = InventoryList.Where(p => p.CountOutput == Min).ToList();
                    InventoryList.Clear();
                    foreach (Inventory item in ListFilter)
                    {
                        InventoryList.Add(item);
                    }
                }    

            }
            else
            {
                LoadInventoryData(true);
                var ListFilter = InventoryList.Where(p => p.Object.DisplayName.ToLower().Replace(" ", "") == Filter.ToLower().Replace(" ", "")).ToList();
                InventoryList.Clear();
                foreach(Inventory item in ListFilter)
                {
                    InventoryList.Add(item);
                }
                
            }    
        }
        #endregion
        public void LoadInventoryData(bool flag)
        {
            
            /*Củ chuối*/
            //InventoryList = new ObservableCollection<Inventory>();
            //var InputInfo = DataProvider.Ins.DB.InputInfoes.GroupBy(p => p.IDObject).
            //      Select(pp => new
            //      {
            //          pp.Key,
            //          SUM = pp.Sum(ppp => ppp.Count)
            //      }).ToList();
            //var OutputInfo = DataProvider.Ins.DB.OutputInfoes.GroupBy(p => p.IDObject).
            //      Select(pp => new
            //      {
            //          pp.Key,
            //          SUM = pp.Sum(ppp => ppp.Count)
            //      }).ToList();
            //int STT = 0;
            //foreach (var item1 in InputInfo)
            //{
            //    STT++;
            //    Inventory inventory = new Inventory();
            //    var ObjectInfo = DataProvider.Ins.DB.Objects.Where(p=>p.ID==item1.Key).ToList();

            //    foreach (var item2 in OutputInfo)
            //    {
            //        if (item1.Key == item2.Key)
            //        {
            //            inventory.Count = (int)(item1.SUM - item2.SUM);
            //            inventory.Object = ObjectInfo[0];
            //            inventory.STT = STT;
            //            InventoryList.Add(inventory);
            //            break;
            //        }
            //        else if(item2.Equals(OutputInfo.Last()))
            //        {
            //            inventory.Count = (int)(item1.SUM);
            //            inventory.Object = ObjectInfo[0];
            //            inventory.STT = STT;
            //            InventoryList.Add(inventory);
            //        }
            //    }
            //}

            /*Xịn*/

            InventoryList = new ObservableCollection<Inventory>();
            ThongKe = new ThongKe();
            var ojectList = DataProvider.Ins.DB.Objects;

            int luongNhap = 0;
            int luongXuat = 0;
            
            
            double tongTienTon = 0;
            double tongTienLai = 0;
            int Stt = 1;

            var dataInputInfo = DataProvider.Ins.DB.InputInfoes.ToList();
            var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.ToList();
            foreach (var item in ojectList)
            {
                double tongTienXuat = 0;
                double tongTienNhap = 0;
                var inputList = DataProvider.Ins.DB.InputInfoes.Where(p => p.IDObject == item.ID);
                var outputList = DataProvider.Ins.DB.OutputInfoes.Where(p => p.IDObject == item.ID);

                int sumInput = 0;
                int sumOutput = 0;
                double tienNhap = 0;
                double tienXuat = 0;
                foreach(var objectItem in dataInputInfo)
                {
                    if(objectItem.IDObject == item.ID)
                    {
                        tienNhap = (double)(objectItem.Count * objectItem.InputPrice);
                        tongTienNhap += tienNhap;
                    }                   
                }
                foreach (var objectItem in dataOutputInfo)
                {
                    if (objectItem.IDObject == item.ID)
                    {
                        tienXuat = (double)objectItem.SumPrice;
                        tongTienXuat += tienXuat;
                    }
                }
                if (inputList != null && inputList.Count() != 0)
                {
                    sumInput = (int)inputList.Sum(p => p.Count);
                    tienNhap = (double)inputList.Sum(p => p.InputPrice);
                    ////tienXuat = (double)inputList.Sum(p => p.OutputPrice);
                    //luongNhap += sumInput;
                }
                if (outputList != null && outputList.Count() != 0)
                {
                    sumOutput = (int)outputList.Sum(p => p.Count);
                    //luongXuat += sumOutput;
                }

                //tongTienTon += (sumInput - sumOutput) * tienNhap;
                //tongTienLai += sumOutput * (tienXuat - tienNhap);
                //tongTienNhap += sumInput * tienNhap;
                //tongTienXuat += sumOutput * tienXuat;

                Inventory inventory = new Inventory();
                inventory.STT = Stt;
                inventory.CountInput = sumInput;
                inventory.CountOutput = sumOutput;
                inventory.CountInventory = sumInput - sumOutput;
                inventory.MoneyInput = tongTienNhap;
                inventory.MoneyOutput = tongTienXuat;
                //inventory.MoneyInventory = (sumInput - sumOutput) * tienNhap;
                if (tongTienXuat - tongTienNhap > 0)
                {
                    inventory.MoneyEarn = tongTienXuat - tongTienNhap;
                }
                else
                    inventory.MoneyEarn = 0;
                inventory.Object = item;
                InventoryList.Add(inventory);
                Stt++;
            
                ThongKe.LuongNhap += inventory.CountInput;
                ThongKe.LuongXuat += inventory.CountOutput;
                ThongKe.GiaNhap += inventory.MoneyInput;
                ThongKe.GiaXuat += inventory.MoneyOutput;
                ThongKe.LuongTon += inventory.CountInventory;
                //ThongKe.GiaTon = tongTienTon;
                ThongKe.GiaLai += inventory.MoneyEarn;
            }
            if(!flag)
                LoadChart(ThongKe);
        }
    }
}
