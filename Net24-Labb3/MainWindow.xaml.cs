using Net24_Labb3.Model;
using Net24_Labb3.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Net24_Labb3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
           
        }

        private void StackPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindowViewModel = (MainWindowViewModel)this.DataContext;
            if (mainWindowViewModel?.ChooseQuestionCommand?.CanExecute(null) == true)
            {
                mainWindowViewModel.ChooseQuestionCommand.Execute(null);
            }
        }
    }
}