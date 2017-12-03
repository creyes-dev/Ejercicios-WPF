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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Crear una figura y animarla
            IGeneradorCamino genCaminosOndulados = new GeneradorCaminoOndulado();
            IGeneradorCamino genCaminoZigZag = new GeneradorCaminoZigZag();
            IGeneradorCamino genCaminoLinea = new GeneradorCaminoLinea();
            IGeneradorCamino genCaminoCurvas = new GeneradorCaminoCurvas();

            Figura alien = new Figura(this.Canvas, genCaminosOndulados, "alien", "alien.png", 64, 64, 100, 0);
            alien.AnimarFigura(Direccion.Derecha, 3, false);

            Figura alien3 = new Figura(this.Canvas, genCaminoLinea, "car", "car-loading.PNG", 64, 64, 100, 0);
            alien3.AnimarFigura(Direccion.Derecha, 3, false);

            Figura galleta = new Figura(this.Canvas, genCaminoZigZag, "galleta", "galleta.png", 64, 64, 300, 0);
            galleta.AnimarFigura(Direccion.Derecha, 5, false);

            Figura chromium = new Figura(this.Canvas, genCaminoCurvas, "chromium", "Chromium.png", 64, 64, 500, 0);
            chromium.AnimarFigura(Direccion.Derecha, 3, false);


        }

    }
}
