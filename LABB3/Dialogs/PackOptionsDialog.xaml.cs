using System.Windows;
using System.Windows.Controls;

namespace LABB3.Dialogs
{
    public partial class PackOptionsDialog : Window
    {
        public string PackName { get; set; }
        public string SelectedDifficulty { get; set; }

        public PackOptionsDialog(string packName, string difficulty)
        {
            InitializeComponent();
            PackNameTextBox.Text = packName;
            DifficultyComboBox.SelectedItem = difficulty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PackName = PackNameTextBox.Text;
            SelectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem).Content.ToString();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}