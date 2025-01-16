using Net24_Labb3.Command;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net24_Labb3.ViewModel
{
    class QuestionPackViewModel : ViewModelBase
    {
        private readonly Model.QuestionPack model;

        [JsonConstructor]
        public QuestionPackViewModel(Model.QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);
            this.Categories = new ObservableCollection<Category>(model.Categories);
        }

        public string Name 
        { get => model.Name; 
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }

        //public string Category
        //{
        //    get => model.Category;
        //    set
        //    {
        //        model.Category = value;
        //        RaisePropertyChanged();
        //    }
        //}


       
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }

        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<Question> Questions { get; }
        public ObservableCollection<Category> Categories { get; }
       
    }
}
