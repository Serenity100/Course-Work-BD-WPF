using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class StatisticsPage : Page
    {
        public StatisticsPage() => InitializeComponent();
        private MainWindow GetParentWindow() => Window.GetWindow(this) as MainWindow;
        private void StartMoving(object sender, MouseButtonEventArgs e) => GetParentWindow().DragMove();
        private void BackToPassPage(object sender, MouseButtonEventArgs e) => GetParentWindow().SetPasswordPage();
        private void MoveToQueryBuilder(object sender, RoutedEventArgs e) => GetParentWindow().SetSelectTablePage();
        private void MoveToStatisticPage(object sender, RoutedEventArgs e) => GetParentWindow().SetStatisticPage();

        private void MoveToSalaryEvalPage(object sender, RoutedEventArgs e)
        {
            GetParentWindow().SetSalaryEvalPage();
        }

        private void MoveToChangeDBPage(object sender, RoutedEventArgs e)
        {
            GetParentWindow().SetSelectTableForUpdatePage();
        }
    }
}
