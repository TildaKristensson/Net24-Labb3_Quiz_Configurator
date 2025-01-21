using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
    public class QuestionPackViewModel : ViewModelBase
    {
        private readonly Model.QuestionPack model;

        [JsonConstructor]
        public QuestionPackViewModel(Model.QuestionPack model)
        {
            
            this.model = model;
            Category = model.Category;
            this.Questions = new ObservableCollection<Question>(model.Questions);
            
        }

        
        public ObjectId Id
        {
            get => model.Id;
            set
            {
                model.Id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }

        public Category Category { get; set; }


       
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
        
       
    }
}
