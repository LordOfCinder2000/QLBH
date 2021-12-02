using QLBH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QLBH.ViewModel
{
    
    class LoginViewModel:BaseViewModel
    {
        public bool IsLogin { get; set; }

        private int _IDUserRole;
        public int IDUserRole { get => _IDUserRole; set { _IDUserRole = value; OnPropertyChanged(); } }

        private String _UserName;
        public String UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private String _PassWord;
        public String PassWord { 
            get => _PassWord; 
            set { _PassWord = value;
                OnPropertyChanged();
                if (PassWord != "")
                {
                    EyeColor = "Green";
                }
                else
                {
                    EyeColor = "#FF9DA0A5";
                }
            } 
        }

        private String _EyeColor;
        public String EyeColor { get => _EyeColor; set { _EyeColor = value; OnPropertyChanged(); } }

        public ICommand LoginWindowCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public ICommand ShowPasswordCommand { get; set; }
        public ICommand HiddenPasswordCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            PassWord = "";
            UserName = "";
            LoginWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null)
                    return;
                string PassWordEncode = Base64Encode(PassWord);
                PassWordEncode = MD5Encode(PassWordEncode);
                try
                {
                    var acc = DataProvider.Ins.DB.Users.Where(x => x.UserName == UserName && x.PassWord == PassWordEncode && x.DisplayName != null).ToList();//x thay cho từng record trang bảng try vấn
                    var accCount = acc.Count();
                
                    //accCount = 1; //auto dung tai khoan dang nhap
                    if (accCount == 1)
                    {

                        IDUserRole = acc[0].IDRole;
                        IsLogin = true;
                        p.Close();
                    }
                    else
                    {
                        IsLogin = false;
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PassWord = p.Password;
            });

            CloseWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

            ShowPasswordCommand = new RelayCommand<List<object>>((p) => { return true; }, (p) =>
            {
                ((Grid)p[2]).Children[0].Visibility = Visibility.Hidden;
                ((Grid)p[2]).Children[1].Visibility = Visibility.Visible;
                if(PassWord != "" && ((PasswordBox)p[0]).SecurePassword.Length > 0)
                {
                    ((PasswordBox)p[0]).Visibility = Visibility.Hidden;
                    ((TextBox)p[1]).Visibility = Visibility.Visible;
                    ((TextBox)p[1]).Text = ((PasswordBox)p[0]).Password;
                }
                
            });

            HiddenPasswordCommand = new RelayCommand<List<object>>((p) => { return true; }, (p) =>
            {
                ((Grid)p[2]).Children[0].Visibility = Visibility.Visible;
                ((Grid)p[2]).Children[1].Visibility = Visibility.Hidden;
                ((PasswordBox)p[0]).Visibility = Visibility.Visible;
                ((TextBox)p[1]).Visibility = Visibility.Hidden;
                ((PasswordBox)p[0]).Password = ((TextBox)p[1]).Text;
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
