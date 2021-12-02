using Microsoft.Win32;
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
using System.Windows.Media;

namespace QLBH.ViewModel
{
 
    class ObjectViewModel:BaseViewModel
    {
       
        private ObservableCollection<Model.Object> _List;
        public ObservableCollection<Model.Object> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Unit> _ListUnit;
        public ObservableCollection<Unit> ListUnit { get => _ListUnit; set { _ListUnit = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _ListSuplier;
        public ObservableCollection<Suplier> ListSuplier { get => _ListSuplier; set { _ListSuplier = value; OnPropertyChanged(); } }

        private ObservableCollection<OutputInfo> _ListOutputInfo;
        public ObservableCollection<OutputInfo> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<InputInfo> _ListInputInfo;
        public ObservableCollection<InputInfo> ListInputInfo { get => _ListInputInfo; set { _ListInputInfo = value; OnPropertyChanged(); } }

        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    QRCode = SelectedItem.QRCode;
                    BarCode = SelectedItem.BarCode;
                    SelectedUnit = SelectedItem.Unit;
                    SelectedSuplier = SelectedItem.Suplier;
                    SelectedSuplierText = SelectedItem.Suplier.DisplayName;
                    SelectedUnitText = SelectedItem.Unit.DisplayName;
                    FileName = SelectedItem.Image;
                    if (FileName == null)
                    {
                        Avatar = Visibility.Collapsed;
                        NoImage = Visibility.Visible;
                        CloseButtonVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        Avatar = Visibility.Visible;
                        NoImage = Visibility.Collapsed;
                    }
                }
            }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _QRCode;
        public string QRCode { get => _QRCode; set { _QRCode = value; OnPropertyChanged(); } }

        private string _BarCode;
        public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }

        private Unit _SelectedUnit;
        public Unit SelectedUnit { get => _SelectedUnit; set { _SelectedUnit = value; OnPropertyChanged(); } }

        private Suplier _SelectedSuplier;
        public Suplier SelectedSuplier { get => _SelectedSuplier; set { _SelectedSuplier = value; OnPropertyChanged(); } }

        private string _SelectedUnitText;
        public string SelectedUnitText { get => _SelectedUnitText; set { _SelectedUnitText = value; OnPropertyChanged(); } }

        private string _SelectedSuplierText;
        public string SelectedSuplierText { get => _SelectedSuplierText; set { _SelectedSuplierText = value; OnPropertyChanged(); } }


        private bool _ValidateErrorDisplayName;
        public bool ValidateErrorDisplayName { get => _ValidateErrorDisplayName; set { _ValidateErrorDisplayName = value; OnPropertyChanged(); } }

        /*Ảnh*/
        private string _FileName;
        public string FileName
        {
            get => _FileName; set
            {
                _FileName = value; OnPropertyChanged();
                if (FileName != null)
                    CloseButtonVisibility = Visibility.Visible;
                else
                    CloseButtonVisibility = Visibility.Hidden;
            }
        }

        private Visibility _CloseButtonVisibility;
        public Visibility CloseButtonVisibility { get => _CloseButtonVisibility; set { _CloseButtonVisibility = value; OnPropertyChanged(); } }

        private Visibility _Avatar;
        public Visibility Avatar { get => _Avatar; set { _Avatar = value; OnPropertyChanged(); } }

