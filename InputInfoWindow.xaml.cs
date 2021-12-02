using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace QLBH
{
    /// <summary>
    /// Interaction logic for InputInfoWindow.xaml
    /// </summary>
    public partial class InputInfoWindow : Window
    {
        public InputInfoWindow()
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new ViewModel.InputInfoViewModel();
            
        }

    }
}
