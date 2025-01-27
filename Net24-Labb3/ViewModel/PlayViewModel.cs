﻿using Net24_Labb3.Command;
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
using System.Windows.Media;
using static System.Formats.Asn1.AsnWriter;

namespace Net24_Labb3.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand ValidateAnswerCommand { get; }
        public DelegateCommand MoveToNextQuestionCommand { get; }

        private DispatcherTimer dispatcherTimer;
        private TimeSpan time;
        private QuestionPack _playingPack;

        private Question _playingQuestion;

        private bool _nextQuestionAvailable;
        public bool NextQuestionAvailable
        {
            get => _nextQuestionAvailable;
            set
            {
                _nextQuestionAvailable = value;
                RaisePropertyChanged(nameof(NextQuestionAvailable));
                RaisePropertyChanged();
            }
        }

        public bool IsPlayMode
        {
            get { return mainWindowViewModel.IsPlayMode; }
        }

        private Brush _answerOptionOneBackgroundColor = Brushes.Ivory;
        public Brush AnswerOptionOneBackgroundColor
        {
            get => _answerOptionOneBackgroundColor;
            set
            {
                _answerOptionOneBackgroundColor = value;
                RaisePropertyChanged(nameof(AnswerOptionOneBackgroundColor));
                RaisePropertyChanged();
            }
        }

        private Brush _answerOptionTwoBackgroundColor = Brushes.Ivory;
        public Brush AnswerOptionTwoBackgroundColor
        {
            get => _answerOptionTwoBackgroundColor;
            set
            {
                _answerOptionTwoBackgroundColor = value;
                RaisePropertyChanged(nameof(AnswerOptionTwoBackgroundColor));
                RaisePropertyChanged();
            }
        }

        private Brush _answerOptionThreeBackgroundColor = Brushes.Ivory;
        public Brush AnswerOptionThreeBackgroundColor
        {
            get => _answerOptionThreeBackgroundColor;
            set
            {
                _answerOptionThreeBackgroundColor = value;
                RaisePropertyChanged(nameof(AnswerOptionThreeBackgroundColor));
                RaisePropertyChanged();
            }
        }

        private Brush _answerOptionFourBackgroundColor = Brushes.Ivory;
        public Brush AnswerOptionFourBackgroundColor
        {
            get => _answerOptionFourBackgroundColor;
            set
            {
                _answerOptionFourBackgroundColor = value;
                RaisePropertyChanged(nameof(AnswerOptionFourBackgroundColor));
                RaisePropertyChanged();
            }
        }

        public Question PlayingQuestion 
        { get => _playingQuestion;
            set
            {
                _playingQuestion = value;
                RaisePropertyChanged();
            }
        }

        private string _timeText;
        private List<Answer> _answers;

        public List<Answer> Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                RaisePropertyChanged(nameof(Answers));
                RaisePropertyChanged();
            }
        }

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

        public int TotalQuestions { get; private set; }
        public int CurrentQuestion { get; private set; }
        public int Score { get; private set; }

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            ValidateAnswerCommand = new DelegateCommand(ValidateAnswer);

            MoveToNextQuestionCommand = new DelegateCommand(MoveToNextQuestion);
        }

        private string _questionCounterText;
        public string QuestionCounterText
        {
            get => $"Question {CurrentQuestion} of {TotalQuestions}";
            set
            {
                RaisePropertyChanged(nameof(QuestionCounterText));
                RaisePropertyChanged();
            }
        }

        private string _resultText;
        public string ResultText
        {
            get => $"You answered {Score} of {TotalQuestions} questions correct! :)";
            set
            {
                RaisePropertyChanged(nameof(_resultText));
                RaisePropertyChanged();
            }
        }

        private void MoveToNextQuestion(object obj)
        {
            if(_playingPack.Questions.Count == CurrentQuestion)
            {
                mainWindowViewModel.IsResultMode = true;               
                mainWindowViewModel.IsPlayMode = false;
                NextQuestionAvailable = false;
                ResetAllAnswerColors();

                return;
            }

            PlayingQuestion = _playingPack.Questions[CurrentQuestion];
            CurrentQuestion++;

            ShuffleAnswer(PlayingQuestion);

            RaisePropertyChanged(nameof(QuestionCounterText));
            RaisePropertyChanged(nameof(PlayingQuestion));
            RaisePropertyChanged(nameof(AnswerOptionOneBackgroundColor));
            RaisePropertyChanged(nameof(AnswerOptionTwoBackgroundColor));
            RaisePropertyChanged(nameof(AnswerOptionThreeBackgroundColor));
            RaisePropertyChanged(nameof(AnswerOptionFourBackgroundColor));
            NextQuestionAvailable = false;

            time = TimeSpan.FromSeconds(_playingPack.TimeLimitInSeconds);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            RaisePropertyChanged(nameof(QuestionCounterText));

            ResetAllAnswerColors();
        }

        private void ValidateAnswer(object obj)
        {
            var answer = (Answer)obj;

            if (answer.IsCorrectAnswer)
            {
                SetCorrectAnswerColor(answer);
                Score++;                
            }
            else
            {
                var correctAnswer = Answers.Find(answer => answer.IsCorrectAnswer);
                SetCorrectAnswerColor(correctAnswer);
                SetIncorrectAnswerColor(answer);
            }

            RaisePropertyChanged(nameof(ResultText));
            NextQuestionAvailable = true;
        }

        private void ResetAllAnswerColors()
        {
            AnswerOptionOneBackgroundColor = Brushes.Ivory;
            AnswerOptionTwoBackgroundColor = Brushes.Ivory;
            AnswerOptionThreeBackgroundColor = Brushes.Ivory;
            AnswerOptionFourBackgroundColor = Brushes.Ivory;
        }

        private void SetIncorrectAnswerColor(Answer answer)
        {
            if (answer.AnswerOrderId == 1)
                AnswerOptionOneBackgroundColor = Brushes.OrangeRed;

            if (answer.AnswerOrderId == 2)
                AnswerOptionTwoBackgroundColor = Brushes.OrangeRed;

            if (answer.AnswerOrderId == 3)
                AnswerOptionThreeBackgroundColor = Brushes.OrangeRed;

            if (answer.AnswerOrderId == 4)
                AnswerOptionFourBackgroundColor = Brushes.OrangeRed;
        }

        private void SetCorrectAnswerColor(Answer answer)
        {
            if (answer.AnswerOrderId == 1)
                AnswerOptionOneBackgroundColor = Brushes.SpringGreen;

            if (answer.AnswerOrderId == 2)
                AnswerOptionTwoBackgroundColor = Brushes.SpringGreen;

            if (answer.AnswerOrderId == 3)
                AnswerOptionThreeBackgroundColor = Brushes.SpringGreen;

            if (answer.AnswerOrderId == 4)
                AnswerOptionFourBackgroundColor = Brushes.SpringGreen;
        }

        public void StartQuiz(QuestionPack ActivePack)
        {

            _playingPack = ActivePack;
            _playingPack.Questions.Shuffle();

            if (_playingPack.Questions.Count > 0)
            {
                TotalQuestions = _playingPack.Questions.Count;
                CurrentQuestion = 1;
                Score = 0;
                PlayingQuestion = _playingPack.Questions[0];
                ShuffleAnswer(PlayingQuestion);
            }
            else
            {
                MessageBox.Show("There are no questions in the selected question pack.", "No question available", MessageBoxButton.OK, MessageBoxImage.Information);
                
                mainWindowViewModel.IsPlayMode = false;
                mainWindowViewModel.ShowPackQuestions = true;
            }

            time = TimeSpan.FromSeconds(_playingPack.TimeLimitInSeconds);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            RaisePropertyChanged(nameof(QuestionCounterText));

        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (time == TimeSpan.Zero) 
            {
                HandleTimeHasRunOut();
            }
            else if (mainWindowViewModel?.IsPlayMode == false)
            {
                dispatcherTimer.Stop();
            }
            else
            {
                time = time.Add(TimeSpan.FromSeconds(-1));
                TimeText = time.ToString("ss");
            }
        }

        private void HandleTimeHasRunOut()
        {
            dispatcherTimer.Stop();
            MessageBox.Show("Your time is up! Go to Next Question", "Time's Up!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            NextQuestionAvailable = true;

        }

        public void ShuffleAnswer(Question ActiveQuestion)
        {
            var answers = new List<Answer>() { 
                new Answer { 
                    IsCorrectAnswer = true, 
                    AnswerOption = ActiveQuestion.CorrectAnswer
                } 
            };

            foreach (var answer in ActiveQuestion.IncorrectAnswers)
            {
                answers.Add(new Answer 
                { 
                    AnswerOption = answer,
                    IsCorrectAnswer = false
                });
            }

            answers.Shuffle();

            for (int i = 0; i < answers.Count; i++) 
            {
                answers[i].AnswerOrderId = i + 1;
            }

            Answers = answers;
        }
    }
}
