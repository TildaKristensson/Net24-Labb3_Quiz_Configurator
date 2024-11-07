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
        private readonly JsonSerializerOptions _options;

        public JsonFileHandler()
        {
            _filePath = Directory.GetCurrentDirectory() + "\\Packs1.txt";
            EnsureDirectoryAndFileExists();

            _options = new JsonSerializerOptions
            {
                IncludeFields = true, 
                PropertyNameCaseInsensitive = true 
            };
        }

        //för att lägga till eller uppdatera ett questionPack
        public async Task AddOrUpdateQuestionPack(QuestionPackViewModel questionPack)
        {
            //Hämtar det som finns
            var fileContent = await File.ReadAllTextAsync(_filePath);
            var models = JsonSerializer.Deserialize<ObservableCollection<Model.QuestionPack>>(fileContent, _options);
            var viewModels = models.Select(model => new QuestionPackViewModel(model)).ToList();
            var viewModelsObservableCollection = new ObservableCollection<QuestionPackViewModel>(viewModels);

            var packExistsAtIndex = -1;

            //kollar om den vi ska lägga till eller uppdatera redan finns
            for (int i = 0; i < viewModelsObservableCollection.Count; i++)
            {
                if (viewModelsObservableCollection[i].Name.Equals(questionPack.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    packExistsAtIndex = i;
                }
            }

            //om den redan finns så uppdaterar vi den
            if (packExistsAtIndex >= 0)
            {
                viewModelsObservableCollection[packExistsAtIndex] = questionPack;
            }
            else
            {
                //annars lägger vi till den
                viewModelsObservableCollection.Add(questionPack);
            }

            //vi gör om det till string igen
            var newFileCOntent = JsonSerializer.Serialize(viewModelsObservableCollection);
            File.WriteAllText(_filePath, newFileCOntent);
        }

        // för att ta bort packs
        public async Task RemoveQuestionPackByName(string packName)
        {
            var fileContent = await File.ReadAllTextAsync(_filePath);
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent, _options);

            var packToRemove = questionPacks.FirstOrDefault(p => p.Name.Equals(packName, StringComparison.CurrentCultureIgnoreCase));

            if (packToRemove != null)
            {
                questionPacks.Remove(packToRemove);

                var updateContent = JsonSerializer.Serialize(questionPacks);
                await File.WriteAllTextAsync(_filePath, updateContent);
            }
        }

        //för att hämta en enskild questionpack "by name"
        //public async Task<QuestionPackViewModel> GetQuestionPackByName(string packName)
        //{
        //    var fileContent = await File.ReadAllTextAsync(_filePath);
        //    var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(fileContent, _options);

        //    foreach (var questionPack in questionPacks)
        //    {
        //        if(questionPack.Name.Equals(packName, StringComparison.InvariantCultureIgnoreCase))
        //            return questionPack;
        //    }

        //    return null;
        //}

        //Hämtar alla questionPacks
        public async Task<ObservableCollection<QuestionPackViewModel>> GetQuestionPacksFromFile()
        {
            var fileContent = await File.ReadAllTextAsync(_filePath);
            if (fileContent == null || fileContent == string.Empty)
                return null;

            var models = JsonSerializer.Deserialize<ObservableCollection<Model.QuestionPack>>(fileContent, _options);
            var viewModels = models.Select(model => new QuestionPackViewModel(model)).ToList();
            var viewModelsObservableCollection = new ObservableCollection<QuestionPackViewModel>(viewModels);

            return viewModelsObservableCollection;
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
                    string jsonContent = JsonSerializer.Serialize(emptyPacks, _options);

                    File.WriteAllTextAsync(_filePath, jsonContent);
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
