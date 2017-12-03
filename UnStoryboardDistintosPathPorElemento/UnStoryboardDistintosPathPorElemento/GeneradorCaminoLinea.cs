using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UnStoryboardDistintosPathPorElemento
{
    public class GeneradorCaminoLinea : IGeneradorCamino
    {

        /// <summary>
        /// Genera un camino conformado por llineas que se mueven horizontalmente en ambas direcciones
        /// </summary>
        /// <param name="orientacion">Define dónde inicia y termina el camino</param>
        /// <param name="puntoNeutroY">Altura en donde se posicionará la mitad del camino</param>
        /// <param name="valorMinimoX">Punto minimo de X</param>
        /// <param name="valorMaximoX">Punto máximo de X</param>
        /// <returns></returns>
        public PathGeometry ObtenerCamino(Direccion orientacion, int puntoNeutroY, int valorMinimoX, int valorMaximoX)
        {
            PathGeometry camino = new PathGeometry();
            PathFigure caminoFigura = new PathFigure();

            // Valor del desplazamiento
            int direccion = (orientacion == Direccion.Derecha) ? 1 : -1;

            // El camino inicia desde puntos distintos dependiendo si la orientación es 
            // hacia la izquierda o hacia la derecha
            if (orientacion == Direccion.Derecha)
            {
                caminoFigura.StartPoint = new Point(valorMinimoX, puntoNeutroY);
            }
            else
            {
                caminoFigura.StartPoint = new Point(valorMaximoX, puntoNeutroY);
            }

            PolyLineSegment segmentoLineal = new PolyLineSegment();

            caminoFigura.Segments.Add(segmentoLineal);
            camino.Figures.Add(caminoFigura);

            // Determinar la coordenada de X en el punto inicial y el punto final
            // Determinar los puntos máximos y mínimos de Y en las dos partes de un ciclo
            int posicionInicial;
            int posicionFinal;
            int puntoExtremoYPrimerCiclo = puntoNeutroY - 100;
            int puntoExtremoYSegundoCiclo = puntoNeutroY + 100;

            if (orientacion == Direccion.Derecha)
            {
                // Si la figura se desplazará hacia la derecha: 
                // La figura comenzará desde el valor mínimo de X hasta el máximo
                posicionInicial = 0;
                posicionFinal = Convert.ToInt32(valorMaximoX);
            }
            else
            {
                posicionInicial = Convert.ToInt32(valorMaximoX);
                posicionFinal = 0;
            }

            int adelantoRetraso;
            int cantCiclos = 3;
            int posicionXActual = posicionInicial;
            
            Random numero = new Random();

            for (int i = 1; i <= cantCiclos; i++)
            {
                int anchoCiclo;

                // Si es el ultimo ciclo completo el ancho que falta para llenar hasta el punto máximo de X
                if (i == cantCiclos)
                {
                    if (orientacion == Direccion.Derecha)
                    {
                        anchoCiclo = Convert.ToInt32(valorMaximoX) - posicionXActual;
                    }
                    else
                    {
                        anchoCiclo = posicionXActual;
                    }
                }
                else
                {
                    // No es el ultimo ciclo el ancho del mismo es al azar
                    anchoCiclo = numero.Next(80, 360);
                }

                adelantoRetraso = Convert.ToInt32(numero.Next(-1, 1));

                posicionXActual = posicionXActual + anchoCiclo * direccion;
                segmentoLineal.Points.Add(new Point(posicionXActual, puntoNeutroY));

                if (adelantoRetraso != 0)
                {
                    segmentoLineal.Points.Add(new Point(posicionXActual + Convert.ToInt32(numero.Next(64,128)) * direccion * adelantoRetraso, puntoNeutroY));
                    segmentoLineal.Points.Add(new Point(posicionXActual, puntoNeutroY));
                }

            }
            return camino;
        }

    }
}
