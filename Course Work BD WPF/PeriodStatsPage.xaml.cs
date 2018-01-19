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
    public partial class PeriodStatsPage : Page
    {
        private bool IsDateChanged = false;
        private StatsGetter stats;
        private int currentChart = 2;

        public PeriodStatsPage()
        {
            InitializeComponent();
            Loaded += LoadInfoToChart;
        }

        private void LoadInfoToChart(object sender, RoutedEventArgs e)
        {
            stats = new StatsGetter(GetSqlConnector());
            stats.LoadInfoFromDB();

            FromDatePicker.Text = stats.GetDates()[0];
            ToDatePicker.Text = stats.GetDates()[stats.GetDates().Count - 1];

            ReloadChartInfo(stats.GetDates()[0], stats.GetDates()[stats.GetDates().Count - 1]);
        }

        private void ReloadChartInfo(String fromDate, String toDate)
        {
            stats.FromDate = fromDate;
            stats.ToDate = toDate;

            AxisX.Labels = stats.GetDates();
            ChartLineSeries.Values = stats.GetPrices();
            ChartColumnSeries.Values = stats.GetPrices();
            ChartOHCLSeries.Values = stats.GetPrices();
        }

        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.Black;
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            (sender as DatePicker).Foreground = Brushes.White;
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

        private class StatsGetter
        {
            private SqlConnector connector;
            private IChartValues Prices;
            private IList<String> Dates;
            public String FromDate { get; set; }
            public String ToDate { get; set; }
            
            public StatsGetter(SqlConnector connector)
            {
                this.connector = connector;
            }

            public IChartValues GetPrices()
            {
                IChartValues FilteredValues = new ChartValues<int>();

                if (GetStartIndex() != -1)
                    for (int i = GetStartIndex(); i <= GetLastIndex(); ++i)
                        FilteredValues.Add(Prices[i]);

                return FilteredValues;
            }

            private int GetStartIndex()
            {
                for (int i = 0; i < Dates.Count; ++i)
                    if (DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(FromDate)) >= 0 && DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(ToDate)) <= 0)
                        return i;

                return -1;
            }

            private int GetLastIndex()
            {
                int j = 0;
                bool WasInside = false;
                for (int i = 0; i < Dates.Count; ++i)
                    if (DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(FromDate)) >= 0 && DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(ToDate)) <= 0)
                    {
                        j = i;
                        WasInside = true;
                    }

                return WasInside == true ? j : -1; 
            }

            public IList <String> GetDates()
            {
                IList<String> FilteredList = new List<String>();

                for (int i = 0; i < Dates.Count; ++i)
                    if (DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(FromDate)) >= 0 && DateTime.Parse(Dates[i]).CompareTo(DateTime.Parse(ToDate)) <= 0)
                        FilteredList.Add(Dates[i]);

                return FilteredList;   
            }

            public void LoadInfoFromDB()
            {
                String request = "select [Дата оформления], sum(Стоимость) as 'Сумма' " +
                                 "from (select [Дата оформления], [Номер], [ISBN книги] " +
                                 "from Заказы left join [Заказы на книги] on " +
                                 "Заказы.Номер = [Заказы на книги].[Номер заказа]) " +
                                 "as temp left join Книги on " +
                                 "temp.[ISBN книги] = Книги.ISBN group by [Дата оформления]";

                SqlCommand command = new SqlCommand(request, connector.SqlConnection);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable("Продавцы");
                sda.Fill(dataTable);

                Prices = new ChartValues<Int32>();
                Dates = new List<String>();

                foreach(DataRow EachRaw in dataTable.Rows)
                {
                    Dates.Add(EachRaw[0].ToString().Remove(11));
                    Prices.Add(Int32.Parse(EachRaw[1].ToString()));
                }

                FromDate = Dates[0];
                ToDate = Dates[Dates.Count - 1];
            }
        }

        private void SelectedFromDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromDatePicker.Text != "" && ToDatePicker.Text != "")
            { 
                if(IsDateChanged == true)
                {
                    IsDateChanged = false;
                    return;
                }

                if (((DateTime)e.AddedItems[0]).CompareTo(DateTime.Parse(ToDatePicker.Text)) > 0)
                {
                    IsDateChanged = true;
                    FromDatePicker.Text = e.RemovedItems[0].ToString();
                }

                else
                    ReloadChartInfo(e.AddedItems[0].ToString(), ToDatePicker.Text);
            }
        }

        private void SelectedToDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromDatePicker.Text != "" && ToDatePicker.Text != "")
            {
                if (IsDateChanged == true)
                {
                    IsDateChanged = false;
                    return;
                }

                if (((DateTime)e.AddedItems[0]).CompareTo(DateTime.Parse(ToDatePicker.Text)) > 0)
                {
                    IsDateChanged = true;
                    FromDatePicker.Text = e.RemovedItems[0].ToString();
                }

                else
                    ReloadChartInfo(FromDatePicker.Text, e.AddedItems[0].ToString());
            }
        }

        private void BackToMenuPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetStatisticsPage();
        }

        private void ChangeChartType(object sender, MouseButtonEventArgs e)
        {
            if(currentChart == 0)
            {
                ChartOHCLSeries.Visibility = Visibility.Visible;
                ChartLineSeries.Visibility = Visibility.Hidden;
                currentChart++;
            }

            else if(currentChart == 1)
            {
                ChartOHCLSeries.Visibility = Visibility.Hidden;
                ChartColumnSeries.Visibility = Visibility.Visible;
                currentChart++;
            }

            else if(currentChart == 2)
            {
                ChartColumnSeries.Visibility = Visibility.Hidden;
                ChartLineSeries.Visibility = Visibility.Visible;
                currentChart = 0;
            }
        }
    }
}
