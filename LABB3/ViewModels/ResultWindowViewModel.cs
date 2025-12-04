using LABB3.Command;
using LABB3.Views;
using System.Windows.Input;
using System.Windows;
using System.Linq;

namespace LABB3.ViewModels;

internal class ResultWindowViewModel : ViewModelBase
{
    private int _correctAnswers;
    private int _totalQuestions;
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public int CorrectAnswers
    {
        get => _correctAnswers;
        set
        {
            if (_correctAnswers != value)
            {
                _correctAnswers = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ResultText));
            }
        }
    }

    public int TotalQuestions
    {
        get => _totalQuestions;
        set
        {
            if (_totalQuestions != value)
            {
                _totalQuestions = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ResultText));
            }
        }
    }

    public string ResultText => $"{CorrectAnswers}/{TotalQuestions} Correct!";
    public ICommand PlayAgainCommand { get; }
    public ICommand MenuCommand { get; }

    public ResultWindowViewModel(int correctCount, int totalQuestions, MainWindowViewModel? mainWindowViewModel = null)
    {
        CorrectAnswers = correctCount;
        TotalQuestions = totalQuestions;
        _mainWindowViewModel = mainWindowViewModel;
        PlayAgainCommand = new DelegateCommand(OnPlayAgain);
        MenuCommand = new DelegateCommand(OnMenu);
    }

    private void OnPlayAgain(object? obj)
    {
        _mainWindowViewModel?.SetCurrentView("Menu");
        CloseDialog();
    }

    private void OnMenu(object? obj)
    {
        _mainWindowViewModel?.SetCurrentView("Menu");
        CloseDialog();
    }

    private void CloseDialog()
    {
        System.Windows.Application.Current.Windows.OfType<ResultView>().FirstOrDefault()?.Close();
    }
}