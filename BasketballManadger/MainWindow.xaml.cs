using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private void Window_Loaded (object sender, RoutedEventArgs e)
        {
            var teams = JsonPath.GetTeams();
            var players = JsonPath.GetBasketballPlayers();
            foreach (var item in teams)
            {
                item.BasketballPlayers = players;
            }
            var basketballPlayer = new BasketballPlayers();
            var playersToRelate = new List<BasketballPlayers>();
            foreach (var item in teams)
            {
                playersToRelate = basketballPlayer.RelatePlayerToATeam(item, players);
                item.BasketballPlayers = playersToRelate;
                item.Players.AddRange(playersToRelate.Select(x => x.Name).ToList());
            }
            dgTeams.ItemsSource = teams;

        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        private void btnShowPlayers_Click(object sender, RoutedEventArgs e)
        {
            var players = JsonPath.GetBasketballPlayers();
            var teams = JsonPath.GetTeams();
            var basketballPlayer = new BasketballPlayers();
            foreach (var item in teams)
            {
               basketballPlayer.RelatePlayerToATeam(item, players);
            }
        }
    }
}
