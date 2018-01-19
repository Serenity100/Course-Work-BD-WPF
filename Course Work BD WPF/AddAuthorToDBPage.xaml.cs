using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class AddAuthorToDBPage : Page
    {
        public AddAuthorToDBPage()
        {
            InitializeComponent();
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private void BackToManagerPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetManagerPage();
        }

        private void AddAuthorToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();
            SqlCommand sqlCommand = new SqlCommand("AddAuthor", sqlConnector.SqlConnection);

            AuthorGetter author = new AuthorGetter(this);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@Name", SqlDbType.NChar).Value = author.GetName();
            sqlCommand.Parameters.Add("@Surname", SqlDbType.NChar).Value = author.GetSurname();
            sqlCommand.Parameters.Add("@Patronymic", SqlDbType.NChar).Value = author.GetPatronymic();
            sqlCommand.Parameters.Add("@BirthdayYear", SqlDbType.Date).Value = "01.01." + author.GetBirthdayYear();

            try
            {
                sqlCommand.ExecuteNonQuery();
                GetParentWindow().ShowSuccessRequestToDB();
            }

            catch (Exception ex)
            {
                GetParentWindow().ShowErrorWhileRequestingToDB(ex.Message);
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsLoaded)
            {
                if (NameTB.Text.Length == 0 || SurnameTB.Text.Length == 0 || PatronymicTB.Text.Length == 0 || BirthdayYearTB.Text.Length != 4)
                    AddAuthorBtn.Visibility = Visibility.Hidden;
                else
                    AddAuthorBtn.Visibility = Visibility.Visible;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
