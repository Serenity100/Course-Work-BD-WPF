using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class ManagerPage : Page
    {
        public ManagerPage()
        {
            InitializeComponent();
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private void BackToPassPage(object sender, MouseButtonEventArgs e)
        {
            (GetParentWindow() as MainWindow).SetPasswordPage();
        }

        private void MoveToAddCustomerPage(object sender, RoutedEventArgs e)
        {
            (GetParentWindow() as MainWindow).SetAddCustomerPage();
        }

        private void MoveToAddBookPage(object sender, RoutedEventArgs e)
        {
            (GetParentWindow() as MainWindow).SetAddBookPage();
        }

        private void MoveToAddPublishPage(object sender, RoutedEventArgs e)
        {
            (GetParentWindow() as MainWindow).SetAddPublishPage();
        }

        private async void MoveToAddEmployeePage(object sender, RoutedEventArgs e)
        {
            await (GetParentWindow() as MainWindow).SetAddEmployeePageAsync();
        }

        private void MoveToAddAuthorPage(object sender, RoutedEventArgs e)
        {
            (GetParentWindow() as MainWindow).SetAddAuthorPage();
        }
    }
}
