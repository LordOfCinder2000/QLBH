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
    
    public class UnitViewModel:BaseViewModel
    {
        private ObservableCollection<Unit> _List;
        public ObservableCollection<Unit> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        //private ObservableCollection<Model.Object> _ListObject;
        //public ObservableCollection<Model.Object> ListObject { get => _ListObject; set { _ListObject = value; OnPropertyChanged(); } }

        //private ObservableCollection<InputInfo> _ListInputInfo;
        //public ObservableCollection<InputInfo> ListInputInfo { get => _ListInputInfo; set { _ListInputInfo = value; OnPropertyChanged(); } }

        //private ObservableCollection<OutputInfo> _ListOutputInfo;
        //public ObservableCollection<OutputInfo> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        private Unit _SelectedItem;
        public Unit SelectedItem { get => _SelectedItem; set {
                _SelectedItem = value; 
                OnPropertyChanged();
                if(SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }    
            } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private bool _ValidateErrorDisplayName;
        public bool ValidateErrorDisplayName { get => _ValidateErrorDisplayName; set { _ValidateErrorDisplayName = value; OnPropertyChanged(); } }


        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public UnitViewModel()
        {
            var ListUnit = DataProvider.Ins.DB.Units;
            if(ListUnit != null)
            {
                List = new ObservableCollection<Unit>(ListUnit);
            }

            //var ListObjects = DataProvider.Ins.DB.Objects;
            //if (ListObjects != null)
            //{
            //    ListObject = new ObservableCollection<Model.Object>(ListObjects);
            //}

            //var ListInputInfos = DataProvider.Ins.DB.InputInfoes;
            //if (ListInputInfos != null)
            //{
            //    ListInputInfo = new ObservableCollection<InputInfo>(ListInputInfos);
            //}

            //var ListOutputInfos = DataProvider.Ins.DB.OutputInfoes;
            //if (ListOutputInfos != null)
            //{
            //    ListOutputInfo = new ObservableCollection<OutputInfo>(ListOutputInfos);
            //}

            AddCommand = new RelayCommand<object>((p) => { 
                if(string.IsNullOrEmpty(DisplayName) || DisplayName =="" || ValidateErrorDisplayName==true)
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);
                if (displayList ==  null || displayList.Count() != 0)
                {
                    return false;
                }
                return true;
            }, (p) => {
                var unit = DataProvider.Ins.DB.Units.Add(new Unit() { DisplayName = DisplayName });
                DataProvider.Ins.DB.SaveChanges();
                List.Add(unit);
            });

            EditCommand = new RelayCommand<ListView>((p) => {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null || ValidateErrorDisplayName == true)
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                return true;
            }, (p) => {
                var unit = DataProvider.Ins.DB.Units.Where(pp => pp.ID == SelectedItem.ID).SingleOrDefault();
                unit.DisplayName = DisplayName;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].ID == SelectedItem.ID)
                    {
                        List[i] = new Unit() { ID = SelectedItem.ID, DisplayName = DisplayName };
                        p.SelectedItem = List[i];
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

                    var unit = DataProvider.Ins.DB.Units.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataObject = DataProvider.Ins.DB.Objects.Where(x => x.IDUnit == SelectedItem.ID).ToList();
                    //String IDObject = dataObject[0].ID;
                    var dataInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.Object.IDUnit == SelectedItem.ID).ToList();
                    //String IDInputInfo = dataInputInfo[0].ID;
                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.Object.IDUnit == SelectedItem.ID).ToList();


                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)//Xóa ID của object trong Out
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);
                            //ListOutputInfo.Remove(item);
                            //ListOutputInfo.Clear();
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    var dataInOut = DataProvider.Ins.DB.OutputInfoes.Where(x => x.InputInfo.Object.IDUnit == SelectedItem.ID).ToList();
                    if (dataInOut != null && dataInOut.Count() != 0)//Xóa ID của In trong Out
                    {
                        foreach (var item in dataInOut)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);
                            //ListOutputInfo.Remove(item);
                            //ListOutputInfo.Clear();
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    if (dataInputInfo != null && dataInputInfo.Count() != 0)//Xóa ID của object trong In
                    {
                        foreach (var item in dataInputInfo)
                        {
                            DataProvider.Ins.DB.InputInfoes.Remove(item);
                            //ListInputInfo.Remove(item);
                            //ListInputInfo.Clear();
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }

                    if (dataObject != null && dataObject.Count() != 0)//Xóa ID của unit trong object
                    {
                        foreach (var item in dataObject)
                        {
                            DataProvider.Ins.DB.Objects.Remove(item);
                            //ListObject.Remove(item);
                            //ListObject.Clear();
                            DataProvider.Ins.DB.SaveChanges();

                        }
                    }
                    foreach (var item in List)
                    {
                        if (item.ID == unit.ID)
                        {
                            List.Remove(item);
                            break;
                        }

                    }
                    DataProvider.Ins.DB.Units.Remove(unit);//Xóa unit
                    DataProvider.Ins.DB.SaveChanges();
                    //List.Remove(unit);

                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedItem = null;

                }
            });
        }
    }
}
