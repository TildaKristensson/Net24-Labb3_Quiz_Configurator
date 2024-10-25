using Net24_Labb3.Command;
using Net24_Labb3.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Net24_Labb3.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private DispatcherTimer timer;

        private string _testData;

        public string TestData
        {
            get => _testData;
            private set
            {
                _testData = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateButtonCommand { get; }

      
        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            TestData = "Start Value";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();

            UpdateButtonCommand = new DelegateCommand(UpdateButton, CanUpdateButton);
            //AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);

        }

       

        private bool CanUpdateButton(object? arg) => TestData.Length < 20;

        private void UpdateButton(object obj)
        {
            TestData += "X";
            UpdateButtonCommand.RaiseCanExecuteChanged();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestData += "X";
        }
    }
}
