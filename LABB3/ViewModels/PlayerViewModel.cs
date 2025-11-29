using LABB3.Command;
using LABB3.Models;
using LABB3.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LABB3.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    public int TotalQuestions => ActivePack?.Questions.Count ?? 0;
    public string QuestionProgress => $"Question {CurrentQuestionIndex}/{TotalQuestions}";

    public string? Answer0 => CurrentAnswers.Count > 0 ? CurrentAnswers[0] : null;
    public string? Answer1 => CurrentAnswers.Count > 1 ? CurrentAnswers[1] : null;
    public string? Answer2 => CurrentAnswers.Count > 2 ? CurrentAnswers[2] : null;
    public string? Answer3 => CurrentAnswers.Count > 3 ? CurrentAnswers[3] : null;

    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

    public ObservableCollection<string> CurrentAnswers { get; set; }
    public Question? CurrentQuestion { get; set; }

    private int _currentQuestionIndex;

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        set
        {
            _currentQuestionIndex = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(QuestionProgress));
        }
    }

    private int _correctCount;

    public int CorrectCount
    {
        get => _correctCount;
        set
        {
            _correctCount = value;
            RaisePropertyChanged();
        }
    }

    public DelegateCommand AnswerQuestionCommand { get; set; }

    private string? _selectedAnswer;

    public string? SelectedAnswer
    {
        get => _selectedAnswer;
        set
        {
            _selectedAnswer = value;
            RaisePropertyChanged();
        }
    }

    private string? _correctAnswerButton;

    public string? CorrectAnswerButton
    {
        get => _correctAnswerButton;
        set
        {
            _correctAnswerButton = value;
            RaisePropertyChanged();
            RaiseIconProperties();
        }
    }

    private string? _wrongAnswerButton;

    public string? WrongAnswerButton
    {
        get => _wrongAnswerButton;
        set
        {
            _wrongAnswerButton = value;
            RaisePropertyChanged();
            RaiseIconProperties();
        }
    }

    private bool _canAnswer = true;

    public bool CanAnswer
    {
        get => _canAnswer;
        set
        {
            _canAnswer = value;
            RaisePropertyChanged();
        }
    }

    public string Answer0Icon => GetIcon(Answer0);
    public string Answer1Icon => GetIcon(Answer1);
    public string Answer2Icon => GetIcon(Answer2);
    public string Answer3Icon => GetIcon(Answer3);

    public Brush Answer0IconColor => GetIconColor(Answer0);
    public Brush Answer1IconColor => GetIconColor(Answer1);
    public Brush Answer2IconColor => GetIconColor(Answer2);
    public Brush Answer3IconColor => GetIconColor(Answer3);

    private string GetIcon(string? answer)
    {
        if (answer == null) return "";
        if (answer == CorrectAnswerButton) return "✔";
        if (answer == WrongAnswerButton) return "✖";
        return "";
    }

    private Brush GetIconColor(string? answer)
    {
        if (answer == CorrectAnswerButton) return Brushes.Green;
        if (answer == WrongAnswerButton) return Brushes.Red;
        return Brushes.Transparent;
    }

    private void RaiseIconProperties()
    {
        RaisePropertyChanged(nameof(Answer0Icon));
        RaisePropertyChanged(nameof(Answer1Icon));
        RaisePropertyChanged(nameof(Answer2Icon));
        RaisePropertyChanged(nameof(Answer3Icon));

        RaisePropertyChanged(nameof(Answer0IconColor));
        RaisePropertyChanged(nameof(Answer1IconColor));
        RaisePropertyChanged(nameof(Answer2IconColor));
        RaisePropertyChanged(nameof(Answer3IconColor));
    }

    private int _timeRemaining;

    public int TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            _timeRemaining = value;
            RaisePropertyChanged();
        }
    }

    private DispatcherTimer? _timer;

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        CurrentAnswers = new ObservableCollection<string>();
        CurrentQuestionIndex = 0;
        CorrectCount = 0;
        AnswerQuestionCommand = new DelegateCommand(AnswerQuestion);
    }

    public void LoadNextQuestion()
    {
        CanAnswer = true;
        CorrectAnswerButton = null;
        WrongAnswerButton = null;
        SelectedAnswer = null;

        _timer?.Stop();
        TimeRemaining = 0;

        if (ActivePack?.Questions.Count > CurrentQuestionIndex)
        {
            CurrentQuestion = ActivePack.Questions[CurrentQuestionIndex];
            RaisePropertyChanged(nameof(CurrentQuestion));
            ShuffleAnswers();
            StartTimer();
            CurrentQuestionIndex++;
        }
        else
        {
            ShowResults();
        }
    }

    private void ShowResults()
    {
        ResultView resultWindow = new ResultView(CorrectCount, TotalQuestions);
        resultWindow.Show();
        Application.Current.MainWindow?.Close();
    }

    private void ShuffleAnswers()
    {
        CurrentAnswers.Clear();

        var answers = new List<string>
        {
            CurrentQuestion.CorrectAnswer,
            CurrentQuestion.IncorrectAnswers[0],
            CurrentQuestion.IncorrectAnswers[1],
            CurrentQuestion.IncorrectAnswers[2]
        };

        var random = new Random();
        foreach (var ans in answers.OrderBy(x => random.Next()))
            CurrentAnswers.Add(ans);

        RaisePropertyChanged(nameof(Answer0));
        RaisePropertyChanged(nameof(Answer1));
        RaisePropertyChanged(nameof(Answer2));
        RaisePropertyChanged(nameof(Answer3));
        RaiseIconProperties();
    }

    private void StartTimer()
    {
        TimeRemaining = ActivePack.TimeLimitInSeconds;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimeRemaining--;
    }

    private async void AnswerQuestion(object? parameter)
    {
        CanAnswer = false;

        var answers = new[] { Answer0, Answer1, Answer2, Answer3 };
        int buttonIndex = int.Parse(parameter.ToString());
        string selectedAnswer = answers[buttonIndex];
        SelectedAnswer = selectedAnswer;

        if (selectedAnswer == CurrentQuestion.CorrectAnswer)
        {
            CorrectAnswerButton = selectedAnswer;
            WrongAnswerButton = null;
            CorrectCount++;
        }
        else
        {
            WrongAnswerButton = selectedAnswer;
            CorrectAnswerButton = CurrentQuestion.CorrectAnswer;
        }

        _timer?.Stop();

        await Task.Delay(3000);

        LoadNextQuestion();
    }
}