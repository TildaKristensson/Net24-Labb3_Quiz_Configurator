using Net24_Labb3.Command;
using Net24_Labb3.Dialogs;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net24_Labb3.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand OpenCreatePackDialogCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }

        private Question _activeQuestion;

        public Question ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }

        private string _query;
        public string Query { 
            get => ActiveQuestion.Query;
            set {
                ActiveQuestion.Query = value;
                RaisePropertyChanged("ActiveQuestion");
            }
        }

        private string _correctAnswer;

        public string CorrectAnswer {
            get => ActiveQuestion.CorrectAnswer; 
            set
            {
                ActiveQuestion.CorrectAnswer = value;
                RaisePropertyChanged(nameof(CorrectAnswer));
            }
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            OpenCreatePackDialogCommand = new DelegateCommand(OpenCreatePackDialog, CanOpenCreatePackDialog);

            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);
        }

        private bool CanOpenPackOptions(object? arg)
        {
            return true;
        }

        private void OpenPackOptions(object obj)
        {
            PackOptionsDialog packOptionsDialog = new();
            packOptionsDialog.ShowDialog();

            OpenPackOptionsCommand.RaiseCanExecuteChanged();
        }

  
        private bool CanOpenCreatePackDialog(object? arg)
        {
            return true;
        }

        private void OpenCreatePackDialog(object obj)
        {

            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();

            OpenCreatePackDialogCommand.RaiseCanExecuteChanged();
        }
    }
}
