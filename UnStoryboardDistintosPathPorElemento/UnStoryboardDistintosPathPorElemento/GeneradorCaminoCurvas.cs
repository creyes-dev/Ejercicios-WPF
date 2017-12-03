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
    public class GeneradorCaminoCurvas : IGeneradorCamino
    {
        /// <summary>
        /// Genera un camino conformado por curvas beizer para dibujar una onda 
        /// La cantidad de ciclos de la onda es constante pero el ancho del ciclo no es uniforme
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

            PolyBezierSegment segmentoBezier = new PolyBezierSegment();
            caminoFigura.Segments.Add(segmentoBezier);
            camino.Figures.Add(caminoFigura);

            // Determinar la coordenada de X en el punto inicial y el punto final
            // Determinar los puntos máximos y mínimos de Y en las dos partes de un ciclo
            int posicionInicial;
            int posicionFinal;
            int posicionMaximaY = puntoNeutroY + 100;
            int posicionMinimaY = puntoNeutroY - 100;

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

            int cantCiclos = 4;

            int posicionXActual = posicionInicial;
            int puntoPosicionY = puntoNeutroY;
            int puntoPosicionX = posicionInicial;
            int posicioncitaY = puntoNeutroY;
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
                    anchoCiclo = numero.Next(200, 330);
                }

                // PosicionXActual es la posicon del ultimo ciclo procesado la posicion actual es la siguiente:
                int posicioncita = posicionXActual;
                
                // Generar puntos intermedios
                while (posicioncita != posicionXActual + (anchoCiclo * direccion))
                {
                    if (direccion == 1)
                    {
                        posicioncita = numero.Next(posicioncita + 60,
                                                    posicioncita + 120);
                    }
                    else
                    {
                        posicioncita = numero.Next(posicioncita - 120,
                                                    posicioncita - 60);
                    }

                    posicioncitaY = numero.Next(posicioncitaY - 80, posicioncitaY + 80);

                    if(((posicioncita > posicionXActual + anchoCiclo) && (direccion == 1)) || 
                        ((posicioncita < posicionXActual - anchoCiclo) && (direccion == -1)))
                    {
                        posicioncita = posicionXActual + anchoCiclo * direccion;
                    }

                    if (posicioncitaY > posicionMaximaY)
                    {
                        posicioncitaY = posicionMaximaY;
                    }

                    if(posicioncitaY < posicionMinimaY)
                    {
                        posicioncitaY = posicionMinimaY;
                    }

                    segmentoBezier.Points.Add(new Point(posicioncita, posicioncitaY));
                }

                posicionXActual = posicioncita;
            }
            return camino;
        }

    }
}
