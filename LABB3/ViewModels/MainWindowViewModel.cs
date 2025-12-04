using LABB3.Command;
using LABB3.Dialogs;
using LABB3.Models;
using LABB3.Services;
using LABB3.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace LABB3.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private QuestionPackViewModel _activePack;
    private ObservableCollection<QuestionPackViewModel> _questionPacks;
    private ConfigurationViewModel? _configurationViewModel;
    private PlayerViewModel? _playerViewModel;
    private ObservableCollection<string> _currentAnswers = new();
    private string _currentView = "Menu";
    private readonly QuestionPackService _packService;

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<QuestionPackViewModel> QuestionPacks
    {
        get => _questionPacks;
        set
        {
            _questionPacks = value;
            RaisePropertyChanged();
        }
    }

    public ConfigurationViewModel? ConfigurationViewModel
    {
        get => _configurationViewModel;
        private set
        {
            _configurationViewModel = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<string> CurrentAnswers
    {
        get => _currentAnswers;
        set
        {
            _currentAnswers = value;
            RaisePropertyChanged();
        }
    }

    public string CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            RaisePropertyChanged();
        }
    }

    public DelegateCommand CreateNewPackCommand { get; set; }
    public DelegateCommand EditQuestionPackCommand { get; set; }
    public DelegateCommand ExitCommand { get; set; }
    public DelegateCommand PlayCommand { get; set; }
    public DelegateCommand SelectPackCommand { get; set; }
    public DelegateCommand DeletePackCommand { get; set; }
    public DelegateCommand FullScreenCommand { get; set; }


    public PlayerViewModel? PlayerViewModel
    {
        get => _playerViewModel;
        private set
        {
            _playerViewModel = value;
            RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        _packService = new QuestionPackService();

        SelectPackCommand = new DelegateCommand(OnSelectPack);
        DeletePackCommand = new DelegateCommand(OnDeletePack);
        PlayCommand = new DelegateCommand(OnPlay);
        EditQuestionPackCommand = new DelegateCommand(OnEdit);
        ExitCommand = new DelegateCommand(OnExit);
        CreateNewPackCommand = new DelegateCommand(OnCreateNewPack);
        FullScreenCommand = new DelegateCommand(OnFullscreen);

        QuestionPacks = new ObservableCollection<QuestionPackViewModel>();

        LoadPacksAsync();

        PlayerViewModel = new PlayerViewModel(this);
        ConfigurationViewModel = new ConfigurationViewModel(this);
    }

    private async void LoadPacksAsync()
    {
        var packs = await _packService.LoadPacksAsync();

        if (packs.Count == 0)
        {
            var defaultPack = new QuestionPack("My Question Pack");
            defaultPack.Questions.Add(new Question($"Vad är 1+1", "2", "3", "1", "4"));
            defaultPack.Questions.Add(new Question($"Vad heter sveriges huvudstad?", "Stockholm", "Oslo", "London", "Göteborg"));
            packs.Add(defaultPack);
        }

        foreach (var pack in packs)
        {
            QuestionPacks.Add(new QuestionPackViewModel(pack));
        }

        if (QuestionPacks.Count > 0)
            ActivePack = QuestionPacks[0];
    }

    private async void SavePacksAsync()
    {
        var packs = QuestionPacks.Select(vm => vm.Pack).ToList();
        await _packService.SavePacksAsync(packs);
    }

    private void OnCreateNewPack(object? obj)
    {
        var dialog = new CreateNewPackDialog();
        if (dialog.ShowDialog() == true)
        {
            var newPack = new QuestionPack(dialog.PackName);
            newPack.Difficulty = Enum.Parse<Difficulty>(dialog.SelectedDifficulty);
            var packViewModel = new QuestionPackViewModel(newPack);
            QuestionPacks.Add(packViewModel);
            ActivePack = packViewModel;

            SavePacksAsync();
        }
    }

    private void OnSelectPack(object? obj)
    {
        if (QuestionPacks.Count == 0)
        {
            MessageBox.Show("No packs available!");
            return;
        }

        var dialog = new SelectPackDialog(QuestionPacks.ToList());
        if (dialog.ShowDialog() == true && dialog.SelectedPack != null)
        {
            ActivePack = dialog.SelectedPack;
        }
    }

    private void OnDeletePack(object? obj)
    {
        if (QuestionPacks.Count == 0)
        {
            MessageBox.Show("No packs available!");
            return;
        }

        var dialog = new SelectPackDialog(QuestionPacks.ToList());
        if (dialog.ShowDialog() == true && dialog.SelectedPack != null)
        {
            DeletePack(dialog.SelectedPack);
        }
    }

    private void OnFullscreen(object? obj)
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow != null)
        {
            if (mainWindow.WindowState == WindowState.Normal)
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.WindowStyle = WindowStyle.None;
            }
            else
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }

    public void SetCurrentView(string view)
    {
        CurrentView = view;
    }

    private void OnEdit(object? obj)
    {
        CurrentView = "Edit";
    }

    private void OnExit(object? obj)
    {
        Application.Current.Shutdown();
    }

    private void OnPlay(object? obj)
    {
        CurrentView = "Player";
        PlayerViewModel.ResetPlayer();
        PlayerViewModel.LoadNextQuestion();
    }

    public void DeletePack(QuestionPackViewModel pack)
    {
        if (QuestionPacks.Contains(pack))
        {

            if (ActivePack == pack)
            {
                ActivePack = QuestionPacks.FirstOrDefault(p => p != pack) ?? null;
            }

            QuestionPacks.Remove(pack);
            SavePacksAsync();

            if (QuestionPacks.Count == 0)
                ActivePack = null;
        }
    }

    public void ShowResults(int correctCount, int totalQuestions)
    {
        ResultWindowViewModel resultViewModel = new ResultWindowViewModel(correctCount, totalQuestions, this);
        ResultView resultWindow = new ResultView();
        resultWindow.DataContext = resultViewModel;
        resultWindow.Owner = Application.Current.MainWindow;
        resultWindow.ShowDialog();
    }
}