using LABB3.ViewModels;
using LABB3.Views;
using System.Windows;

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
    }

    private void UpdateViewVisibility(string view)
    {
        MenuPanel.Visibility = view == "Menu" ? Visibility.Visible : Visibility.Collapsed;
        MenuView.Visibility = Visibility.Visible;
        PlayerView.Visibility = view == "Player" ? Visibility.Visible : Visibility.Collapsed;
        ConfigurationView.Visibility = view == "Edit" ? Visibility.Visible : Visibility.Collapsed;
    }
}