using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Net24_Labb3.Model;
using Net24_Labb3.ViewModel;
using ZstdSharp.Unsafe;

namespace Net24_Labb3.Data
{
    public class QuizDbs
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<Category> Categories { get; set; }

        public IMongoCollection<QuestionPack> QuestionPacks { get; set; }

        public QuizDbs()
        {
            var client = new MongoClient("mongodb://localhost:27017/");

            _database = client.GetDatabase("TildaKristensson");

            Categories = _database.GetCollection<Category>("Categories");
            QuestionPacks = _database.GetCollection<QuestionPack>("QuestionPacks");
        }

    }
}
