using Net24_Labb3.ViewModel;
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

namespace Net24_Labb3.Dialogs
{
    /// <summary>
    /// Interaction logic for PackOptionsDialog.xaml
    /// </summary>
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();

            DataContext = App.Current.MainWindow.DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
