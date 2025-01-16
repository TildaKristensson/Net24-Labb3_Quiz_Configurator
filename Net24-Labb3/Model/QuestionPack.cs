using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net24_Labb3.Model
{
    enum Difficulty { Easy, Medium, Hard }
   
    internal class QuestionPack
    {
        [JsonConstructor]

        public QuestionPack(string name,Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Categories = new List<Category>();
            Questions = new List<Question>();
        }

        public string Name { get; set; }

        public Difficulty Difficulty { get; set; }

        public int TimeLimitInSeconds { get; set; }

        public List<Question> Questions { get; set; }

        public List<Category> Categories { get; set; }
    }
}
