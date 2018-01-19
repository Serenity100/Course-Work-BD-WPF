using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class AddCustomerToDBPage : Page
    {
        public AddCustomerToDBPage()
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

        private void BackToManagerPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetManagerPage();
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private void AddClientToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();

            SqlCommand sqlCommand = new SqlCommand("AddCustomer", sqlConnector.SqlConnection);
            CustomerGetter customer = new CustomerGetter(this);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@name", SqlDbType.NChar).Value = customer.GetName();
            sqlCommand.Parameters.Add("@surname", SqlDbType.NChar).Value = customer.GetSurname();
            sqlCommand.Parameters.Add("@phone", SqlDbType.NChar).Value = customer.GetPhone();
            sqlCommand.Parameters.Add("@address", SqlDbType.NChar).Value = customer.GetAddress();

            try
            {
                sqlCommand.ExecuteNonQuery();
                GetParentWindow().ShowSuccessRequestToDB();
            }

            catch(Exception ex)
            {
                GetParentWindow().ShowErrorWhileRequestingToDB(ex.Message);
            }
        }

        private bool IsContentEmpty(TextBox textBox)
        {
            return textBox.Text.Length == 0;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsLoaded)
            {
                if (IsContentEmpty(NameTB) || IsContentEmpty(SurnameTB) || IsContentEmpty(PhoneTB)
                    || IsContentEmpty(CityTB) || IsContentEmpty(StreetTB) || IsContentEmpty(HouseTB) || IsContentEmpty(FlatTB))
                    AddCustomerImgBtn.Visibility = Visibility.Hidden;
                else
                    AddCustomerImgBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
