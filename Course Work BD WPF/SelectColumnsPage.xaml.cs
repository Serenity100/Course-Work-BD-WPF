using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Course_Work_BD_WPF
{
    public partial class SelectColumnsPage : Page
    {
        String tableName;

        public SelectColumnsPage(String tableName)
        {
            InitializeComponent();
            this.tableName = tableName;

            Loaded += PageLoaded;
        }

        private MainWindow GetParentWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private SqlConnector GetSqlConnector()
        {
            return GetParentWindow().Connector;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            IReadOnlyList <String> columns = (new SchemaColumnsGetter(tableName, GetSqlConnector())).GetColumns();

            foreach(String eachColumn in columns)
            {
                var toggleButton = new ToggleSwitch
                {
                    Content = eachColumn,
                    Foreground = Brushes.White,
                    IsChecked = true
                };

                ColumnsStackPanel.Children.Add(toggleButton);
            }
        }

        private void StartMoving(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().DragMove();
        }

        private void BackToSelectTablePage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetSelectTablePage();
        }

        private IList<String> GetSelectedColumns()
        {
            IList <String> selectedColumns = new List<String>();

            foreach(var eachTS in ColumnsStackPanel.Children)
            {
                ToggleSwitch ts = eachTS as ToggleSwitch;

                if (ts.IsChecked == true)
                    selectedColumns.Add(ts.Content as String);
            }

            return selectedColumns;
        }

        private void ShowSelectedTableView(object sender, MouseButtonEventArgs e)
        {
            IList<String> selectedColumns = GetSelectedColumns();

            if(selectedColumns.Count == 0)
            {
                GetParentWindow().ShowErrorWhileRequestingToDB("Выберите хотя бы одну колонку");
                return;
            }

            String request = "select top " + RowNumberUpDown.Value + " ";

            for (int i = 0; i < selectedColumns.Count; ++i)
            {
                request += "["+selectedColumns[i]+"]";

                if (i != selectedColumns.Count - 1)
                    request += ", ";
            }

            request += " from " + "[" + tableName + "]";

            SqlConnector connector = GetSqlConnector();

            SqlCommand command = new SqlCommand(request, connector.SqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable(tableName);
            sda.Fill(dataTable);
            SelectedTableViewDG.ItemsSource = dataTable.DefaultView;

            TableViewFlyout.Header = tableName;
            TableViewFlyout.IsOpen = true;
        }

        private void SaveToExcelFormat(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };

            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

            for (int j = 0; j < SelectedTableViewDG.Columns.Count; j++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[1, j + 1];
                sheet.Cells[1, j + 1].Font.Bold = true;
                sheet.Columns[j + 1].ColumnWidth = 18;
                myRange.Value2 = SelectedTableViewDG.Columns[j].Header;
            }

            for (int i = 0; i < SelectedTableViewDG.Columns.Count; i++)
            {
                for (int j = 0; j < SelectedTableViewDG.Items.Count; j++)
                {
                    if (SelectedTableViewDG.Columns[i].GetCellContent(SelectedTableViewDG.Items[j]) is TextBlock b)
                    {
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[j + 2, i + 1];
                        myRange.Value2 = b.Text;
                    }
                }
            }
        }

    }
}
