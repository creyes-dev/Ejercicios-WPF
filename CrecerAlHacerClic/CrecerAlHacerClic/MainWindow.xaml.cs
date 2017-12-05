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
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace CrecerAlHacerClic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animacionAncho = new DoubleAnimation();
            animacionAncho.From = 125;
            animacionAncho.To = this.Width - 30;
            animacionAncho.Duration = TimeSpan.FromSeconds(5); 
            boton.BeginAnimation(Button.WidthProperty, animacionAncho);
       }
    }
}
