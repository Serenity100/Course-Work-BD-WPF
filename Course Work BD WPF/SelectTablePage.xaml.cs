using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Course_Work_BD_WPF
{
    public partial class SelectTablePage : Page
    {
        bool NextPageFlag;

        public SelectTablePage(bool NextPageFlag = true)
        {
            InitializeComponent();
            this.NextPageFlag = NextPageFlag;
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

        private void TileClicked(object sender, RoutedEventArgs e)
        {
            if (NextPageFlag)
                GetParentWindow().SetSelectColumnsPage((sender as Tile).Title);
            else
                GetParentWindow().SetEditTablePage((sender as Tile).Title);
        }

        private void BackToStatisticsPage(object sender, MouseButtonEventArgs e)
        {
            GetParentWindow().SetStatisticsPage();
        }
    }
}
