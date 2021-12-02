using QLBH.Model;
using QLBH.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QLBH.ViewModel
{
    class ChangePasswordViewModel:BaseViewModel
    {
        private bool _ValidateErrorPassword;
        public bool ValidateErrorPassword { get => _ValidateErrorPassword; set { _ValidateErrorPassword = value; OnPropertyChanged(); } }

        private bool _ValidateErrorConfirmPassword;
        public bool ValidateErrorConfirmPassword { get => _ValidateErrorConfirmPassword; set { _ValidateErrorConfirmPassword = value; OnPropertyChanged(); } }

        private String _NewPassword;
        public String NewPassword
        {
            get => _NewPassword;
            set
            {
                _NewPassword = value;
                OnPropertyChanged();
                if (NewPassword != "" || NewPassword + ConfirmNewPassword != "")
                {
                    EyeColor = "Green";
                    ShowPasswordTextStyle = "Green";
                }
                else
                {
                    EyeColor = "#FF9DA0A5";
                    ShowPasswordTextStyle = "#FF9DA0A5";
                }
            }
        }

        private String _ConfirmNewPassword;
        public String ConfirmNewPassword
        {
            get => _ConfirmNewPassword;
            set
            {
                _ConfirmNewPassword = value;
                OnPropertyChanged();
                if (ConfirmNewPassword != "" || NewPassword + ConfirmNewPassword != "")
                {
                    EyeColor = "Green";
                    ShowPasswordTextStyle = "Green";
                }
                else
                {
                    EyeColor = "#FF9DA0A5";
                    ShowPasswordTextStyle = "#FF9DA0A5";
                }
            }
        }

        private String _EyeColor;
        public String EyeColor { get => _EyeColor; set { _EyeColor = value; OnPropertyChanged(); } }

        private String _ShowPasswordTextStyle;
        public String ShowPasswordTextStyle { get => _ShowPasswordTextStyle; set { _ShowPasswordTextStyle = value; OnPropertyChanged(); } }

        private String _ShowPasswordText;
        public String ShowPasswordText { get => _ShowPasswordText; set { _ShowPasswordText = value; OnPropertyChanged(); } }


        private User _SelectedItem;
        public User SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand PasswordChangedCommand { get; set; }
        public ICommand SaveNewPasswordCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public ICommand ShowPasswordCommand { get; set; }
        public ICommand HiddenPasswordCommand { get; set; }

        public ChangePasswordViewModel()
        {
            
            NewPassword = "";
            ConfirmNewPassword = "";
            ShowPasswordText = "Hiển thị mật khẩu";
            ValidateErrorPassword = true;
            ValidateErrorConfirmPassword = true;
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                if(p.Name == "NewPasswordBox")
                    NewPassword = p.Password;
                if(p.Name == "ConfirmNewPasswordBox")
                    ConfirmNewPassword = p.Password;
            });


            SaveNewPasswordCommand = new RelayCommand<PasswordBox>((p) => {
                
                if (NewPassword == "" || ConfirmNewPassword == "" || ValidateErrorPassword == true || ValidateErrorConfirmPassword == true)
                    return false;
                return true; 
            }, (p) =>
            {
                
                if (NewPassword == ConfirmNewPassword)
                {
                    string PassWordEncode = Base64Encode(NewPassword);
                    PassWordEncode = MD5Encode(PassWordEncode);
                    if (SelectedItem != null)
                    {
                        if (MessageBox.Show("Bạn có muốn thay đổi mật khẩu ?",
                                "Cautions", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                        {
                            var user = DataProvider.Ins.DB.Users.Where(x => x.ID == SelectedItem.ID).First();
                            user.PassWord = PassWordEncode;
                            DataProvider.Ins.DB.SaveChanges();
                            MessageBox.Show("Thay đổi thành công !", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }    
                else
                {
                    MessageBox.Show("Xác nhận lại mật khẩu không đúng !", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }    
                
            });

            CloseWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                
                p.Close();
            });

            ShowPasswordCommand = new RelayCommand<List<object>>((p) => { return true; }, (p) =>
            {
                ShowPasswordText = "Ẩn mật khẩu";
                ((Grid)p[4]).Children[0].Visibility = Visibility.Hidden;
                ((Grid)p[4]).Children[1].Visibility = Visibility.Visible;
                //if (NewPassword != "" && ((PasswordBox)p[0]).SecurePassword.Length > 0)
                //{
                    Panel.SetZIndex(((PasswordBox)p[0]),-1);
                    ((TextBox)p[1]).Visibility = Visibility.Visible;
                    Panel.SetZIndex(((TextBox)p[1]), 1);
                    ((TextBox)p[1]).Text = ((PasswordBox)p[0]).Password;
                    var bc = new BrushConverter();
                    ((TextBox)p[1]).Background = (Brush)bc.ConvertFrom("#FFF0F0F0");
                //}
                //if (ConfirmNewPassword != "" && ((PasswordBox)p[2]).SecurePassword.Length > 0)
                //{
                    Panel.SetZIndex(((PasswordBox)p[2]), -1);
                    ((TextBox)p[3]).Visibility = Visibility.Visible;
                    Panel.SetZIndex(((TextBox)p[1]), 1);
                    ((TextBox)p[3]).Text = ((PasswordBox)p[2]).Password;
                    //var bc = new BrushConverter();
                    ((TextBox)p[3]).Background = (Brush)bc.ConvertFrom("#FFF0F0F0");
                //}

            });

            HiddenPasswordCommand = new RelayCommand<List<object>>((p) => { return true; }, (p) =>
            {
                ShowPasswordText = "Hiển thị mật khẩu";
                ((Grid)p[4]).Children[0].Visibility = Visibility.Visible;
                ((Grid)p[4]).Children[1].Visibility = Visibility.Hidden;
                
                Panel.SetZIndex(((PasswordBox)p[0]), 1);
                ((TextBox)p[1]).Visibility = Visibility.Hidden;
                Panel.SetZIndex(((TextBox)p[1]), -1);
                ((PasswordBox)p[0]).Password = ((TextBox)p[1]).Text;

                Panel.SetZIndex(((PasswordBox)p[2]), 1);
                ((TextBox)p[3]).Visibility = Visibility.Hidden;
                Panel.SetZIndex(((TextBox)p[3]), -1);
                ((PasswordBox)p[2]).Password = ((TextBox)p[3]).Text;
            });
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
    }
}
