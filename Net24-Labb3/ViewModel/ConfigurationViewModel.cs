using Net24_Labb3.Command;
using Net24_Labb3.Dialogs;
using Net24_Labb3.FileHandlers;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Net24_Labb3.ViewModel
{
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand OpenCreatePackDialogCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }

        public DelegateCommand OpenEditCategoriesCommand { get; }

        public DelegateCommand AddNewQuestionCommand { get; }

        public DelegateCommand RemoveQuestionCommand { get; }

        public DelegateCommand RemovePackCommand { get; }

        public DelegateCommand SaveQuestionCommand { get; }

        public DelegateCommand AddNewCategoryCommand { get; }


        private readonly JsonFileHandler _jsonFileHandler;

        private QuestionPackViewModel _newCategory;

        public QuestionPackViewModel NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                RaisePropertyChanged();
            }
        }

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

      private Category _activeCategory;
        public Category ActiveCategory
        {
            get => _activeCategory;
            set
            {
                _activeCategory = value;
                RaisePropertyChanged();
            }
        }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            OpenCreatePackDialogCommand = new DelegateCommand(OpenCreatePackDialog, CanOpenCreatePackDialog);

            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);

            OpenEditCategoriesCommand = new DelegateCommand(OpenEditCategories);

            AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);

            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);

            AddNewCategoryCommand = new DelegateCommand(AddNewCategory);

            SaveQuestionCommand = new DelegateCommand(SaveQuestion);

            RemovePackCommand = new DelegateCommand(RemovePack);

            _jsonFileHandler = new JsonFileHandler();
        }

        private void AddNewCategory(object obj)
        {
            // 
            NewCategory?.Categories.Add(new Category("nameOfCategory"));
        }

        private void RemovePack(object obj)
        {
            mainWindowViewModel?.Packs.Remove(ActivePack);
            Task.Run(() => _jsonFileHandler.RemoveQuestionPackByName(ActivePack.Name));

            RemovePackCommand.RaiseCanExecuteChanged();
        }

        private bool CanRemoveQuestion(object? arg) { return true; }
        private void RemoveQuestion(object obj)
        {
            ActivePack?.Questions.Remove(ActiveQuestion);
            Task.Run(() => _jsonFileHandler.AddOrUpdateQuestionPack(ActivePack));

            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddNewQuestion(object? arg) { return true; }
        private void AddNewQuestion(object obj)
        {
            mainWindowViewModel.IsConfigurationMode = true;
            ActivePack?.Questions.Add(new Question("New Question", string.Empty, new string[] {"", "", ""}));

            AddNewQuestionCommand.RaiseCanExecuteChanged();
            
        }

        private void SaveQuestion(object obj)
        {
            Task.Run(() => _jsonFileHandler.AddOrUpdateQuestionPack(ActivePack));

            SaveQuestionCommand.RaiseCanExecuteChanged();
        }

        private bool CanOpenPackOptions(object? arg) {return true;}

        private void OpenPackOptions(object obj)
        {
            
            PackOptionsDialog packOptionsDialog = new();
            packOptionsDialog.ShowDialog();

            OpenPackOptionsCommand.RaiseCanExecuteChanged();
        }
        private bool CanOpenCreatePackDialog(object? arg) {return true;}

        private void OpenCreatePackDialog(object obj)
        {
            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();

            OpenCreatePackDialogCommand.RaiseCanExecuteChanged();
        }

        private void OpenEditCategories(object obj)
        {
            EditCategories editCategories = new();
            editCategories.ShowDialog();

            OpenEditCategoriesCommand.RaiseCanExecuteChanged();
        }
    }
}
