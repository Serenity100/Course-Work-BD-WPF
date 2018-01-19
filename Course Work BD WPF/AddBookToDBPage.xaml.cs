using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class AddBookToDBPage : Page
    {
        public AddBookToDBPage()
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

        private void AddBookToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();

            SqlCommand sqlCommand = new SqlCommand("AddBook", sqlConnector.SqlConnection);
            BookGetter book = new BookGetter(this);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@ISBN", SqlDbType.Int).Value = book.GetISBN();
            sqlCommand.Parameters.Add("@Title", SqlDbType.NChar).Value = book.GetTitle();
            sqlCommand.Parameters.Add("@PublishYear", SqlDbType.Date).Value = book.GetPublishYear();
            sqlCommand.Parameters.Add("@PublishAddress", SqlDbType.NChar).Value = book.GetAddress();
            sqlCommand.Parameters.Add("@Price", SqlDbType.Int).Value = book.GetPrice();

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

        private void ShowPublishesFlyoutHint(object sender, MouseButtonEventArgs e)
        {
            String request = "select * from Издательства";
            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Издательства");
            sda.Fill(dataTable);
            PublishDataGrid.ItemsSource = dataTable.DefaultView;
            PublishFlyout.IsOpen = true;
        }

        private void PublishDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRow DataRow = (PublishDataGrid.SelectedItem as DataRowView).Row;

                string Address = DataRow.Field<String>("Адрес");

                CityTB.Text = Address.Substring(3, Address.IndexOf(",") - 3);
                StreetTB.Text = Address.Substring(Address.IndexOf("ул. ") + 4, Address.IndexOf(",", Address.IndexOf("ул. ") + 4) - (Address.IndexOf("ул. ") + 4));
                HouseTB.Text = Address.Substring(Address.IndexOf("д. ") + 3, Address.LastIndexOf(",") - (Address.IndexOf("д. ") + 3));
                FlatTB.Text = Address.Substring(Address.IndexOf("кв. ") + 4, Address.Length - (Address.IndexOf("кв. ") + 4));

                PublishFlyout.IsOpen = false;
            }

            catch (NullReferenceException)
            {

            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (ISBNTB.Text.Length == 0 || TitleTB.Text.Length == 0 || YearTB.Text.Length != 4 || PriceTB.Text.Length == 0 ||
                    CityTB.Text.Length == 0 || FlatTB.Text.Length == 0 || HouseTB.Text.Length == 0 || StreetTB.Text.Length == 0)
                    AddBookBtn.Visibility = Visibility.Hidden;
                else
                    AddBookBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
