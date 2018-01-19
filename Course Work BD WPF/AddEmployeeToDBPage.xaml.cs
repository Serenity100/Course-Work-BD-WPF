using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Course_Work_BD_WPF
{
    public partial class AddEmployeeToDBPage : Page
    {
        private Employee employee;

        public AddEmployeeToDBPage(Employee employee)
        {
            InitializeComponent();
            PageTitle.Content += " - " + (employee.Equals(Employee.Courier) ? "Courier" : "Seller");
            this.employee = employee;
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

        private void AddEmployeeToDB(object sender, MouseButtonEventArgs e)
        {
            SqlConnector sqlConnector = GetSqlConnector();

            SqlCommand sqlCommand = new SqlCommand("AddEmployee", sqlConnector.SqlConnection);
            EmployeeGetter employee = new EmployeeGetter(this);

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add("@Name", SqlDbType.NChar).Value = employee.GetName();
            sqlCommand.Parameters.Add("@Surname", SqlDbType.NChar).Value = employee.GetSurname();
            sqlCommand.Parameters.Add("@Phone", SqlDbType.NChar).Value = employee.GetPhone();
            sqlCommand.Parameters.Add("@BirthdayDate", SqlDbType.Date).Value = employee.GetBirthdayDate();
            sqlCommand.Parameters.Add("@EmploymentDate", SqlDbType.Date).Value = employee.GetEmploymentDate();
            sqlCommand.Parameters.Add("@WorkerType", SqlDbType.Bit).Value = !employee.Equals(Employee.Courier);

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
        
        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.Black;
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.White;
        }

        private bool IsContentEmpty(TextBox textBox)
        {
            return textBox.Text.Length == 0;
        }

        private bool IsContentEmpty(DatePicker datePicker)
        {
            return datePicker.Text.Length == 0;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsLoaded)
            {
                if (IsContentEmpty(NameTB) || IsContentEmpty(SurnameTB) || IsContentEmpty(PhoneTB) ||
                    IsContentEmpty(EmploymentDatePicker) || IsContentEmpty(BirthdayDatePicker))
                    AddEmployeeBtn.Visibility = Visibility.Hidden;
                else
                    AddEmployeeBtn.Visibility = Visibility.Visible;
            }
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TextChanged(null, null);
        }
    }
}
