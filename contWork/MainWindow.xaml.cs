using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace contWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Period { get; set; }

        DispatcherTimer Timer;
        TimeSpan Time { get; set; }

        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Period = int.Parse(textBox_Period.Text);
            GoTimer();
            ThreadPool.QueueUserWorkItem(Go);
        }

        private void CreateScreenShot()
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    string filename = "Screen-" + Time.ToString() + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("C:\\screenshots\\" + filename);
                    Opacity = 1;
                }
            }
        }

        private void GoTimer()
        {
            Time = TimeSpan.FromSeconds(Period * 60);

            Timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                timer.Text = Time.ToString("c");
                if (Time == TimeSpan.Zero) Timer.Stop();
                Time = Time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            Timer.Start();
        }

        public void Go(object state)
        {
            int TimeToScreen = random.Next(0, Period*60);
            TimeSpan ts = TimeSpan.FromSeconds((double)TimeToScreen);
            while(Time != TimeSpan.Zero)
            {
                if (ts == Time)
                {
                    CreateScreenShot();
                }
            }
        }

    }
}
