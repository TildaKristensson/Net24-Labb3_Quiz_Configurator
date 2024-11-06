using Net24_Labb3.Command;
using Net24_Labb3.Model;
using Net24_Labb3.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;

namespace Net24_Labb3.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private DispatcherTimer dispatcherTimer;
        private TimeSpan time;
        private QuestionPackViewModel _playingPack;

        private Question _playingQuestion;

        public Question PlayingQuestion 
        { get => _playingQuestion;
            set
            {
                _playingQuestion = value;
                RaisePropertyChanged();
            }
        }

        private string _timeText;
        public string TimeText
        {
            get => _timeText;
            set
            {
                _timeText = value;
                RaisePropertyChanged(nameof(TimeText));
                RaisePropertyChanged();
            }
        }


        public void StartQuiz(QuestionPackViewModel ActivePack)
        {
            // kalla på enskilda metoder(eller ICommands?) för fråga, nummer fråga och svarsalternativ?
            // Shuffle pack för ordning?

            _playingPack = ActivePack;
            _playingPack.Questions.Shuffle();

            if (_playingPack.Questions.Count > 0)
            {
                PlayingQuestion = _playingPack.Questions[0];
                ShuffleAnswer(PlayingQuestion);
            }
            else
            {
                MessageBox.Show("Det finns inga frågor i det valda frågepaketet.", "Ingen fråga tillgänglig", MessageBoxButton.OK, MessageBoxImage.Information);
                
                mainWindowViewModel.IsPlayMode = false;
                mainWindowViewModel.ShowPackQuestions = true;
            }

            time = TimeSpan.FromSeconds(_playingPack.TimeLimitInSeconds);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

       

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            
        }


        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (time == TimeSpan.Zero) dispatcherTimer.Stop();
            else
            {
                time = time.Add(TimeSpan.FromSeconds(-1));
                TimeText = time.ToString("ss");
            }
        }


        public void ShuffleAnswer(Question ActiveQuestion)
        {
            PlayingQuestion.CorrectAnswer = ActiveQuestion.CorrectAnswer;
            PlayingQuestion.IncorrectAnswers = ActiveQuestion.IncorrectAnswers;

        }


    }
}
