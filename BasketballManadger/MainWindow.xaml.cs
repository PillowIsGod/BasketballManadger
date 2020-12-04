﻿using System;
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
        private BindingList<Teams> _teamsList;
        public MainWindow()
        {
            InitializeComponent();
            var teams = JsonPath.GetTeams();
            var players = JsonPath.GetBasketballPlayers();
            foreach (var item in teams)
            {
                item.BasketballPlayers = players;
            }
            var basketballPlayer = new BasketballPlayers();
            foreach (var item in teams)
            {
                item.BasketballPlayers = basketballPlayer.RelatePlayerToATeam(item, players); ;
            }
            _teamsList = teams;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lvTeamsOutput.ItemsSource = _teamsList;
        }

        private void Button_ShowPlayers(object sender, RoutedEventArgs e)
        {
           var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            lvPlayers.ItemsSource = players;
        }



        //private void _teamsList_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
        //    {
        //        try
        //        {
        //            JsonPath.SaveData(sender);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            Close();
        //        }
        //    }



    }
}
