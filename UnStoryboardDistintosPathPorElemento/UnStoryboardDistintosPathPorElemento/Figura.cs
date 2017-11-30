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

namespace UnStoryboardDistintosPathPorElemento
{
    // Figura es una clase que puede dibujarse y animarse a si misma en un canvas
    // El movimiento de la figura seguirá un camino generado con valores aleatorios
    public class Figura
    {
        Canvas canvas;
        Image imagen;
        Path camino;
        Random numero;

        int posicionVertical;
        int posicionHorizontal;

        // contenedor de animaciones de la figura
        Storyboard storyboard;

        // componentes horizontales y verticales de la animación que sigue el camino
        DoubleAnimationUsingPath animacionEjeX;
        DoubleAnimationUsingPath animacionEjeY;
        IGeneradorCamino generadorCamino;

        // Cuando se instancia una figura la misma se dibuja a si misma en el canvas 
        // para ello la imagen debe disponer de un nombre para identificarse
        // las coordenadas y las proporciones de la imagen
        public Figura(Canvas canvas, IGeneradorCamino genCamino, string nombre, string directorioImagen, int ancho, int alto, int posicionVertical, int posicionHorizontal)
        {
            this.canvas = canvas;
            generadorCamino = genCamino;

            this.posicionVertical = posicionVertical;
            this.posicionHorizontal = posicionHorizontal;

            imagen = new Image();
            imagen.Source = new BitmapImage(new Uri(directorioImagen, UriKind.Relative));
            imagen.Name = nombre;
            imagen.Width = ancho;
            imagen.Height = alto;

            DibujarFigura();
        }

        private void DibujarFigura()
        {
            Canvas.SetLeft(imagen, 0);
            Canvas.SetTop(imagen, 0);
            Canvas.SetZIndex(imagen, 10);
            canvas.Children.Add(imagen);
        }

        // Dibuja el camino que sigue la figura al moverse
        private void DibujarCamino(PathGeometry geometriaCamino)
        {
            Path nuevoCamino = new Path();
            nuevoCamino.Stroke = Brushes.Black;
            nuevoCamino.StrokeThickness = 3;
            nuevoCamino.Fill = Brushes.Blue;
            nuevoCamino.Data = geometriaCamino;

            camino = nuevoCamino;
            canvas.Children.Add(camino);
        }

        /// <summary>
        /// Genera y dibuja el camino que seguirá la figura 
        /// e inicia la animación de la figura sobre el camino generado
        /// </summary>
        /// <param name="direccion">Define desde donde inicia y en dónde finaliza la animación</param>
        /// <param name="duracion">Duración de la animación</param>
        /// <param name="animacionFinaliza">La animación puede finalizar o durar hasta siempre</param>
        public void AnimarFigura(Direccion direccion, int duracion, bool animacionFinaliza)
        {
            string nombreAnimacion = imagen.Name + "_animacion";

            // La animación de transformación es instanciada, registrada y asociada a la imagen 
            TranslateTransform animacionTranslateTransform = new TranslateTransform();
            canvas.RegisterName(nombreAnimacion, animacionTranslateTransform);
            imagen.RenderTransform = animacionTranslateTransform;

            // Obtiene un camino con forma de onda y orientada hacia la dirección del movimiento
            PathGeometry caminoOnda = generadorCamino.ObtenerCamino(direccion, 
                posicionVertical, 0, Convert.ToInt32(canvas.Width - 64));
            caminoOnda.Freeze();

            // Dibujar el camino obtenido en el canvas
            this.DibujarCamino(caminoOnda);

            // Crear la animación que mueve la figura horizontalmente
            animacionEjeX = new DoubleAnimationUsingPath();
            animacionEjeX.PathGeometry = caminoOnda;
            animacionEjeX.Duration = TimeSpan.FromSeconds(duracion);
            animacionEjeX.Source = PathAnimationSource.X; // movimiento sobre el eje X
            if (animacionFinaliza) { animacionEjeX.RepeatBehavior = RepeatBehavior.Forever; }

            // Asociar la animación para que oriente la propiedad X de
            // la animación de transformación
            Storyboard.SetTargetName(animacionEjeX, nombreAnimacion);
            Storyboard.SetTargetProperty(animacionEjeX,
                new PropertyPath(TranslateTransform.XProperty));

            // Crear la animación que mueve la figura verticalmente
            animacionEjeY = new DoubleAnimationUsingPath();
            animacionEjeY.PathGeometry = caminoOnda;
            animacionEjeY.Duration = TimeSpan.FromSeconds(duracion);
            animacionEjeY.Source = PathAnimationSource.Y; // movimiento sobre el eje Y
            if (animacionFinaliza) { animacionEjeY.RepeatBehavior = RepeatBehavior.Forever; }

            // Asociar la animacion para que oriente la propiedad Y de
            // la animación de transformación
            Storyboard.SetTargetName(animacionEjeY, nombreAnimacion);
            Storyboard.SetTargetProperty(animacionEjeY,
                new PropertyPath(TranslateTransform.YProperty));

            // Crear el Storyboard para contener y aplicar las animaciones
            storyboard = new Storyboard();
            //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Children.Add(animacionEjeX);
            storyboard.Children.Add(animacionEjeY);

            // Suscribir el procedimiento que atiende la finalización de la animación
            animacionEjeX.Completed += Figura_AnimacionCompletada;

            // Iniciar la animación
            storyboard.Begin(canvas);
        }

        /// <summary>
        /// Procedimiento que es utilizado para ser suscrito a la finalización de la animación
        /// para iniciarla de nuevo pero con un camino nuevo y hacia la dirección contraria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Figura_AnimacionCompletada(object sender, EventArgs e)
        {
            // Obtiene la posición horizontal de la figura luego de que su animación ha 
            // finalizado
            Point coordenadaFigura = imagen.PointToScreen(new Point(0, 0));
            Point coordenadaCanvas = canvas.PointFromScreen(new Point(0, 0));

            // Posicion de la figura en la ventana = posicion del canvas + 
            int posicionXAlien = Convert.ToInt32(coordenadaFigura.X) + Convert.ToInt32(coordenadaCanvas.X);
            bool moverHaciaDerecha = (posicionXAlien == 0);

            canvas.Children.Remove(camino);

            PathGeometry nuevoCamino;

            // Obtiene un camino con forma de onda y orientada hacia la dirección correcta
            if(moverHaciaDerecha)
            {
                nuevoCamino = generadorCamino.ObtenerCamino(Direccion.Derecha, posicionVertical, 0, Convert.ToInt32(canvas.Width - imagen.Width));
            }
            else
            {
                nuevoCamino = generadorCamino.ObtenerCamino(Direccion.Izquierda, posicionVertical, 0, Convert.ToInt32(canvas.Width - imagen.Width));
            }

            // Dibujar el camino en el canvas
            this.DibujarCamino(nuevoCamino);

            // El componente horizontal y vertical de la animación seguirá el nuevo camino generado
            animacionEjeX.PathGeometry = nuevoCamino;
            animacionEjeY.PathGeometry = nuevoCamino;

            // Iniciar la animación
            storyboard.Begin(canvas, true);
        }
        
    }
}
