using Microsoft.Win32;
using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QLBH.ViewModel
{

    class UserViewModel : BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<UserRole> _ListUserRole;
        public ObservableCollection<UserRole> ListUserRole { get => _ListUserRole; set { _ListUserRole = value; OnPropertyChanged(); } }

        private User _SelectedItem;
        public User SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    UserName = SelectedItem.UserName;
                    SelectedUserRole = SelectedItem.UserRole;
                    SelectedUserRoleText = SelectedItem.UserRole.DisplayName;
                    FileName = SelectedItem.Image;
                    if(FileName == null)
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

        private string _SelectedUserRoleText;
        public string SelectedUserRoleText { get => _SelectedUserRoleText; set { _SelectedUserRoleText = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _Note;
        public string Note { get => _Note; set { _Note = value; OnPropertyChanged(); } }

        private string _FileName;
        public string FileName { get => _FileName; set { _FileName = value; OnPropertyChanged(); 
                if (FileName != null) 
                    CloseButtonVisibility = Visibility.Visible; 
                else
                    CloseButtonVisibility = Visibility.Hidden;
            } 
        }

        private UserRole _SelectedUserRole;
        public UserRole SelectedUserRole { get => _SelectedUserRole; set { _SelectedUserRole = value; OnPropertyChanged(); } }

        private int _CurrentRole;
        public int CurrentRole { get => _CurrentRole; set { _CurrentRole = value; OnPropertyChanged(); } }

        private MainWindow _MainWindowTemp;
        public MainWindow MainWindowTemp { get => _MainWindowTemp; set { _MainWindowTemp = value; OnPropertyChanged(); } }

        private bool _ValidateErrorDisplayName;
        public bool ValidateErrorDisplayName { get => _ValidateErrorDisplayName; set { _ValidateErrorDisplayName = value; OnPropertyChanged(); } }

        private bool _ValidateErrorUserName;
        public bool ValidateErrorUserName { get => _ValidateErrorUserName; set { _ValidateErrorUserName = value; OnPropertyChanged(); } }

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
        public ICommand ChangePasswordCommand { get; set; }

        public ICommand ImageCommand { get; set; }

        public ICommand CloseImageCommand { get; set; }

        public UserViewModel()
        {
            Avatar = Visibility.Collapsed;
            NoImage = Visibility.Visible;
            FileName = null;
            string PassWordDefault = "password";
            string PassWordDefaultEncode = Base64Encode(PassWordDefault);
            PassWordDefaultEncode = MD5Encode(PassWordDefaultEncode);
            var ListUser = DataProvider.Ins.DB.Users;
            LoadAndReset(ListUser, PassWordDefaultEncode);
            var ListUserRoles = DataProvider.Ins.DB.UserRoles;
            if (ListUserRoles != null)
            {
                ListUserRole = new ObservableCollection<UserRole>(ListUserRoles);              

            }


            
            AddCommand = new RelayCommand<ListView>((p) => {
                if (string.IsNullOrEmpty(DisplayName)|| string.IsNullOrEmpty(UserName)||SelectedUserRole==null || ValidateErrorUserName == true || ValidateErrorDisplayName == true)
                {
                    return false;
                }
                
                return true;
            }, (p) => {
                Boolean flag = true;
                foreach(var item in List)
                {
                    if(UserName == item.UserName)
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng nhập tên khác.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        flag = false;
                        break;
                    }    
                        
                }
                if(flag)
                {
                    var user = DataProvider.Ins.DB.Users.Add(new User()
                    {
                        DisplayName = DisplayName,
                        IDRole = SelectedUserRole.ID,
                        UserName = UserName,
                        PassWord = PassWordDefaultEncode,
                        Image = FileName,
                        Note = "* Mật khẩu mặc định là \"password\". Nhấn đổi mật khẩu để thay đổi."
                    });
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(user);

                    var temp = List[List.IndexOf(user)];
                    List[List.IndexOf(user)] = List[0];
                    List[0] = temp;
                    p.SelectedItem = List[List.IndexOf(user)];
                }  
            });

            EditCommand = new RelayCommand<ListView>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(UserName) || SelectedUserRole == null || ValidateErrorUserName == true || ValidateErrorDisplayName == true ||SelectedItem==null)
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Users.Where(x => x.ID == SelectedItem.ID);
                if (displayList == null || displayList.Count() != 0)
                {
                    return true;
                }
                return false;
            }, (p) =>
            {
                string notMessage = "* Mật khẩu mặc định là \"password\". Nhấn đổi mật khẩu để thay đổi.";
                var user = DataProvider.Ins.DB.Users.Where(pp => pp.ID == SelectedItem.ID).SingleOrDefault();
                user.DisplayName = DisplayName;
                user.UserName = UserName;
                user.UserRole = SelectedUserRole;
                user.PassWord = SelectedItem.PassWord;
                user.Image = FileName;
                user.Note = notMessage;
                if (SelectedItem.PassWord != PassWordDefaultEncode)
                {
                    notMessage = null;
                    user.Note = null;
                }    
                DataProvider.Ins.DB.SaveChanges();
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].ID == SelectedItem.ID)
                    {
                        List[i] = new User() { ID = SelectedItem.ID, DisplayName = DisplayName,UserName = UserName,UserRole = SelectedUserRole,PassWord=SelectedItem.PassWord,Note = notMessage, Image= FileName };
                        SelectedItem = List[i];
                        p.SelectedItem = SelectedItem;
                        break;
                    }
                }
                MessageBox.Show("Dữ liệu đã được sửa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                CurrentRole = Globals.IDUserRole;
                var userAll = List; 
                var checkLastManager = DataProvider.Ins.DB.Users.Where(x => (x.UserRole.ID == 1) && (x.DisplayName != null)).ToList();
                var checkLastAdmin = DataProvider.Ins.DB.Users.Where(x => (x.UserRole.ID == 4) && (x.DisplayName != null)).ToList();

                if (SelectedItem == null)
                {
                    return false;
                }
                else
                {
                    if (checkLastAdmin != null)
                    {
                        if (checkLastAdmin.Count == 1 && SelectedItem.ID == checkLastAdmin[0].ID)
                        {
                            return false;
                        }
                    }
                    if (checkLastManager != null)
                    {
                        if (checkLastManager.Count == 1 && SelectedItem.ID == checkLastManager[0].ID)
                        {
                            if (CurrentRole == 4)
                                return true;
                            return false;
                        }
                    }
                }
                
                return true;
            }, (p) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu này?",
                            "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {

                    var user = DataProvider.Ins.DB.Users.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();

                    user.DisplayName = null;
                    DataProvider.Ins.DB.SaveChanges();
                    foreach (var item in List)
                    {
                        if (item.ID == user.ID)
                        {
                            List.Remove(item);
                            break;
                        }

                    }
                    //List.Remove(user);
                    MessageBox.Show("Xóa thành công", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                SelectedItem = null;
            });

            ChangePasswordCommand = new RelayCommand<Window>((p) => {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) => {
                ChangePasswordWindow window = new ChangePasswordWindow();
                
                var dataContext = window.DataContext as ChangePasswordViewModel;
                dataContext.SelectedItem = SelectedItem;
                window.ShowDialog();
                ListUser = DataProvider.Ins.DB.Users;
                LoadAndReset(ListUser,PassWordDefaultEncode);

            });

            ImageCommand = new RelayCommand<ImageBrush>((p) => {
                
                return true;
            }, (p) => {
                OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false };
                
                if(ofd.ShowDialog()==true)
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

        public void LoadAndReset(System.Data.Entity.DbSet<User> ListUser, string PassWordDefaultEncode)
        {
            if (ListUser != null)
            {
                List = new ObservableCollection<User>(ListUser);
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].DisplayName == null)
                    {
                        List.Remove(List[i]);
                        i--;
                    }

                }
                for (int i = 0; i < List.Count(); i++)
                {
                    if (List[i].PassWord == PassWordDefaultEncode)
                    {
                        List[i].Note = "* Mật khẩu mặc định là \"password\". Nhấn đổi mật khẩu để thay đổi.";
                    }
                    else
                    {
                        List[i].Note = "";
                    }
                }
            }
        }

        public static string Base64Encode(string str)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Encode(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        //public BitmapImage ToImage(byte[] imageBytes)
        //{
        //    using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        BitmapImage im = new BitmapImage();
        //        im.SetSource(ms);
        //        return im;
        //    }
        //}
        ///* Pass the Byte[] from DB to get the BitmapImage */
        //this.Image = ToImage(byteArray);

        //private byte[] ToByteArray(BitmapImage bi)
        //{
        //    WriteableBitmap bmp = new WriteableBitmap(bi);
        //    int[] p = bmp.Pixels;
        //    int len = p.Length * 4;
        //    byte[] result = new byte[len];
        //    Buffer.BlockCopy(p, 0, result, 0, len);
        //    return result;
        //}

        ///* While saving the Bitmap Image to Database. Convert it to Byte[] and Save it to DB */
        //var ByteArray = ToByteArray(BitImage);

        ///* Your Logic to Save to DB */
        //SaveToDB(ByteArray);
    }
}
