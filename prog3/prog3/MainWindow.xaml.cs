using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prog3
{
/*
 * CS 212 - Program 3
 * Professor: Harry Plantinga
 * Author: Zach Wibbenmeyer
 * Date: November 4, 2016
 */       
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // set the background to an image of dirt
            canvas.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/dirt.jpg")));
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }
    }

}
