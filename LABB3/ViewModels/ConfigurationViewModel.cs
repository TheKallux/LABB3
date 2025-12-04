using LABB3.Command;
using LABB3.Models;
using LABB3.Dialogs;
using System.Windows;

namespace LABB3.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    private Question activeQuestion;
    private QuestionPackViewModel? _activePack;

    public DelegateCommand BackCommand { get; }
    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }
    public DelegateCommand EditPackOptionsCommand { get; }
    public DelegateCommand DeletePackCommand { get; }

    public QuestionPackViewModel? ActivePack
    {
        get => _activePack;
        set
        {
            if (_activePack != value)
            {
                _activePack = value;
                RaisePropertyChanged();
                ActiveQuestion = null;
            }
        }
    }

    public Question ActiveQuestion
    {
        get => activeQuestion;
        set
        {
            activeQuestion = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(CanEditQuestion));
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
    }

    public bool CanEditQuestion => ActiveQuestion != null;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        if (mainWindowViewModel != null)
        {
            mainWindowViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.ActivePack))
                {
                    ActivePack = mainWindowViewModel.ActivePack;
                }
            };
            ActivePack = mainWindowViewModel.ActivePack;
        }
        AddQuestionCommand = new DelegateCommand(AddQuestion);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
        EditPackOptionsCommand = new DelegateCommand(EditPackOptions);
        BackCommand = new DelegateCommand(OnBack);
        DeletePackCommand = new DelegateCommand(DeleteCurrentPack);
    }

    private void AddQuestion(object? obj)
    {
        if (ActivePack == null)
        {
            MessageBox.Show("Select a Question Pack first!");
            return;
        }

        Question newQuestion = new Question("Question", "Correct answer", "Incorrect Answer 1", "Incorrect answer 2", "Incorrect answer 3");
        ActivePack.Questions.Add(newQuestion);
        ActiveQuestion = newQuestion;
    }

    private void RemoveQuestion(object? obj)
    {
        if (ActiveQuestion != null && ActivePack != null)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            ActiveQuestion = null;
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanRemoveQuestion(object? obj)
    {
        return ActiveQuestion != null;
    }

    private void EditPackOptions(object? obj)
    {
        if (ActivePack == null)
        {
            MessageBox.Show("Select a Question Pack first!");
            return;
        }

        var dialog = new PackOptionsDialog(
            ActivePack.Name,
            ActivePack.Difficulty.ToString(),
            ActivePack.TimeLimitInSeconds);
        if (dialog.ShowDialog() == true)
        {
            ActivePack.Name = dialog.PackName;
            ActivePack.Difficulty = Enum.Parse<Difficulty>(dialog.SelectedDifficulty);
            ActivePack.TimeLimitInSeconds = dialog.TimePerQuestion;
        }
    }

    private void OnBack(object? obj)
    {
        mainWindowViewModel?.SetCurrentView("Menu");
    }

    private void DeleteCurrentPack(object? obj)
    {
        if (ActivePack != null)
        {
            mainWindowViewModel?.DeletePack(ActivePack);
            mainWindowViewModel?.SetCurrentView("Menu");
        }
    }
}