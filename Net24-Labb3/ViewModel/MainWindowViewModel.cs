using Net24_Labb3.Command;
using Net24_Labb3.Dialogs;
using Net24_Labb3.FileHandlers;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net24_Labb3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public PlayViewModel PlayViewModel { get; }

        private JsonFileHandler _jsonFileHandler;

        public ConfigurationViewModel ConfigurationViewModel { get; }

        //public bool VisibleWindow { get; set; }

        private bool _showPackQuestions;
        private bool _isPlayMode;
        private bool _isConfigurationMode;

        //public DelegateCommand ShowConfigurationViewCommand;

       
        public DelegateCommand CreatePackCommand { get; }

        public DelegateCommand SetActivePackCommand { get; }

        private QuestionPackViewModel _activePack;

        private QuestionPack _newPack;

		public QuestionPackViewModel ActivePack
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

        //public bool IsConfigurationMode 
        //{ 
        //    get { return _isConfigurationMode; }
        //    set
        //    {
        //        _isConfigurationMode = value;
        //        RaisePropertyChanged(nameof(IsConfigurationMode));
        //    }
        //}

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


        public DelegateCommand ShowPlayViewCommand { get; }
        public DelegateCommand EndPlayViewCommand { get; }

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayViewModel = new PlayViewModel(this);

            //ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
            //ShowPlayViewCommand = new DelegateCommand(ShowPlayMode);
            _jsonFileHandler = new JsonFileHandler();
            //_jsonFileHandler.AddOrUpdateQuestionPack(new QuestionPack("hej", Difficulty.Medium, 30)
            //{

            //}).GetAwaiter().GetResult();
            ShowPlayViewCommand = new DelegateCommand(_ => StartPlay());
            EndPlayViewCommand = new DelegateCommand(_ => EndPlay());

            ShowPackQuestions = true;
            IsPlayMode = false;
            IsConfigurationMode = false;

            Packs = new ObservableCollection<QuestionPackViewModel>();

            SetActivePackCommand = new DelegateCommand(SetActivePack, CanSetActivePack);

            NewPack = new QuestionPack("NewPack");

            ActivePack = new QuestionPackViewModel(new QuestionPack("ITHS Question Pack"));
            //ActivePack.Questions.Add(new Question("Vilken färg är ITHS ankan?", "Lila", "Grön", "Blå", "Rosa"));
            //ActivePack.Questions.Add(new Question("Vad kostar en ITHS-hoodie?", "275kr", "329kr", "249kr", "315kr"));

            ConfigurationViewModel.ActiveQuestion = ActivePack.Questions.FirstOrDefault();

            CreatePackCommand = new DelegateCommand(CreatePack, CanCreatePack);
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
            IsConfigurationMode = true;
            ShowPackQuestions = true;
        }

        

        //private void ShowPlayMode(object obj)
        //{
        //    IsPlayMode = true;
        //}

        //private void ShowConfigurationView(object obj)
        //{
        //    IsPlayMode = false;
        //}

        private void SetActivePack(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;

            //_jsonFileHandler.GetQuestionPacksFromFile("packName").GetAwaiter().GetResult();

            RaisePropertyChanged(nameof(ActivePack));
        }

        private bool CanSetActivePack(object? arg) { return true; }

        private void CreatePack(object obj)
        {
            var questionPack = new QuestionPackViewModel(new QuestionPack(NewPack.Name, NewPack.Difficulty, NewPack.TimeLimitInSeconds));
            Packs.Add(questionPack);

            _jsonFileHandler.AddOrUpdateQuestionPack(questionPack).GetAwaiter().GetResult();

            CreatePackCommand.RaiseCanExecuteChanged();
        }

        private bool CanCreatePack(object? arg) { return true; }
    }
}
