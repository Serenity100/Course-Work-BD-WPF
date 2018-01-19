using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class EditTablePage : Page
    {
        private String tableName;
        private SqlDataAdapter dataAdapter;

        public EditTablePage(String tableName)
        {
            InitializeComponent();

            this.tableName = tableName;
            Loaded += EditTablePageLoaded;
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private void EditTablePageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dataAdapter = new SqlDataAdapter("Select * from " + tableName, GetSqlConnector().SqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataTable.RowChanged += DataTable_RowChanged;
                dataTable.RowDeleted += DataTable_RowDeleted;
                TableDG.ItemsSource = dataTable.DefaultView;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            dataAdapter.Update(sender as DataTable);
        }

        private void DataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            dataAdapter.Update((sender as DataTable));   
        }

        private void BackToMenuPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetStatisticsPage();
        }
    }
}
