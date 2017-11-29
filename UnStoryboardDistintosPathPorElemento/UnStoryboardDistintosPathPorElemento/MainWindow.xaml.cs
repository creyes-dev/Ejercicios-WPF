﻿using System;
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
            IGeneradorCamino camino = new GeneradorCaminoOndulado();
            Figura figura = new Figura(this.Canvas, camino, "alien", "alien.png", 64,64, 100, 0);
            figura.AnimarFigura(Direccion.Derecha, 3, false);
        }

    }
}
