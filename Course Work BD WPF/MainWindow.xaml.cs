using MahApps.Metro.Controls;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace Course_Work_BD_WPF
{
    public partial class MainWindow : MetroWindow
    {
        public  SqlConnector Connector { set; get; }

        public MainWindow()
        {
            InitializeComponent();
            SetPasswordPage();
        }

        public void SetManagerPage()
        {
            RootFrame.Navigate(new ManagerPage());
        }

        public void SetSellerPage()
        {
            RootFrame.Navigate(new SellerPage());
        }

        public void SetPasswordPage()
        {
            RootFrame.Navigate(new PasswordPage());
        }

        public void SetAddCustomerPage()
        {
            RootFrame.Navigate(new AddCustomerToDBPage());
        }

        public void SetAddBookPage()
        {
            RootFrame.Navigate(new AddBookToDBPage());
        }

        public void SetAddPublishPage()
        {
            RootFrame.Navigate(new AddPublishToDBPage());
        }

        public void SetAddAuthorPage()
        {
            RootFrame.Navigate(new AddAuthorToDBPage());
        }

        public void SetStatisticsPage()
        {
            RootFrame.Navigate(new StatisticsPage());
        }

        public void SetSelectTablePage()
        {
            RootFrame.Navigate(new SelectTablePage());
        }

        public void SetSelectTableForUpdatePage()
        {
            RootFrame.Navigate(new SelectTablePage(false));
        }

        public void SetEditTablePage(String tableName)
        {
            RootFrame.Navigate(new EditTablePage(tableName));
        }

        public async void SetSalaryEvalPage()
        {
            MetroDialogSettings metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Продавцов",
                NegativeButtonText = "Курьеров",
                AnimateHide = true,
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult x = await this.ShowMessageAsync("Чью зарплату смотрим?", "", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);

            RootFrame.Navigate(new SalaryEvaluationPage(x.Equals(MessageDialogResult.Affirmative) ? Employee.Seller : Employee.Courier));
        }

        public void SetSelectColumnsPage(String tableName)
        {
            RootFrame.Navigate(new SelectColumnsPage(tableName));
        }

        public void SetStatisticPage()
        {
            RootFrame.Navigate(new PeriodStatsPage());
        }

        public async Task SetAddEmployeePageAsync()
        {
            MetroDialogSettings metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Продавец",
                NegativeButtonText = "Курьер",
                AnimateHide = true,
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult x = await this.ShowMessageAsync("Кого добавляем?", "", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);

            RootFrame.Navigate(new AddEmployeeToDBPage(x.Equals(MessageDialogResult.Affirmative) ? Employee.Seller : Employee.Courier));
        }

        public void ShowSuccessRequestToDB() => this.ShowMessageAsync("Отлично!", "Данные записаны в базу");
        public void ShowErrorWhileRequestingToDB(string message) => this.ShowMessageAsync("Упс...", message);
    }
}
