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

namespace MasDeUnStoryboardMismoElemento
{
    public partial class MainWindow : Window
    {
        Image alien;
        Path pathAlien;
        Random numero;

        Storyboard pathAnimationStoryboard;
        
        public MainWindow()
        {
            numero = new Random();
            InitializeComponent();
            DibujarFigura();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            AnimarAlien();
        }

        private void TranslateAnimation_Completed(object sender, EventArgs e)
        {
            this.pathAnimationStoryboard.Stop();
            this.pathAnimationStoryboard.Children.Clear();
            
            this.Canvas.Children.Remove(alien);
            this.Canvas.Children.Remove(pathAlien);

            AnimarAlien();
        }

        /// <summary>
        /// Animar a la imágen del alien siguiendo un camino con forma de onda
        /// </summary>
        private void AnimarAlien()
        {
            string nombreAnimacion = "AnimacionTranslateTransform_" + DateTime.Now.ToString("YYYYMMDDHHMMss") + "_" + numero.Next(0,100000).ToString();

            // La animación de transformación es instanciada, registrada y asociada a la imagen 
            TranslateTransform animacionTranslateTransform = new TranslateTransform();
            this.RegisterName(nombreAnimacion, animacionTranslateTransform);
            alien.RenderTransform = animacionTranslateTransform;

            // Obtiene un camino con forma de onda y orientada hacia la derecha
            PathGeometry caminoOnda = AgregarPuntosOndaCurvaBeizer(true);
            caminoOnda.Freeze();

            // Dibujar el camino en el canvas
            this.DibujarCamino(caminoOnda);

            // Crear la animación que mueve la figura horizontalmente
            DoubleAnimationUsingPath animacionEjeX =
                new DoubleAnimationUsingPath();
            animacionEjeX.PathGeometry = caminoOnda;
            //animacionEjeX.Duration = TimeSpan.FromSeconds(5);
            animacionEjeX.Source = PathAnimationSource.X; // movimiento sobre el eje X
            
            // Asociar la animación para que oriente la propiedad X de
            // la animación de transformación
            Storyboard.SetTargetName(animacionEjeX, nombreAnimacion);
            Storyboard.SetTargetProperty(animacionEjeX,
                new PropertyPath(TranslateTransform.XProperty));

            // Crear la animación que mueve la figura verticalmente
            DoubleAnimationUsingPath animacionEjeY =
                new DoubleAnimationUsingPath();
            animacionEjeY.PathGeometry = caminoOnda;
            //animacionEjeY.Duration = TimeSpan.FromSeconds(5);
            animacionEjeY.Source = PathAnimationSource.Y; // movimiento sobre el eje Y

            // Asociar la animacion para que oriente la propiedad Y de
            // la animación de transformación
            Storyboard.SetTargetName(animacionEjeY, nombreAnimacion);
            Storyboard.SetTargetProperty(animacionEjeY,
                new PropertyPath(TranslateTransform.YProperty));

            // Crear el Storyboard para contener y aplicar las animaciones
            pathAnimationStoryboard = new Storyboard();
            //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            pathAnimationStoryboard.Children.Add(animacionEjeX);
            pathAnimationStoryboard.Children.Add(animacionEjeY);

            // Suscribir el procedimiento que atiende la finalización de la animación
            animacionEjeX.Completed += TranslateAnimation_Completed;

            // Iniciar la animación
            pathAnimationStoryboard.Begin(this);
        }

