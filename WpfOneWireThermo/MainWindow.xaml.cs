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
        
        public MainWindow()
        {
            this.AllowsTransparency = true;
            InitializeComponent();
            this.isToolWindowHide = true;
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
                case Key.D0: // text colors
                    this.temperatureViewer.Foreground = Brushes.Black;
                    break;
                case Key.D1:
                    this.temperatureViewer.Foreground = Brushes.White;
                    break;
                case Key.D2:
                    this.temperatureViewer.Foreground = Brushes.Red;
                    break;
                case Key.D3:
                    this.temperatureViewer.Foreground = Brushes.Green;
                    break;
                case Key.D4:
                    this.temperatureViewer.Foreground = Brushes.Blue;
                    break;
                case Key.D5:
                    this.temperatureViewer.Foreground = Brushes.DarkOrange;
                    break;
                case Key.D6:
                    this.temperatureViewer.Foreground = Brushes.Cyan;
                    break;
                case Key.D7:
                    this.temperatureViewer.Foreground = Brushes.MediumSeaGreen;
                    break;
                case Key.D8:
                    this.temperatureViewer.Foreground = Brushes.DarkMagenta;
                    break;
                case Key.D9: //end: text colors 
                    this.temperatureViewer.Foreground = Brushes.Indigo;
                    break;



            }

        }
        //double click on wondow then show toolwindow (???)
        private async void DS18B20SensorViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.isToolWindowHide) {
                await Task.Run(() => {
                    
                        int i = 0;
                while (i != 10)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate { temperatureViewer.Content = $"{i}"; });
                           // this.UpdateLayout();
                            i++;
                            Thread.Sleep(1000);
                        }
                  
                });
                
                this.isToolWindowHide = false;

            }
            else
            {

                this.isToolWindowHide = true;
            }
        }
        // window move
        private void DS18B20SensorViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
 
    }
  
}
