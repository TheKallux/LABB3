using LABB3.Command;
using LABB3.Models;
using LABB3.Dialogs;

namespace LABB3.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    private Question activeQuestion;

    public DelegateCommand BackCommand { get; }
    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }
    public DelegateCommand EditPackOptionsCommand { get; }

    public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

    public Question ActiveQuestion
    {
        get => activeQuestion;
        set
        {
            activeQuestion = value;
            RaisePropertyChanged();
            RemoveQuestionCommand.RaiseCanExecuteChanged();
        }
    }

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        AddQuestionCommand = new DelegateCommand(AddQuestion);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
        EditPackOptionsCommand = new DelegateCommand(EditPackOptions);
        BackCommand = new DelegateCommand(OnBack);
    }

    private void AddQuestion(object? obj)
    {
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
}