        private Visibility _NoImage;
        public Visibility NoImage { get => _NoImage; set { _NoImage = value; OnPropertyChanged(); } }




        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ICommand ImageCommand { get; set; }
        public ICommand CloseImageCommand { get; set; }
        public ObjectViewModel()
        {
            Avatar = Visibility.Collapsed;
            NoImage = Visibility.Visible;
            FileName = null;

            var ListObject = DataProvider.Ins.DB.Objects;
            if (ListObject != null)
            {
                List = new ObservableCollection<Model.Object>(ListObject);
            }
            
            var ListUnits = DataProvider.Ins.DB.Units;
            if (ListUnits != null)
            {
                ListUnit = new ObservableCollection<Unit>(ListUnits);
            }

            var ListSupliers = DataProvider.Ins.DB.Supliers;
            if (ListSupliers != null)
            {
                ListSuplier = new ObservableCollection<Suplier>(ListSupliers);
            }

            var ListOutputInfos = DataProvider.Ins.DB.OutputInfoes;
            if (ListOutputInfos != null)
            {
                ListOutputInfo = new ObservableCollection<OutputInfo>(ListOutputInfos);
            }

            var ListInputInfos = DataProvider.Ins.DB.InputInfoes;
            if (ListInputInfos != null)
            {
                ListInputInfo = new ObservableCollection<InputInfo>(ListInputInfos);
            }

            AddCommand = new RelayCommand<ListView>((p) => {
                if(string.IsNullOrEmpty(DisplayName) || SelectedUnit == null || SelectedSuplier == null || ValidateErrorDisplayName == true)
                {
                    return false;
                }    
                return true;
            }, (p) => {
                Boolean flag = true;
                foreach (var item in List)
                {
                    if (DisplayName == item.DisplayName)
                    {
                        MessageBox.Show("Tên mặt hàng đã tồn tại! Vui lòng nhập tên khác.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        flag = false;
                        break;
                    }

                }
                if (flag)
                {
                    var objects = DataProvider.Ins.DB.Objects.Add(new Model.Object()
                    {
                        ID = Guid.NewGuid().ToString(),
                        DisplayName = DisplayName,
                        IDSuplier = SelectedSuplier.ID,
                        IDUnit = SelectedUnit.ID,
                        QRCode = QRCode,
                        BarCode = BarCode,
                        Image = FileName
                    });
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(objects);
                    SelectedItem = objects;
                    p.SelectedItem = SelectedItem;
                }
            });

            EditCommand = new RelayCommand<ListView>((p) => {
                
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem==null ||  SelectedUnit == null || SelectedSuplier == null || ValidateErrorDisplayName == true)//string.IsNullOrEmpty(QRCode) || string.IsNullOrEmpty(BarCode) ||
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedItem.ID);
                if (displayList == null || displayList.Count() != 0)
                {
                    return true;
                }
                return false;
            }, (p) => {
                
                var objects = DataProvider.Ins.DB.Objects.Where(pp => pp.ID == SelectedItem.ID).SingleOrDefault();
                objects.DisplayName = DisplayName;
                objects.IDSuplier = SelectedSuplier.ID;
                objects.IDUnit = SelectedUnit.ID;
                objects.QRCode = QRCode;
                objects.BarCode = BarCode;
                objects.Image = FileName;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.Suplier.DisplayName = SelectedSuplier.DisplayName;
                SelectedItem.Unit.DisplayName = SelectedUnit.DisplayName;
                SelectedItem.QRCode = QRCode;
                SelectedItem.BarCode = BarCode;
                SelectedItem.Image = FileName;
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].ID == SelectedItem.ID)
                    {
                        List[i] = new Model.Object()
                        {
                            ID = SelectedItem.ID,
                            DisplayName = DisplayName,
                            Suplier = SelectedSuplier,
                            Unit = SelectedUnit,
                            QRCode = QRCode,
                            BarCode = BarCode,
                            Image = FileName
                        };
                        SelectedItem = List[i];
                        p.SelectedItem = SelectedItem;
                        break;
                    }
                }
                MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            DeleteCommand = new RelayCommand<ListView>((p) => {
                if (SelectedItem == null)
                {
                    return false;
                }
                //var displayList = DataProvider.Ins.DB.Units.Where(x => x.ID == SelectedItem.ID);
                //if (displayList == null || displayList.Count() == 0)
                //{
                //    return false;
                //}
                return true;
            }, (p) => {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var Object = DataProvider.Ins.DB.Objects.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IDObject == SelectedItem.ID).ToList();
                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDObject == SelectedItem.ID).ToList();

                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)//Xóa ID của Object trong OutputInfo
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    var dataInOut = DataProvider.Ins.DB.OutputInfoes.Where(x => x.InputInfo.IDObject == SelectedItem.ID).ToList();
                    if (dataInOut != null && dataInOut.Count() != 0)//Xóa ID của In trong Out
                    {
                        foreach (var item in dataInOut)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    if (dataInputInfo != null && dataInputInfo.Count() != 0)//Xóa ID của Object trong InputInfo
                    {
                        foreach (var item in dataInputInfo)
                        {
                            DataProvider.Ins.DB.InputInfoes.Remove(item);

                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    foreach (var item in List)
                    {
                        if(item.ID == Object.ID)
                        {
                            List.Remove(item);
                            break;
                        }
                           
                    }
                    DataProvider.Ins.DB.Objects.Remove(Object);
                    DataProvider.Ins.DB.SaveChanges();
                    //List.Remove(Object);

                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedItem = null;
                }
            });


            ImageCommand = new RelayCommand<ImageBrush>((p) => {

                return true;
            }, (p) => {
                OpenFileDialog ofd = new OpenFileDialog() { Filter = "*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png", ValidateNames = true, Multiselect = false };

                if (ofd.ShowDialog() == true)
                {
                    FileName = ofd.FileName;
                    //p.ImageSource = new BitmapImage(new Uri(FileName));
                    Avatar = Visibility.Visible;
                    NoImage = Visibility.Collapsed;
                }




            });

            CloseImageCommand = new RelayCommand<Button>((p) => {
                return true;
            }, (p) => {
                FileName = null;
                p.Visibility = Visibility.Hidden;
                Avatar = Visibility.Collapsed;
                NoImage = Visibility.Visible;
            });
        }
    }
}
