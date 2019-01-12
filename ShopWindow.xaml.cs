using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using Shop.ViewModel;

namespace Shop
{
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : UserControl
    {
        public ShopWindow()
        {
            InitializeComponent();
            //Closing += (s, e) => ViewModelLocator.Cleanup();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Reports==(TabItem)Tabs.SelectedItem)
            {
                ChartArea area = new ChartArea("Default");
                Series series = new Series("Series1");
                if (chart.ChartAreas.IndexOf("Default") == -1)
                    chart.ChartAreas.Add(new ChartArea("Default"));
                if (chart.Series.IndexOf("Series1") == -1)
                {
                    chart.Series.Add(new Series("Series1"));
                    chart.Series["Series1"].ChartArea = "Default";
                    chart.Series["Series1"].ChartType = SeriesChartType.Pie;
                    chart.Series["Series1"].Font = new System.Drawing.Font("Colibri", 14);
                }
                ViewModelLocator loc = new ViewModelLocator();
                chart.Series["Series1"].Points.DataBind(loc.ShopWind.statistics, "name_gc", "amount", "");
            }
        }
    }
}
 