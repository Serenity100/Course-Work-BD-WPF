using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class AddPublishToDBPage : Page
    {
        public AddPublishToDBPage()
        {
            InitializeComponent();
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private void BackToManagerPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetManagerPage();
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private void AddPublishToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();

            SqlCommand sqlCommand = new SqlCommand("AddPublish", sqlConnector.SqlConnection);
            PublishGetter publish = new PublishGetter(this);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@Title", SqlDbType.NChar).Value = publish.GetTitle();
            sqlCommand.Parameters.Add("@Address", SqlDbType.NChar).Value = publish.GetAddress();

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

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private bool IsContentEmpty(TextBox textBox)
        {
            return textBox.Text.Length == 0;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsLoaded)
            {
                if (IsContentEmpty(TitleTB) || IsContentEmpty(CityTB) || IsContentEmpty(StreetTB) ||
                    IsContentEmpty(HouseTB) || IsContentEmpty(FlatTB))
                    AddPublishmentImgBtn.Visibility = Visibility.Hidden;
                else
                    AddPublishmentImgBtn.Visibility = Visibility.Visible;
            }
        }
    }
}