using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;


namespace WpfOneWireThermo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean isToolWindowHide;
        private DS18B20 thermometer;

        private Boolean started = false;
        private String port = "X";
        
        public MainWindow()
        {
            this.AllowsTransparency = true;
            InitializeComponent();
            this.isToolWindowHide = true;

            this.temperatureViewer.Content = "Stopped";
            thermometer = new DS18B20();
            port = "COM3";
            //thermometer.OneWireRun("COM3");
            //ShowTemp();
        }

        private void test()
        {
           
            
        }
        //Window always on top
        private void WindowDeactivated_Event(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        //if keyboard click
        private void DS18B20SensorViewer_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.OemPlus: // opacity increase
                    this.VisualOpacity += (this.VisualOpacity < 1.0)? 0.1 : 0.0;
                    break;
                case Key.OemMinus: // opcity decrease
                    this.VisualOpacity -= (this.VisualOpacity > 0.0) ? 0.1 : 0.0;
                    break;
                case Key.B: // background turn on/off
                    this.Background = (this.Background == Brushes.Transparent) ? Brushes.White:Brushes.Transparent;
                    break;
                case Key.D0: // text colors or COMX
                    if (started == false)
                    {
                        port = "COM0";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Black;
                    break;
                case Key.D1:
                    if (started == false)
                    {
                        port = "COM1";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.White;
                    break;
                case Key.D2:
                    if (started == false)
                    {
                        port = "COM2";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Red;
                    break;
                case Key.D3:
                    if (started == false)
                    {
                        port = "COM3";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Green;
                    break;
                case Key.D4:
                    if (started == false)
                    {
                        port = "COM4";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Blue;
                    break;
                case Key.D5:
                    if (started == false)
                    {
                        port = "COM5";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.DarkOrange;
                    break;
                case Key.D6:
                    if (started == false)
                    {
                        port = "COM6";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Cyan;
                    break;
                case Key.D7:
                    if (started == false)
                    {
                        port = "COM7";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.MediumSeaGreen;
                    break;
                case Key.D8:
                    if (started == false)
                    {
                        port = "COM8";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.DarkMagenta;

                    break;
                case Key.D9: //end: text colors 
                    if (started == false)
                    {
                        port = "COM9";
                        this.temperatureViewer.Content = port;
                    }
                    else
                        this.temperatureViewer.Foreground = Brushes.Indigo;
                    break;
                case Key.T: // show/hide on task bar
                    this.ShowInTaskbar = (this.ShowInTaskbar == true) ? false : true;
                    break;
                case Key.R:
                    if (started == false)
                    {
                       
                        foreach (var aport in thermometer.oneWireAdapterPorts)
                        {
                            if (aport.Equals(port))
                            {
                                started = true;
                                thermometer.OneWireRun(port);
                                ShowTemp();
                            }
                        }
                    }
                    else
                    {
                        started = false;
                    }
                    break;

                default:
                    break;

            }

        }
        //double click on wondow then show toolwindow (???)
        private void DS18B20SensorViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.isToolWindowHide)
            {

                this.isToolWindowHide = false;

            }
            else
            {
                //todo: 
                this.isToolWindowHide = true;
            }
        }
        private void ShowTemp()
        {
            Task.Run(async () =>
            {
                while (started)
                {
                    await this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        temperatureViewer.Content = $"{thermometer.GetSensorTemperature()}°C";
                        temperatureViewer.UpdateLayout();
                    });

                    await Task.Delay(5000);
                    //Thread.Sleep(4000);
                }
                await this.Dispatcher.BeginInvoke((Action)delegate
                {
                    temperatureViewer.Content = "Stopped";
                    temperatureViewer.UpdateLayout();
                });
                

            });
        }
        // window move
        private void DS18B20SensorViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
 
    }
  
}
