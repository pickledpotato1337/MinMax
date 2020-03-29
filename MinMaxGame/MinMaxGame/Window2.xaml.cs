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
    /// Logika interakcji dla klasy Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            TreePrintBox.Clear();
            PlayerConfig.Protagonist = true;
            PlayerConfig.gameTree.Init();
            string output=PlayerConfig.gameTree.LevelOrder();
            TreePrintBox.Text = output;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
