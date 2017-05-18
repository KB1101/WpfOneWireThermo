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
        public MainWindow(Boolean allow)
        {
            this.AllowsTransparency = allow;
            InitializeComponent();
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
                case Key.OemPlus:
                    this.VisualOpacity += (this.VisualOpacity < 1.0)? 0.1 : 0.0;
                    break;
                case Key.OemMinus:
                    this.VisualOpacity -= (this.VisualOpacity > 0.0) ? 0.1 : 0.0;
                    break;

            }


        }
        //double click on wondow then show toolwindow (???)
        private void DS18B20SensorViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.isToolWindowHide) {

                this.isToolWindowHide = false;
            }
            else
            {

                this.isToolWindowHide = true;
            }
        }

        private void DS18B20SensorViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
