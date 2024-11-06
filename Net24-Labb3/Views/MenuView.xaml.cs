using Net24_Labb3.Command;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Net24_Labb3.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public static RoutedCommand OpenFileCommand = new RoutedCommand();
        public static readonly RoutedCommand GetFullScreenCommand = new RoutedCommand();

        public ICommand FullScreenCommand { get; }
        public MenuView()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(OpenFileCommand, OpenFileMenuItem));
            this.InputBindings.Add(new KeyBinding(OpenFileCommand, Key.F, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(GetFullScreenCommand, ExecuteGetFullScreen));
        }

        private void ExecuteGetFullScreen(object sender, ExecutedRoutedEventArgs e)
        {
            GetFullScreen();
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            GetFullScreen();
        }

        private void GetFullScreen()
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow.WindowState == WindowState.Maximized && mainWindow.WindowStyle == WindowStyle.None)
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.WindowStyle = WindowStyle.None;
            }
        }

        private void OpenFileMenuItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (FileMenuItem.IsSubmenuOpen)
            {
                FileMenuItem.IsSubmenuOpen = false; 
            }
            else
            {
                FileMenuItem.IsSubmenuOpen = true; 
            }
        }

        

    }
}
