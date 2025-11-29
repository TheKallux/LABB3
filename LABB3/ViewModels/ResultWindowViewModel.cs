using LABB3.Command;
using LABB3.Views;
using System.Windows.Input;

namespace LABB3.ViewModels;

internal class ResultWindowViewModel : ViewModelBase
{
    private int _correctAnswers;
    private int _totalQuestions;

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

    public ResultWindowViewModel()
    {
        PlayAgainCommand = new DelegateCommand(OnPlayAgain);
        MenuCommand = new DelegateCommand(OnMenu);
    }

    private void OnPlayAgain(object? obj)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();

        System.Windows.Application.Current.Windows.OfType<ResultView>().FirstOrDefault()?.Close();
    }

    private void OnMenu(object? obj)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();

        System.Windows.Application.Current.Windows.OfType<ResultView>().FirstOrDefault()?.Close();
    }
}