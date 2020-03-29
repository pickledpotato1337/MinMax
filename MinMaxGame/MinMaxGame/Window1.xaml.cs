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
using System.Windows.Shapes;

namespace MinMaxGame
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void SelectPlayer1Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerConfig.Protagonist = true;
            PlayerConfig.gameTree.Init();
            this.Close();

        }

        private void SelectPlayer2Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerConfig.Protagonist = false;
            PlayerConfig.gameTree.Init();
            this.Close();
        }
    }
}