        /// <summary>
        /// Agrega puntos a la curva beizer ingresada por parámetros para dibujar una onda 
        /// la cantidad de ciclos de la onda es estática pero el ancho de cada ciclo no es uniforme
        /// </summary>
        /// <param name="segmentoBezier"></param>
        /// <param name="direccionDerecha"></param>
        protected PathGeometry AgregarPuntosOndaCurvaBeizer(bool direccionDerecha)
        {
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            pFigure.StartPoint = new Point(0, 100);//todo...
            PolyBezierSegment segmentoBezier = new PolyBezierSegment();

            pFigure.Segments.Add(segmentoBezier);
            animationPath.Figures.Add(pFigure);

            // Determinar si inicia desde el extremo derecho o izquierdo de la pantalla
            int direccion = direccionDerecha ? 1 : -1;

            // la coordenada de Y en el punto inicial
            int puntoYCero = 100;

            // Determinar la coordenada de X en el punto inicial y el punto final
            // Determinar los puntos máximos y mínimos de Y en las dos partes de un ciclo
            int posicionInicial;
            int posicionFinal;
            int puntoExtremoYPrimerCiclo;
            int puntoExtremoYSegundoCiclo;

            if (direccionDerecha)
            {
                // Si la figura se desplazará hacia la derecha: 
                // La figura comenzará desde el valor máximo de X hasta el mínimo
                // El punto máximo de Y de la primera mitad del ciclo será menor que la de la segunda
                posicionInicial = 0;
                posicionFinal = Convert.ToInt32(this.Canvas.Width - 64);
                puntoExtremoYPrimerCiclo = 0;
                puntoExtremoYSegundoCiclo = 200;
            }
            else
            {
                posicionInicial = Convert.ToInt32(this.Canvas.Width - 64);
                posicionFinal = 0;
                puntoExtremoYPrimerCiclo = 200;
                puntoExtremoYSegundoCiclo = 0;
            }

            int cantCiclos = 3;
            int mitadCiclo;
            int distanciaPuntosCiclo = 0;
            int distanciaEntreDosPuntosCiclo = 0;

            int posicionXActual = posicionInicial;

            for (int i = 1; i <= cantCiclos; i++)
            {
                int anchoCiclo;

                // Si es el ultimo ciclo completo el ancho que falta para llenar hasta el punto máximo de X
                if (i == cantCiclos)
                {
                    if (direccionDerecha)
                    {
                        anchoCiclo = Convert.ToInt32(this.Canvas.Width - 64) - posicionXActual;
                    }
                    else
                    {
                        anchoCiclo = posicionXActual;
                    }
                }
                else
                {
                    // No es el ultimo ciclo el ancho del mismo es al azar
                    anchoCiclo = numero.Next(200, 330);
                }

                mitadCiclo = Convert.ToInt32(anchoCiclo / 2); // punto de inflexión
                distanciaPuntosCiclo = Convert.ToInt32(mitadCiclo / 3); // Distancia entre los puntos máximos y mínimos de Y
                distanciaEntreDosPuntosCiclo = mitadCiclo - (distanciaPuntosCiclo * 2);

                posicionXActual = posicionXActual + distanciaPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYPrimerCiclo));
                posicionXActual = posicionXActual + distanciaEntreDosPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYPrimerCiclo));
                posicionXActual = posicionXActual + distanciaPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoYCero));
                posicionXActual = posicionXActual + distanciaPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYSegundoCiclo));
                posicionXActual = posicionXActual + distanciaEntreDosPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoExtremoYSegundoCiclo));
                posicionXActual = posicionXActual + distanciaPuntosCiclo;
                segmentoBezier.Points.Add(new Point(posicionXActual, puntoYCero));
            }

            return animationPath;
        }

        public void DibujarFigura()
        {
            alien = new Image();
            alien.Source = new BitmapImage(new Uri(@"alien.png", UriKind.Relative));
            alien.Name = "alien";
            alien.Width = 64;
            alien.Height = 64;

            Canvas.SetLeft(alien, 0);
            Canvas.SetTop(alien, 0);
            Canvas.SetZIndex(alien, 10);
            this.Canvas.Children.Add(alien);
        }

        public void DibujarCamino(PathGeometry geometriaCamino)
        {
            Path camino = new Path();
            camino.Stroke = Brushes.Black;
            camino.StrokeThickness = 3;
            camino.Fill = Brushes.Blue;
            camino.Data = geometriaCamino;

            pathAlien = camino;
            this.Canvas.Children.Add(pathAlien);
        }

    }
}
