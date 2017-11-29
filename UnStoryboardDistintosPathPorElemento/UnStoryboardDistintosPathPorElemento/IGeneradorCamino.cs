using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UnStoryboardDistintosPathPorElemento
{
    public interface IGeneradorCamino
    {
        PathGeometry ObtenerCamino(Direccion orientacion, int puntoNeutroY, int valorMinimoX, int valorMaximoX);
    }
}
