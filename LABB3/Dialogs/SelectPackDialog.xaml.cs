using System.Windows;
using LABB3.ViewModels;

namespace LABB3.Dialogs
{
    internal partial class SelectPackDialog : Window
    {
        public QuestionPackViewModel? SelectedPack { get; set; }

        public SelectPackDialog(List<QuestionPackViewModel> packs)
        {
            InitializeComponent();
            PackListBox.ItemsSource = packs;
            PackListBox.DisplayMemberPath = "Name";
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (PackListBox.SelectedItem is QuestionPackViewModel pack)
            {
                SelectedPack = pack;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select a pack!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}