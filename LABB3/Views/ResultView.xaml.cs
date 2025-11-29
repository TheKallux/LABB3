using LABB3.ViewModels;
using System.Windows;

namespace LABB3.Views
{
    public partial class ResultView : Window
    {
        public ResultView(int correctCount, int totalQuestions)
        {
            InitializeComponent();
            var viewModel = new ResultWindowViewModel();
            viewModel.CorrectAnswers = correctCount;
            viewModel.TotalQuestions = totalQuestions;
            DataContext = viewModel;
        }
    }
}