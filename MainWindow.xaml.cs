using GraphicsPractice.Labs._1;
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

namespace GraphicsPractice
{
    /// <summary>
    /// Main window of the Project, we shall have a lot of projects in one!
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * Pick a lab to view
         */
        private void LabPick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Window window = null;
            switch (button.Name)
            {
                case "L1T1":
                    window = new Lab1Task1();
                    break;
                case "L1T2":
                    window = new Lab1Task2();
                    break;
                case "L1T3":
                    window = new Lab1Task3();
                    break;
                default:
                    break;
            }
            if (window != null)
            {
                window.Show();
            }
        }
    }
}
