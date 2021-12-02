using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading;
using System.Globalization;
using QLBH.ViewModel;
using QLBH.Model;
using System.Collections.ObjectModel;

namespace QLBH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Inventory> listInventory;
        public bool IsSort;
        public MainWindow()
        {
           
            InitializeComponent();
            FormatDate();

  
            //IsSort = false;
            //sinhViens = new List<SinhVien>();
            //sinhViens.Add(new SinhVien() { MaSV = "SV01", TenSV = "Vũ Văn A", NamSinh = "19981117" });
            //sinhViens.Add(new SinhVien() { MaSV = "SV02", TenSV = "Vũ Văn B", NamSinh = "20051112" });
            //sinhViens.Add(new SinhVien() { MaSV = "SV04", TenSV = "Vũ Văn C", NamSinh = "20101123" });
            //sinhViens.Add(new SinhVien() { MaSV = "SV03", TenSV = "Vũ Văn D", NamSinh = "18901117" });
            
        }

        public class SinhVien
        {
            public String MaSV { get; set; }
            public String TenSV { get; set; }
            public String NamSinh { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test test = new Test();
            test.Show();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }


        public void FormatDate()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.LongDatePattern = "MMM/yyyy"; 
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; 
            Thread.CurrentThread.CurrentCulture = ci;
        }
        ////private void ControlBarUC_Loaded(object sender, RoutedEventArgs e)
        ////{

        ////}

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
           

        }

        //private void GridViewColumnHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    IsSort = false;
        //    this.DataContext = new MainViewModel();
            
        //    GridViewColumnHeader header = sender as GridViewColumnHeader;
        //    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listInventory);
        //    if (IsSort)
        //    {
        //        view.SortDescriptions.Clear();
        //        view.SortDescriptions.Add(new SortDescription(header.Tag.ToString(), ListSortDirection.Ascending));
        //    }
        //    else
        //    {
        //        view.SortDescriptions.Clear();
        //        view.SortDescriptions.Add(new SortDescription(header.Tag.ToString(), ListSortDirection.Descending));
        //    }
        //    IsSort = !IsSort;
        //}
    }
}
