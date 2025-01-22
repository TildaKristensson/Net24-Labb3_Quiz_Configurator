using MongoDB.Driver;
using Net24_Labb3.Command;
using Net24_Labb3.Data;
using Net24_Labb3.Dialogs;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration.Internal;
using System.Drawing.Design;
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

        private readonly QuestionPackViewModel? questionPackViewModel;

        public DelegateCommand OpenCreatePackDialogCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }

        public DelegateCommand OpenEditCategoriesCommand { get; }

        public DelegateCommand AddNewQuestionCommand { get; }

        public DelegateCommand RemoveQuestionCommand { get; }

        public DelegateCommand RemovePackCommand { get; }

        public DelegateCommand SaveCategoryCommand { get; }

        public DelegateCommand UpdateCategoryCommand { get; }

        public DelegateCommand SaveQuestionCommand { get; }

        public DelegateCommand AddNewCategoryCommand { get; }
        public DelegateCommand RemoveCategoryCommand { get; }

        private readonly QuizDbs _quizDbs;

        private Category _newCategory;

        public Category NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                RaisePropertyChanged();
            }
        }

        public QuestionPack? ActivePack { get => mainWindowViewModel.ActivePack; }


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
                RaisePropertyChanged(nameof(ActiveCategory));
            }
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }


        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _quizDbs = new QuizDbs();

            OpenCreatePackDialogCommand = new DelegateCommand(OpenCreatePackDialog, CanOpenCreatePackDialog);

            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);

            OpenEditCategoriesCommand = new DelegateCommand(OpenEditCategories);

            AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);

            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);

            AddNewCategoryCommand = new DelegateCommand(AddNewCategory);
            RemoveCategoryCommand = new DelegateCommand(RemoveCategory);

            SaveQuestionCommand = new DelegateCommand(SaveQuestion);

            SaveCategoryCommand = new DelegateCommand(SaveCategory);

            UpdateCategoryCommand = new DelegateCommand(UpdateCategory);

            RemovePackCommand = new DelegateCommand(RemovePack);

            Categories = new ObservableCollection<Category>();
            
        }

        private void UpdateCategory(object obj)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, ActiveCategory.Id);
            var options = new ReplaceOptions { IsUpsert = false };

            _quizDbs.Categories.ReplaceOne(filter, ActiveCategory, options);
        }

        private void SaveCategory(object obj)
        {
            var categoryToSave = ActiveCategory;
            _quizDbs.Categories.InsertOne(categoryToSave);
            
        }

        private void AddNewCategory(object obj)
        {

            AddingCategory("New Category");

            AddNewCategoryCommand.RaiseCanExecuteChanged();
        }

        public void AddingCategory(string name)
        {
            var newCategory = new Category(name);
            Categories.Add(newCategory);

            RaisePropertyChanged(nameof(Categories));
        }



        private void RemoveCategory(object obj)
        {
            if (ActiveCategory != null)
            {
                Categories.Remove(ActiveCategory);

                _quizDbs.Categories.DeleteOne(c => c.Id == ActiveCategory.Id);
                RemoveCategoryCommand.RaiseCanExecuteChanged();
            }
            else { return; }
        }

        private void RemovePack(object obj)
        {
            if (ActivePack != null)
            {
                mainWindowViewModel?.Packs.Remove(ActivePack);

                _quizDbs.QuestionPacks.DeleteOne(q => q.Name == ActivePack.Name);

                RemovePackCommand.RaiseCanExecuteChanged();
            }
            else { return; }
        }

        private bool CanRemoveQuestion(object? arg) { return true; }
        private void RemoveQuestion(object obj)
        {
            if (ActiveQuestion != null)
            {
                var collection = _quizDbs.QuestionPacks;
                var filter = Builders<QuestionPack>.Filter.Eq(qp => qp.Id, ActivePack.Id);
                var questionFilter = Builders<Question>.Filter.Eq(q => q.Id, ActiveQuestion.Id);
                var update = Builders<QuestionPack>.Update.PullFilter("Questions", questionFilter);

                collection.UpdateOne(filter, update);

                ActivePack?.Questions.Remove(ActiveQuestion);

                RemoveQuestionCommand.RaiseCanExecuteChanged();
            }
            else { return; }
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

            var collection = _quizDbs.QuestionPacks;
            var filter = Builders<QuestionPack>.Filter.Eq(qp => qp.Id, ActivePack.Id);
            var update = Builders<QuestionPack>.Update.Push(gp => gp.Questions, ActiveQuestion);

            collection.UpdateOne(filter, update);

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
