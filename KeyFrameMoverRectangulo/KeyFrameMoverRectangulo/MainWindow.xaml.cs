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

namespace KeyFrameMoverRectangulo
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Crear la transformacion de desplazamiento
            TranslateTransform transformacionTralsado = new TranslateTransform();
            rectangulo.RenderTransform = transformacionTralsado;

            // Crear la animación que servira para orientar la posicion horizontal de la figura
            DoubleAnimationUsingKeyFrames transformacionAnimacion = new DoubleAnimationUsingKeyFrames();
            transformacionAnimacion.Duration = TimeSpan.FromSeconds(10);

            // Animar a 100 en 3 segundos
            transformacionAnimacion.KeyFrames.Add(
                new LinearDoubleKeyFrame(100, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));

            // Animar en 300 en 4 segundos
            transformacionAnimacion.KeyFrames.Add(
                new LinearDoubleKeyFrame(300, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4))));

            // Animar a 500 en 10 segundos
            transformacionAnimacion.KeyFrames.Add(
                new LinearDoubleKeyFrame(500, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(10))));

            // Iniciar la animacion
            transformacionTralsado.BeginAnimation(TranslateTransform.XProperty, transformacionAnimacion);
        }
    }
}
