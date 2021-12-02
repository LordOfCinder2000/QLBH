using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace QLBH.ViewModel
{
    class InputInfoViewModel:BaseViewModel
    {
        private ObservableCollection<InputInfo> _List;
        public ObservableCollection<InputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        //private ObservableCollection<Model.OutputInfo> _ListOutputInfo;
        //public ObservableCollection<Model.OutputInfo> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Object> _ListObject;
        public ObservableCollection<Model.Object> ListObject { get => _ListObject; set { _ListObject = value; OnPropertyChanged(); } }

        //private ObservableCollection<Input> _Input;
        //public ObservableCollection<Input> Input { get => _Input; set { _Input = value; OnPropertyChanged(); } }

        private Model.Object _SelectedObject;
        public Model.Object SelectedObject { get => _SelectedObject; set { _SelectedObject = value; OnPropertyChanged(); } }

        private Input _SelectedInput;
        public Input SelectedInput
        {
            get => _SelectedInput;
            set
            {
                _SelectedInput = value;
                OnPropertyChanged();
                if (SelectedInput != null)
                {
                    //MessageBox.Show(SelectedInput.ID);
                }
                    
            }
        }



        private InputInfo _SelectedItem;
        public InputInfo SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedObject = SelectedItem.Object;
                    SelectedInput = SelectedItem.Input;                  
                    SelectedInputDate = SelectedItem.Input.DateInput;
                    SelectedObjectText = SelectedItem.Object.DisplayName;
                    Count = SelectedItem.Count;
                    Status = SelectedItem.Status;
                    InputPrice = SelectedItem.InputPrice;
                    OutputPrice = SelectedItem.OutputPrice;
                }
            }
        }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private string _SelectedObjectText;
        public string SelectedObjectText { get => _SelectedObjectText; set { _SelectedObjectText = value; OnPropertyChanged(); } }

        private DateTime? _SelectedInputDate;
        public DateTime? SelectedInputDate { 
            get => _SelectedInputDate; 
            set 
            { 
                _SelectedInputDate = value; 
                OnPropertyChanged(); 
                if(SelectedInputDate!=null)
                {
                    //MessageBox.Show(SelectedInputDate.ToString());
                    SelectedInput.DateInput = SelectedInputDate;
                }    
            }
        }

        private bool _ValidateErrorCount;
        public bool ValidateErrorCount { get => _ValidateErrorCount; set { _ValidateErrorCount = value; OnPropertyChanged(); } }

        private bool _ValidateErrorDate;
        public bool ValidateErrorDate { get => _ValidateErrorDate; set { _ValidateErrorDate = value; OnPropertyChanged(); } }

        private bool _ValidateErrorInputPrice;
        public bool ValidateErrorInputPrice { get => _ValidateErrorInputPrice; set { _ValidateErrorInputPrice = value; OnPropertyChanged(); } }

        private bool _ValidateErrorOutputPrice;
        public bool ValidateErrorOutputPrice { get => _ValidateErrorOutputPrice; set { _ValidateErrorOutputPrice = value; OnPropertyChanged(); } }

        /*Chức năng nút*/
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }


        public InputInfoViewModel()
        {
            

            var ListInputInfo = DataProvider.Ins.DB.InputInfoes; 
            if (ListInputInfo != null)
            {
                List = new ObservableCollection<InputInfo>(ListInputInfo);
            }

            var ListObjects = DataProvider.Ins.DB.Objects;
            if (ListObjects != null)
            {
                ListObject = new ObservableCollection<Model.Object>(ListObjects);
            }

            SelectedInput = new Input();
            
            AddCommand = new RelayCommand<object>((p) =>
            {

                if (SelectedObject == null || SelectedInput.DateInput == null || ValidateErrorCount == true || Count == 0 || ValidateErrorDate == true || ValidateErrorInputPrice == true || ValidateErrorOutputPrice == true)
                    return false;

                return true;

            }, (p) =>
            {
                if (InputPrice > OutputPrice)
                {
                    MessageBox.Show("Giá xuất nhỏ hơn giá nhập !, Thêm không thành công.");
                }
                else
                {
                    var input = new Input()
                    {
                        ID = Guid.NewGuid().ToString(),
                        DateInput = SelectedInputDate
                    };

                    var inputInfo = new InputInfo()
                    {
                        ID = Guid.NewGuid().ToString(),
                        IDObject = SelectedObject.ID,
                        IDInput = input.ID,
                        Count = Count,
                        InputPrice = InputPrice,
                        OutputPrice = OutputPrice,
                        Status = Status
                    };

                    DataProvider.Ins.DB.Inputs.Add(input);
                    DataProvider.Ins.DB.InputInfoes.Add(inputInfo);
                    try
                    {
                        DataProvider.Ins.DB.SaveChanges();
                        List.Add(inputInfo);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            });

            EditCommand = new RelayCommand<ListView>((p) =>{
                if (SelectedItem == null || SelectedObject == null || SelectedInput == null || ValidateErrorCount == true || Count == 0 || ValidateErrorDate == true || ValidateErrorInputPrice == true || ValidateErrorOutputPrice == true)
                    return false;

                var displayList = DataProvider.Ins.DB.InputInfoes.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>{
                if (InputPrice > OutputPrice)
                {
                    MessageBox.Show("Giá xuất nhỏ hơn giá nhập !, Sửa không thành công.");
                }
                else
                {
                    var inputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    inputInfo.IDObject = SelectedObject.ID;
                    inputInfo.IDInput = SelectedInput.ID;

                    //InputInfo.idInput = Input.id;
                    inputInfo.Count = Count;
                    inputInfo.InputPrice = InputPrice;
                    inputInfo.OutputPrice = OutputPrice;
                    inputInfo.Status = Status;
                    DataProvider.Ins.DB.SaveChanges();
                    for (int i = 0; i < List.Count(); i++)
                    {
                        if (List[i].ID == SelectedItem.ID)
                        {
                            //SelectedInput = DataProvider.Ins.DB.Inputs.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                            List[i] = new InputInfo()
                            {
                                ID = SelectedItem.ID,
                                Object = SelectedObject,
                                Input = SelectedInput,
                                Count = Count,
                                InputPrice = InputPrice,
                                OutputPrice = OutputPrice,
                                Status = Status
                            };
                            p.SelectedItem = List[i];
                            break;
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            });

            DeleteCommand = new RelayCommand<InputInfo>((p) =>{
                if (SelectedItem == null || SelectedObject == null || SelectedInput == null)
                    return false;

                var displayList = DataProvider.Ins.DB.InputInfoes.Where(x => x.ID == SelectedItem.ID);
                if (displayList != null && displayList.Count() != 0)
                    return true;
                return false;

            }, (p) =>{
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var inputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    var dataOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IDInputInfo == SelectedItem.ID).ToList();
                    var dataInput = DataProvider.Ins.DB.Inputs.Where(x => x.ID == SelectedItem.Input.ID).ToList();

                    if (dataOutputInfo != null && dataOutputInfo.Count() != 0)
                    {
                        foreach (var item in dataOutputInfo)
                        {
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);
                            DataProvider.Ins.DB.SaveChanges();
                            //ListOutputInfo.Remove(item);
                        }
                        //dataOutputInfo.Clear();
                    }

                    DataProvider.Ins.DB.InputInfoes.Remove(inputInfo);
                    List.Remove(inputInfo);
                    DataProvider.Ins.DB.SaveChanges();

                    if (dataInput != null && dataInput.Count() != 0)
                    {
                        foreach (var item in dataInput)
                        {
                            DataProvider.Ins.DB.Inputs.Remove(item);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                    }
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedItem = null;
                }
            });
        }
    }
}
