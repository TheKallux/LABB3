using LABB3.ViewModels;
using LABB3.Views;
using System.Windows;
using System.Windows.Input;

namespace LABB3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        DataContext = viewModel;

        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.CurrentView))
            {
                UpdateViewVisibility(viewModel.CurrentView);
            }
        };

        UpdateViewVisibility("Menu");

        RegisterKeyboardShortcuts(viewModel);
    }

    private void RegisterKeyboardShortcuts(MainWindowViewModel viewModel)
    {
        this.InputBindings.Add(new KeyBinding(viewModel.ConfigurationViewModel.AddQuestionCommand,
            new KeyGesture(Key.Insert)));
        this.InputBindings.Add(new KeyBinding(viewModel.ConfigurationViewModel.RemoveQuestionCommand,
            new KeyGesture(Key.Delete)));
        this.InputBindings.Add(new KeyBinding(viewModel.ConfigurationViewModel.EditPackOptionsCommand,
            new KeyGesture(Key.O, ModifierKeys.Control)));
        this.InputBindings.Add(new KeyBinding(viewModel.PlayCommand,
            new KeyGesture(Key.P, ModifierKeys.Control)));
        this.InputBindings.Add(new KeyBinding(viewModel.EditQuestionPackCommand,
            new KeyGesture(Key.E, ModifierKeys.Control)));
        this.InputBindings.Add(new KeyBinding(viewModel.FullScreenCommand,
            new KeyGesture(Key.Return, ModifierKeys.Alt)));
    }

    private void UpdateViewVisibility(string view)
    {
        MenuPanel.Visibility = view == "Menu" ? Visibility.Visible : Visibility.Collapsed;
        MenuView.Visibility = Visibility.Visible;
        PlayerView.Visibility = view == "Player" ? Visibility.Visible : Visibility.Collapsed;
        ConfigurationView.Visibility = view == "Edit" ? Visibility.Visible : Visibility.Collapsed;
    }
}