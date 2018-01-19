using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class PasswordPage : Page
    {
        public PasswordPage()
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

        private void QuitApp(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().Close();
        }

        private void ShowBadPassHint()
        {
            BadPassHintLbl.Visibility = Visibility.Visible;
        }

        private void HideBadPassHint()
        {
            BadPassHintLbl.Visibility = Visibility.Hidden;
        }

        private void Login(object sender, MouseButtonEventArgs e)
        {
            String login = LoginTB.Text;
            String pass = PasswordPB.Password;
            SqlConnector sqlConnector = new SqlConnector(login, pass);

            try
            {
                sqlConnector.Connect();

                GetParentWindow().Connector = sqlConnector;

                if (login.Equals("Manager"))
                    GetParentWindow().SetManagerPage();
                else if (login.Equals("Accounting"))
                    GetParentWindow().SetStatisticsPage();
                else
                    GetParentWindow().SetSellerPage();

                HideBadPassHint();
            }

            catch (BadPassword)
            {
                ShowBadPassHint();
            }
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (LoginTB.Text.Length == 0 || PasswordPB.Password.Length == 0)
                    LoginImgBtn.Visibility = Visibility.Hidden;
                else
                    LoginImgBtn.Visibility = Visibility.Visible;
            }
        }

        private void InputPasswordChanged(object sender, RoutedEventArgs e)
        {
            InputTextChanged(null, null);
        }
    }
}
