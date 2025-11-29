using System.Windows;
using System.Windows.Controls;

namespace LABB3.Dialogs;

public partial class PackOptionsDialog : Window
{
    public string PackName { get; set; }
    public string SelectedDifficulty { get; set; }
    public int TimePerQuestion { get; set; }

    public PackOptionsDialog(string packName, string difficulty, int timerPerQuestion = 30)
    {
        InitializeComponent();
        PackNameTextBox.Text = packName;
        DifficultyComboBox.SelectedItem = difficulty;
        TimeSlider.Value = timerPerQuestion;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (DifficultyComboBox.SelectedItem == null)
        {
            MessageBox.Show("Pick a difficulty!");
            return;
        }

        PackName = PackNameTextBox.Text;
        SelectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem).Content.ToString();
        TimePerQuestion = (int)TimeSlider.Value;
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}