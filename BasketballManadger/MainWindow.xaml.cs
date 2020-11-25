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

namespace BasketballManadger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static JsonFileProcessing JsonPath = new JsonFileProcessing(@"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\content.json");
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void btnShowMessage_Click(object sender, RoutedEventArgs e)
        {
            List<Teams> players = JsonPath.GetTeams();
            tbMessageShower.Text = "";
        }

       



        private void tb_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = Brushes.DarkRed;
        }

        private void tb_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = Brushes.Red;
        }


        


       
    }
}
