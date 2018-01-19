using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Course_Work_BD_WPF
{
    public partial class SellerPage : Page
    {
        public SellerPage()
        {
            InitializeComponent();
        }

        private void ShowSellersFlyoutHint(object sender, MouseButtonEventArgs e)
        {
            String request = "select [Табельный номер], Имя, Фамилия from Продавцы";
            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Продавцы");
            sda.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
            Flyout.Header = "Продавцы";
            Flyout.IsOpen = true;
        }

        private void ShowCouriersFlyoutHint(object sender, MouseButtonEventArgs e)
        {
            String request = "select [Табельный номер], Имя, Фамилия from Курьеры";
            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Курьеры");
            sda.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
            Flyout.Header = "Курьеры";
            Flyout.IsOpen = true;
        }

        private void ShowBuyersFlyoutHint(object sender, MouseButtonEventArgs e)
        {
            String request = "select id, Имя, Фамилия, [Номер телефона] from Покупатели";
            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Покупатели");
            sda.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
            Flyout.Header = "Покупатели";
            Flyout.IsOpen = true;
        }

        private void ShowBooksFlyout(object sender, MouseButtonEventArgs e)
        {
            String request = "select ISBN, Название, Стоимость from Книги";
            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Покупатели");
            sda.Fill(dataTable);
            BookDataGrid.ItemsSource = dataTable.DefaultView;

            FlyoutBooks.Header = "Книги";

            FlyoutBooks.IsOpen = true;
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private void BackToLoginPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetPasswordPage();
        }

        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.Black;
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.White;
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList books = new List<ComboBoxItem>();

            foreach (DataRowView each in BookDataGrid.SelectedItems)
            {
                DataRow dr = each.Row;

                String isbn = dr.Field<Int32>("ISBN").ToString();
                String title = dr.Field<String>("Название");
                String result = String.Concat(isbn.Trim(), "/", title.Trim());

                books.Add(new ComboBoxItem() { Content = result });
            }

            SelectedBooksCmb.ItemsSource = books;
        }

        private void DeliveryMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (((ComboBoxItem)e.AddedItems[0]).Content.ToString() == "Самовывоз")
                {
                    CourierId.IsEnabled = false;
                    CourierId.Value = null;
                    OpenCouriersFlyoutImgBtn.IsEnabled = false;
                }

                else
                {
                    CourierId.IsEnabled = true;
                    OpenCouriersFlyoutImgBtn.IsEnabled = true;
                }

                SelectedDateChanged(null, null);
            }
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRow dataRow = (DataGrid.SelectedItem as DataRowView).Row;

                if (Flyout.Header.Equals("Покупатели"))
                    BuyerId.Value = (Int32)dataRow.ItemArray[0];
                else if (Flyout.Header.Equals("Продавцы"))
                    SellerId.Value = (Int32)dataRow.ItemArray[0];
                else
                    CourierId.Value = (Int32)dataRow.ItemArray[0];

                Flyout.IsOpen = false;
            }

            catch (NullReferenceException)
            {

            }
        }

        private void AddOrderToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();
            SqlCommand sqlCommand = new SqlCommand("AddOrder", sqlConnector.SqlConnection);
      
            OrderGetter order = new OrderGetter(this);

            SqlTransaction transaction = sqlConnector.SqlConnection.BeginTransaction();
            SqlParameter lastInsertedIdParam = new SqlParameter("@result", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@DateOfIssue", SqlDbType.Date).Value = order.GetIssueDate();
            sqlCommand.Parameters.Add("@DateOfCompletion", SqlDbType.Date).Value = order.GetCompletionDate();
            sqlCommand.Parameters.Add("@DeliveryMethod", SqlDbType.NChar).Value = order.GetDeliveryMethod();
            sqlCommand.Parameters.Add("@SellerId", SqlDbType.Int).Value = order.GetSellerId();
            sqlCommand.Parameters.Add("@CourierId", SqlDbType.Int).Value = order.GetCourierId();
            sqlCommand.Parameters.Add("@BuyerId", SqlDbType.Int).Value = order.GetBuyerId();
            sqlCommand.Parameters.Add(lastInsertedIdParam);

            try
            {
                sqlCommand.Transaction = transaction;
                sqlCommand.ExecuteNonQuery();

                IList<SqlCommand> commands = AddBooksCommands((Int32)lastInsertedIdParam.Value);

                foreach (var eachCommand in commands)
                {
                    eachCommand.Transaction = transaction;
                    eachCommand.ExecuteNonQuery();
                }

                transaction.Commit();

                GetParentWindow().ShowSuccessRequestToDB();
            }

            catch (Exception ex)
            {
                GetParentWindow().ShowErrorWhileRequestingToDB(ex.Message);
                transaction.Rollback();
            }

            finally
            {
                transaction.Dispose();
            }
        }

        private IList<SqlCommand> AddBooksCommands(int insertedId)
        {
            IList<SqlCommand> list = new List<SqlCommand>();

            foreach(ComboBoxItem each in SelectedBooksCmb.ItemsSource)
            {
                int ISBN = Int32.Parse(each.Content.ToString().Split('/')[0]);

                SqlCommand command = new SqlCommand("AddOrderOnBooks", GetSqlConnector().SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@OrderId", SqlDbType.Int).Value = insertedId;
                command.Parameters.Add("@BookISBN", SqlDbType.Int).Value = ISBN;

                list.Add(command);
            }

            return list;
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (CompletionDatePicker.Text.Length == 0 || IssueDatePicker.Text.Length == 0 || SellerId.Value.ToString().Length == 0 ||
                    BuyerId.Value.ToString().Length == 0 || (CourierId.IsEnabled == true && CourierId.Value.ToString().Length == 0) || 
                    SelectedBooksCmb.HasItems == false)
                    AddOrderImgBtn.Visibility = Visibility.Hidden;
                else
                    AddOrderImgBtn.Visibility = Visibility.Visible;
            }
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            SelectedDateChanged(null, null);
        }
    }
}
