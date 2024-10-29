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

        public DelegateCommand AddNewQuestionCommand { get; }

        public DelegateCommand RemoveQuestionCommand { get; }

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
                RaisePropertyChanged("ActiveQuestion");
            }
        }

        private string _incorrectAnswer1;

        public string IncorrectAnswer1 
        {
            get => ActiveQuestion.IncorrectAnswers[0]; 
            set
            {
                ActiveQuestion.IncorrectAnswers[0] = value;
                RaisePropertyChanged("ActiveQuestion");
            }
        }

        private string _incorrectAnswer2;
        public string IncorrectAnswer2
        {
            get => ActiveQuestion.IncorrectAnswers[1];
            set
            {
                ActiveQuestion.IncorrectAnswers[1] = value;
                RaisePropertyChanged("ActiveQuestion");
            }
        }

        private string _incorrectAnswer3;
        public string IncorrectAnswer3
        {
            get => ActiveQuestion.IncorrectAnswers[2];
            set
            {
                ActiveQuestion.IncorrectAnswers[2] = value;
                RaisePropertyChanged("ActiveQuestion");
            }
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            OpenCreatePackDialogCommand = new DelegateCommand(OpenCreatePackDialog, CanOpenCreatePackDialog);

            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);

            AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);

            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
        }

        private void RemoveQuestion(object obj)
        {
            ActivePack?.Questions.Remove(ActiveQuestion);
        }

        private bool CanRemoveQuestion(object? arg)
        {
            return true;
        }

        private bool CanAddNewQuestion(object? arg)
        {
            return true;
        }

        private void AddNewQuestion(object obj)
        {
            ActivePack?.Questions.Add(new Question(Query, CorrectAnswer, IncorrectAnswer1, IncorrectAnswer2, IncorrectAnswer3));

            AddNewQuestionCommand.RaiseCanExecuteChanged();
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
