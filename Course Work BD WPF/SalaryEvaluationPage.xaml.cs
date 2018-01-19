using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Course_Work_BD_WPF
{
    public partial class SalaryEvaluationPage : Page
    {
        private int currentChart = 2;
        private Employee workerType;
        private StatsGetter stats;
        
        public SalaryEvaluationPage(Employee workerType)
        {
            InitializeComponent();
            Loaded += LoadInfoToChart;
            this.workerType = workerType;
        }

        private void LoadInfoToChart(object sender, RoutedEventArgs e)
        {
            stats = new StatsGetter(GetSqlConnector(), workerType);
            
            FromDatePicker.Text = "01.01.2000";
            ToDatePicker.Text = DateTime.Now.ToString();

            ReloadChartInfo("01.01.2000", DateTime.Now.ToString());
        }

        private void ReloadChartInfo(String fromDate, String toDate)
        {
            stats.FromDate = fromDate;
            stats.ToDate = toDate;

            stats.LoadInfoFromDB();

            AxisX.Labels = stats.WorkerLabels;
            ChartLineSeries.Values = stats.Payment;
            ChartColumnSeries.Values = stats.Payment;
            ChartOHCLSeries.Values = stats.Payment;
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

        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.Black;
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.White;
        }

        private void BackToMenuPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetStatisticsPage();
        }

        private void ChangeChartType(object sender, MouseButtonEventArgs e)
        {
            if (currentChart == 0)
            {
                ChartOHCLSeries.Visibility = Visibility.Visible;
                ChartLineSeries.Visibility = Visibility.Hidden;
                currentChart++;
            }

            else if (currentChart == 1)
            {
                ChartOHCLSeries.Visibility = Visibility.Hidden;
                ChartColumnSeries.Visibility = Visibility.Visible;
                currentChart++;
            }

            else if (currentChart == 2)
            {
                ChartColumnSeries.Visibility = Visibility.Hidden;
                ChartLineSeries.Visibility = Visibility.Visible;
                currentChart = 0;
            }
        }

        private void SelectedFromDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadChartInfo(e.AddedItems[0].ToString(), ToDatePicker.Text);
        }

        private void SelectedToDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadChartInfo(FromDatePicker.Text, e.AddedItems[0].ToString());
        }

        private class StatsGetter
        {
            private SqlConnector connector;
            public IChartValues Payment { get; private set; }
            public IList<String> WorkerLabels { get; private set; }
            private Employee workerType;
            public String FromDate { get; set; }
            public String ToDate { get; set; }

            public StatsGetter(SqlConnector connector, Employee workerType)
            {
                this.connector = connector;
                this.workerType = workerType;
            }

            public void LoadInfoFromDB()
            {
                String workerID = workerType == Employee.Seller ? "[Табельный номер продавца]" : "[Табельный номер курьера]";
                String request = "select " + workerID + ", sum(Стоимость) as 'Стоимость' " +
                                 "from " +
                                 "(select [Дата оформления], " + workerID + ", sum(Стоимость) as 'Стоимость' " +
                                 "from " +
                                 "(select [Дата оформления], " + workerID + ", [ISBN книги] " +
                                 "from Заказы left join [Заказы на книги] " +
                                 "on Заказы.Номер = [Заказы на книги].[Номер заказа]) as temp left join Книги " +
                                 "on temp.[ISBN книги] = Книги.ISBN " +
                                 "where [Дата оформления] >= '" + FromDate + "' and [Дата оформления] <= '" + ToDate + "' and " + workerID + " is not null " +
                                 "group by " + workerID + ", [Дата оформления]) as t " +
                                 "group by " + workerID;

                SqlCommand command = new SqlCommand(request, connector.SqlConnection);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable("Работник");

                sda.Fill(dataTable);
              
                Payment = new ChartValues<int>();
                WorkerLabels = new List<String>();
                
                for (int i = 0; i < dataTable.Rows.Count; ++i)
                {
                    WorkerLabels.Add(dataTable.Rows[i][0].ToString());
                    Payment.Add((int)dataTable.Rows[i][1]);
                }
            }
        }
    }
}

