using Net24_Labb3.Command;
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

        private QuestionPackViewModel? _activePack;

		public QuestionPackViewModel? ActivePack
		{
			get => _activePack;
			set 
			{
				_activePack = value;
				RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged("ActivePack");
			}
		}

        public MainWindowViewModel()
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayViewModel = new PlayViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("ITHS Question Pack"));
            ActivePack.Questions.Add(new Question("Vilken färg är ITHS ankan?", "Lila", "Grön", "Blå", "Rosa"));
            ActivePack.Questions.Add(new Question("Vad kostar en ITHS-hoodie?", "275kr", "329kr", "249kr", "315kr"));

            ConfigurationViewModel.ActiveQuestion = ActivePack.Questions.FirstOrDefault();
        }
    }
}
