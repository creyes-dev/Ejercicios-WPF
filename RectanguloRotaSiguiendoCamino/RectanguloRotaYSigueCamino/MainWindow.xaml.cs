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

namespace RectanguloRotaYSigueCamino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Create a NameScope for the page so that
            // we can use Storyboards.
            //NameScope.SetNameScope(this, new NameScope());

            // Crear un rectangulo y agregarlo al canvas
            Rectangle rectangulo = new Rectangle();
            rectangulo.Width = 30;
            rectangulo.Height = 30;
            rectangulo.Fill = Brushes.Blue;
            canvas.Children.Add(rectangulo);
           
            // Crear las transformaciones de desplazamiento y rotación
            RotateTransform animacionRotacion = new RotateTransform();
            TranslateTransform animacionTraslado = new TranslateTransform();

            // Crear el grupo de transformaciones que va a contener las animaciones de
            // transformación de desplazamiento y rotación
            TransformGroup grupoTransformaciones = new TransformGroup();
            grupoTransformaciones.Children.Add(animacionRotacion);
            grupoTransformaciones.Children.Add(animacionTraslado);

            // Asociar el grupo de transformaciones al rectángulo
            rectangulo.RenderTransform = grupoTransformaciones; 

            // Registrar las transformaciones para que puedan ser utilizadas
            // por el StoryBoard
            this.RegisterName("AnimatedRotateTransform", animacionRotacion);
            this.RegisterName("AnimatedTranslateTransform", animacionTraslado);
            
            // Crear el camino que seguirá el rectángulo
            PathGeometry caminoAnimacion = new PathGeometry();
            PathFigure camino = new PathFigure();
            camino.StartPoint = new Point(10, 100);
            PolyBezierSegment segmentoBezier = new PolyBezierSegment();
            segmentoBezier.Points.Add(new Point(35, 0));
            segmentoBezier.Points.Add(new Point(135, 0));
            segmentoBezier.Points.Add(new Point(160, 100));
            segmentoBezier.Points.Add(new Point(180, 190));
            segmentoBezier.Points.Add(new Point(285, 200));
            segmentoBezier.Points.Add(new Point(310, 100));
            camino.Segments.Add(segmentoBezier);
            caminoAnimacion.Figures.Add(camino);

            caminoAnimacion.Freeze();

            // Crear la animación que orienta el ángulo del rectángulo
            // en función del ángulo del segmento del camino en el punto del trayecto
            // en el que se encuentre
            DoubleAnimationUsingPath animacionAngulo = new DoubleAnimationUsingPath();
            animacionAngulo.PathGeometry = caminoAnimacion;
            animacionAngulo.Duration = TimeSpan.FromSeconds(5);
            
            // El origen del valor de la animación en un punto 
            // es el angulo del camino en dicho punto
            animacionAngulo.Source = PathAnimationSource.Angle; 

            // Registrar la animacion para asignar el valor de la 
            // propiedad del angulo de la animación de transformación
            Storyboard.SetTargetName(animacionAngulo, "AnimatedRotateTransform");
            Storyboard.SetTargetProperty(animacionAngulo,
                new PropertyPath(RotateTransform.AngleProperty));

            // Crear la animación que orienta la posicion horizontal del rectángulo
            // en función del punto horizontal del segmento del camino en el punto
            // del trayecto en el que se encuentre
            DoubleAnimationUsingPath animacionTrasladoHorizontal =
                new DoubleAnimationUsingPath();
            animacionTrasladoHorizontal.PathGeometry = caminoAnimacion;
            animacionTrasladoHorizontal.Duration = TimeSpan.FromSeconds(5);

            // El origen del valor de la animación en un punto 
            // es el valor de x del camino en dicho punto
            animacionTrasladoHorizontal.Source = PathAnimationSource.X;

            // Registrar la animacion para asignar el valor de la 
            // propiedad del traslado horizontal del rectángulo por la animación de transformación
            Storyboard.SetTargetName(animacionTrasladoHorizontal, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(animacionTrasladoHorizontal,
                new PropertyPath(TranslateTransform.XProperty));

            // Crear la animación que orienta la posicion vertical del rectángulo
            // en función del punto vertical del segmento del camino en el punto
            // del trayecto en el que se encuentre
            DoubleAnimationUsingPath animacionTrasladoVertical =
                new DoubleAnimationUsingPath();
            animacionTrasladoVertical.PathGeometry = caminoAnimacion;
            animacionTrasladoVertical.Duration = TimeSpan.FromSeconds(5);

            // El origen del valor de la animación en un punto 
            // es el valor de y del camino en dicho punto
            animacionTrasladoVertical.Source = PathAnimationSource.Y;


            // Registrar la animacion para asignar el valor de la 
            // propiedad del traslado vertical del rectángulo por la animación de transformación
            Storyboard.SetTargetName(animacionTrasladoVertical, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(animacionTrasladoVertical,
                new PropertyPath(TranslateTransform.YProperty));

            // Create a Storyboard to contain and apply the animations.

            // crear el StoryBoard que contiene las animaciones y las aplica
            Storyboard animacion = new Storyboard();
            animacion.RepeatBehavior = RepeatBehavior.Forever;
            animacion.AutoReverse = true;
            animacion.Children.Add(animacionAngulo);
            animacion.Children.Add(animacionTrasladoHorizontal);
            animacion.Children.Add(animacionTrasladoVertical);

            // iniciar la animación
            animacion.Begin(this);
        }
    }
}
