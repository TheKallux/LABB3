using System.Windows;
using System.Windows.Controls;

namespace LABB3.Dialogs
{
    public partial class CreateNewPackDialog : Window
    {
        public string PackName { get; set; }
        public string SelectedDifficulty { get; set; }

        public CreateNewPackDialog()
        {
            InitializeComponent();
            DifficultyComboBox.SelectedIndex = 0;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PackNameTextBox.Text))
            {
                MessageBox.Show("Pack name cannot be empty!");
                return;
            }

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