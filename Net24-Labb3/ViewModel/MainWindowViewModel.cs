using MongoDB.Driver;
using Net24_Labb3.Command;
using Net24_Labb3.Data;
using Net24_Labb3.Dialogs;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Net24_Labb3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPack> Packs { get; set; }

        public PlayViewModel PlayViewModel { get; }


        private QuizDbs _quizDbs;

        public ConfigurationViewModel ConfigurationViewModel { get; }


        public DelegateCommand ChooseQuestionCommand { get; }

        private bool _showPackQuestions;
        private bool _isPlayMode;
        private bool _isConfigurationMode;
        private bool _isResultMode;

       public DelegateCommand UpdatePackCommand { get; }
        public DelegateCommand CreatePackCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }


        private QuestionPack _activePack;

        private QuestionPack _newPack;

 

		public QuestionPack ActivePack
		{
			get => _activePack;
			set 
			{
				_activePack = value;
				RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged();
			}
		}


        public QuestionPack NewPack 
        { 
            get => _newPack;
            set
            {
                _newPack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged("NewPack");
            }
        }

        public bool IsPlayMode 
        { 
            get { return _isPlayMode; }
            set
            {
                _isPlayMode = value;
                RaisePropertyChanged(nameof(IsPlayMode));
                RaisePropertyChanged(nameof(IsConfigurationMode));
            }
        }

        public bool IsConfigurationMode 
        { 
            get { return _isConfigurationMode; }
            set
            {
                _isConfigurationMode = value;
                RaisePropertyChanged(nameof(IsConfigurationMode));
                RaisePropertyChanged(nameof(IsPlayMode));
            } 
        }

        public bool ShowPackQuestions
        {
            get { return _showPackQuestions; }
            set
            {
                _showPackQuestions = value;
                RaisePropertyChanged(nameof(ShowPackQuestions));
            }

        }

        public bool IsResultMode
        {
            get { return _isResultMode; }
            set
            {
                _isResultMode = value;
                RaisePropertyChanged(nameof(IsResultMode));
                RaisePropertyChanged(nameof(IsConfigurationMode));
                RaisePropertyChanged(nameof(IsPlayMode));
            }
        }

        public DelegateCommand ShowPlayViewCommand { get; }
        public DelegateCommand EndPlayViewCommand { get; }
        

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayViewModel = new PlayViewModel(this);

            _quizDbs = new QuizDbs();
     
            ChooseQuestionCommand = new DelegateCommand(ChooseQuestion);

            ShowPlayViewCommand = new DelegateCommand(_ => StartPlay());
            EndPlayViewCommand = new DelegateCommand(_ => EndPlay());

            ShowPackQuestions = true;
            IsPlayMode = false;
            IsConfigurationMode = false;
            IsResultMode = false;

            Packs = new ObservableCollection<QuestionPack>();

            LoadPacksLoadPacksAndCategories();

            SetActivePackCommand = new DelegateCommand(SetActivePack, CanSetActivePack);

            UpdatePackCommand = new DelegateCommand(UpdatePack);

            NewPack = new QuestionPack("NewPack");

            ActivePack = new QuestionPack("Choose or create a Pack!");

           

            ConfigurationViewModel.ActiveQuestion = ActivePack.Questions.FirstOrDefault();

            //ConfigurationViewModel.ActiveCategory = ActivePack.Categories.FirstOrDefault();

            CreatePackCommand = new DelegateCommand(CreatePack, CanCreatePack);
        }

        public void LoadPacksLoadPacksAndCategories()
        {
            var packsInDb = _quizDbs.QuestionPacks.Find(_ => true).ToList();

            var categoriesInDb = _quizDbs.Categories.Find(_ => true).ToList();

            foreach (var pack in packsInDb)
            {
                Packs.Add(pack);
            }

            foreach (var cat in categoriesInDb)
            {
                ConfigurationViewModel.Categories.Add(cat);
            }

            RaisePropertyChanged(nameof(Packs));
            RaisePropertyChanged(nameof(ConfigurationViewModel.Categories));
        }
        private void ChooseQuestion(object obj)
        {
            IsConfigurationMode = true;
        }
        private void UpdatePack(object obj)
        {
            // Implementera denna! 

            UpdatePackCommand.RaiseCanExecuteChanged();
        }

        private void StartPlay()
        {
            IsPlayMode = true;
            IsConfigurationMode = false;
            ShowPackQuestions = false;
            PlayViewModel.StartQuiz(ActivePack);
        }
        private void EndPlay()
        {
            
            IsPlayMode = false;
            IsResultMode = false;
            IsConfigurationMode = true;
            ShowPackQuestions = true;
        }


        private void SetActivePack(object obj)
        {
            ActivePack = (QuestionPack)obj;

            RaisePropertyChanged(nameof(ActivePack));
        }

        private bool CanSetActivePack(object? arg) { return true; }

        private void CreatePack(object obj)
        {
            var questionPack = new QuestionPack(NewPack.Name, NewPack.Category, NewPack.Difficulty, NewPack.TimeLimitInSeconds);
            Packs.Add(questionPack);

            _quizDbs.QuestionPacks.InsertOne(questionPack);

            CreatePackCommand.RaiseCanExecuteChanged();
        }

        private bool CanCreatePack(object? arg) { return true; }
    }
}
