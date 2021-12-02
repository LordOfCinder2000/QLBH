using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QLBH.ViewModel
{
    public class ControlBarViewModel:BaseViewModel
    {
        private PackIcon _KindIcon;
        public PackIcon KindIcon { get => _KindIcon; set { _KindIcon = value; OnPropertyChanged(); } }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        public ICommand MouseEnterWindowCommand { get; set; }
        public ICommand MouseLeaveWindowCommand { get; set; }
        public ControlBarViewModel()
        {
            KindIcon = new PackIcon();
            KindIcon.Kind = PackIconKind.WindowMaximize;
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p==null? false: true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {                  
                    w.Close(); 
                }
               
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    
                    if (w.WindowState != WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Maximized;
                        KindIcon.Kind = PackIconKind.WindowRestore;
                    }
                    else
                    {
                        w.WindowState = WindowState.Normal;
                        KindIcon.Kind = PackIconKind.WindowMaximize;
                    }
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if(w.WindowState != WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                }
            });

            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    //var a = w.FindName("ScrollBar");
                    w.DragMove();
                }
            });

            //MouseEnterWindowCommand = new RelayCommand<Button>((p) => { return p == null ? false : true; }, (p) =>
            //{
            //    p.Content = MaterialDesignColors
            //});

            //MouseLeaveWindowCommand = new RelayCommand<Button>((p) => { return p == null ? false : true; }, (p) =>
            //{
            //    //p.Background = Brushes.Transparent;
            //});
        }

        public FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;
            while(parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
