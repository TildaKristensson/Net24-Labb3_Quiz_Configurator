using Net24_Labb3.Model;
using Net24_Labb3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net24_Labb3.FileHandlers
{
    class JsonFileHandler
    {
        private readonly string _filePath;

        public JsonFileHandler()
        {
            _filePath = Directory.GetCurrentDirectory() + "\\Packs.txt";
            //File.SetAttributes(filePath, FileAttributes.Normal);
            EnsureDirectoryAndFileExists();
        }

        //för att lägga till eller uppdatera ett questionPack
        public async Task AddOrUpdateQuestionPack(QuestionPackViewModel questionPack)
        {
            var fileContent = File.ReadAllText(_filePath);
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent);

            var packExistsAtIndex = -1;

            for (int i = 0; i < questionPacks.Count; i++)
            {
                if (questionPacks[i].Name.Equals(questionPack.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    packExistsAtIndex = i;
                }
            }

            if (packExistsAtIndex >= 0)
            {
                questionPacks[packExistsAtIndex] = questionPack;
            }
            else
            {
                questionPacks.Add(questionPack);
            }

            var newFileCOntent = JsonSerializer.Serialize(questionPacks);
            File.WriteAllText(_filePath, newFileCOntent);
        }

        // för att ta bort packs
        public async Task RemoveQuestionPackByName(string packName)
        {
            var fileContent = await File.ReadAllTextAsync(_filePath);
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent);

            var packToRemove = questionPacks.FirstOrDefault(p => p.Name.Equals(packName, StringComparison.CurrentCultureIgnoreCase));

            if (packToRemove != null)
            {
                questionPacks.Remove(packToRemove);

                var updateContent = JsonSerializer.Serialize(questionPacks);
                await File.WriteAllTextAsync(_filePath, updateContent);
            }
        }

        //för att hämta en enskild questionpack "by name"
        public async Task<QuestionPackViewModel> GetQuestionPackByName(string packName)
        {
            var fileContent = File.ReadAllText(_filePath);
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent);

            foreach (var questionPack in questionPacks)
            {
                if(questionPack.Name.Equals(packName, StringComparison.InvariantCultureIgnoreCase))
                    return questionPack;
            }

            return null;
        }

        //Hämtar alla questionPacks
        public async Task<ObservableCollection<QuestionPackViewModel>> GetQuestionPacksFromFile(string packName)
        {
            var fileContent = File.ReadAllText(_filePath);
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent);

            return questionPacks;
        }

        public void EnsureDirectoryAndFileExists()
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
                {
                    var emptyPacks = new ObservableCollection<QuestionPackViewModel>();
                    string jsonContent = JsonSerializer.Serialize(emptyPacks);

                    File.WriteAllText(_filePath, jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to ensure directory and file exists: {ex.Message}");
                throw;
            }
        }
    }
}
