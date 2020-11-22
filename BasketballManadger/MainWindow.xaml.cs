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
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void btnShowMessage_Click(object sender, RoutedEventArgs e)
        {
            tbMessageShower.Text = "some text";
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
