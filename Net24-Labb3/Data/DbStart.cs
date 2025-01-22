using MongoDB.Driver;
using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net24_Labb3.Data
{
    public static class DbStart
    {
        public static void CategoryInitializer()
        {
            var db = new QuizDbs();

            var count = db.Categories.CountDocuments(FilterDefinition<Category>.Empty);

            if (count == 0)
            {
                var defaultCategories = new ObservableCollection<Category>
                {
                    new Category("Science"),
                    new Category("Random Trivia"),
                };
                
                db.Categories.InsertMany(defaultCategories);
                
            }

        }

        public static QuestionPack QuestionPackInitializer()
        {
            var db = new QuizDbs();

            var packCount = db.QuestionPacks.CountDocuments(FilterDefinition<QuestionPack>.Empty);
            if(packCount == 0)
            {

                var categoryName = "Remember the names";
                var existingCategory = db.Categories.Find(c => c.Name == categoryName).FirstOrDefault();
               
                var category = existingCategory ?? new Category(categoryName);

                if (existingCategory == null)
                {
                    db.Categories.InsertOne(category);
                }

                var defaultPack = new QuestionPack
                {
                    Name = "Who was it?",
                    Difficulty = Difficulty.Medium,
                    TimeLimitInSeconds = 30,
                    Category = category,
                    Questions = new ObservableCollection<Question>
                    {
                        new Question
                        {
                            Query = "Who created this app?",
                            CorrectAnswer = "Tilda",
                            IncorrectAnswers = new[] {"Hilda", "Matilda", "Tilde"}
                        },
                        new Question
                        {
                            Query = "Who invented the pacemaker?",
                            CorrectAnswer = "Rune Elmqvist",
                            IncorrectAnswers = new[] {"Earl Bakken", "John Hopps", "Åke Senning"}
                        },
                         new Question
                        {
                            Query = "Who painted Mona Lisa?",
                            CorrectAnswer = "Leonardo da Vinci",
                            IncorrectAnswers = new[] {"Leonardo DiCaprio", "Pablo Picasso", "Vincent van Gogh"}
                        }
                    }
                };
                db.QuestionPacks.InsertOne(defaultPack);
                return defaultPack; 
            }
            else
            {
                return db.QuestionPacks.Find(_ => true).FirstOrDefault();
            }
        }
    }
}
