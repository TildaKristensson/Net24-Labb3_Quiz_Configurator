using Net24_Labb3.Command;
using Net24_Labb3.Dialogs;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net24_Labb3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public PlayViewModel PlayViewModel { get; }

        public ConfigurationViewModel ConfigurationViewModel { get; }

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

        public QuestionPackViewModel MorePacks { get; private set; }

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayViewModel = new PlayViewModel(this);

            Packs = new ObservableCollection<QuestionPackViewModel>();

            SetActivePackCommand = new DelegateCommand(SetActivePack, CanSetActivePack);

            NewPack = new QuestionPack("NewPack");

            ActivePack = new QuestionPackViewModel(new QuestionPack("ITHS Question Pack"));
            //ActivePack.Questions.Add(new Question("Vilken färg är ITHS ankan?", "Lila", "Grön", "Blå", "Rosa"));
            //ActivePack.Questions.Add(new Question("Vad kostar en ITHS-hoodie?", "275kr", "329kr", "249kr", "315kr"));

            ConfigurationViewModel.ActiveQuestion = ActivePack.Questions.FirstOrDefault();

            CreatePackCommand = new DelegateCommand(CreatePack, CanCreatePack);
        }

        private void SetActivePack(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;
            //if (obj is QuestionPackViewModel activePack)
            //{
            //    ActivePack = activePack;
            //}

            RaisePropertyChanged(nameof(ActivePack));
        }

        private bool CanSetActivePack(object? arg) { return true; }

        private void CreatePack(object obj)
        {

            Packs.Add(new QuestionPackViewModel(new QuestionPack(NewPack.Name)));

            CreatePackCommand.RaiseCanExecuteChanged();
        }

        private bool CanCreatePack(object? arg) { return true; }
    }
}
