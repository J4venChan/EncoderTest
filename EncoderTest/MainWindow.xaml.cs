using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using System.Timers;
using System.Windows.Threading;
using OxyPlot.Legends;

namespace EncoderTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EncoderSensor sensor1;
        public EncoderSensor sensor2;
        public EncoderSensor sensor3;
        public EncoderSensor sensor4;

        public PlotModel Model { get; set; }
        private LineSeries lineSeries1;
        private LineSeries lineSeries2;
        private LineSeries lineSeries3;
        private LineSeries lineSeries4;

        private DispatcherTimer timer;
        private Random random1;
        private Random random2;
        private Random random3;
        private Random random4;


        public MainWindow()
        {
            InitializeComponent();
            sensor1 = new EncoderSensor(txb_ip.Text.Trim(), Convert.ToInt32(txb_sensor1Port.Text.Trim()),"传感器1");
            sensor2 = new EncoderSensor(txb_ip.Text.Trim(), Convert.ToInt32(txb_sensor2Port.Text.Trim()), "传感器2");
            sensor3 = new EncoderSensor(txb_ip.Text.Trim(), Convert.ToInt32(txb_sensor3Port.Text.Trim()), "传感器3");
            sensor4 = new EncoderSensor(txb_ip.Text.Trim(), Convert.ToInt32(txb_sensor4Port.Text.Trim()), "传感器4");
           
            DataContext = this;
            Model = new PlotModel { Title = "编码器数据"};
            lineSeries1 = new LineSeries() { Title = $"编码器1" };
            lineSeries2 = new LineSeries() { Title = $"编码器2" };
            lineSeries3 = new LineSeries() { Title = $"编码器3" };
            lineSeries4 = new LineSeries() { Title = $"编码器4" };

            Model.Series.Add(lineSeries1);
            Model.Series.Add(lineSeries2);
            Model.Series.Add(lineSeries3);
            Model.Series.Add(lineSeries4);

            //设置图例
            Legend legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 1,
                LegendTextColor = OxyColors.LightGray,
            };


            Model.Legends.Add(legend);


            random1 = new Random();
            random2 = new Random();
            random3 = new Random();
            random4 = new Random();


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // 设置定时器间隔为100毫秒
            timer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double x1 = lineSeries1.Points.Count > 0 ? lineSeries1.Points[lineSeries1.Points.Count - 1].X + 1 : 0;
            double y1 = random1.NextDouble() * 100; // 生成随机数据
            lineSeries1.Points.Add(new DataPoint(x1, y1));

            double x2 = lineSeries2.Points.Count > 0 ? lineSeries2.Points[lineSeries2.Points.Count - 1].X + 1 : 0;
            double y2 = random2.NextDouble() * 100; // 生成随机数据
            lineSeries2.Points.Add(new DataPoint(x2, y2));

            double x3 = lineSeries3.Points.Count > 0 ? lineSeries3.Points[lineSeries3.Points.Count - 1].X + 1 : 0;
            double y3 = random3.NextDouble() * 100; // 生成随机数据
            lineSeries3.Points.Add(new DataPoint(x3, y3));

            double x4 = lineSeries4.Points.Count > 0 ? lineSeries4.Points[lineSeries4.Points.Count - 1].X + 1 : 0;
            double y4 = random4.NextDouble() * 100; // 生成随机数据
            lineSeries4.Points.Add(new DataPoint(x4, y4));

            // 只显示最近的100个数据点
            if (lineSeries1.Points.Count > 100)
            {
                lineSeries1.Points.RemoveAt(0);
                lineSeries2.Points.RemoveAt(0);
                lineSeries3.Points.RemoveAt(0);
                lineSeries4.Points.RemoveAt(0);

            }

            Model.InvalidatePlot(true);
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("1111");
            sensor1.Connect();
            sensor2.Connect();
            sensor3.Connect();
            sensor4.Connect();

        }


        private void btn_getClockFrequency_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_setClockFrequency_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_getSensorPos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}