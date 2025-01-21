using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net24_Labb3.Model
{
    public enum Difficulty { Easy, Medium, Hard }
   
    public class QuestionPack
    {
        [JsonConstructor]

        public QuestionPack(string name, Category category = null, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Category = category;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Question>();
        }

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Difficulty Difficulty { get; set; }

        public int TimeLimitInSeconds { get; set; }

        public List<Question> Questions { get; set; }

        public Category Category { get; set; }
    }
}
