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
using Pokemon;

namespace PokemonGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player player = Player.Instance;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ManageButton.Content = Method.ImageChange("NormalBall");
            
        }

        public void Location()
        {
            ShowLocation.Text = "x:" + Canvas.GetLeft(playerimg) + ", y:" + Canvas.GetTop(playerimg);
        }

        

        private void ManageButton_Click(object sender, RoutedEventArgs e)
        {
            Manage m = new Manage();
            m.ShowDialog();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            PokemonFactory.Create();
            foreach (KeyValuePair<int, Pokemon.Pokemon> pokemon in Player.Instance.Pokemons)
                pokemon.Value.EXP += 50;
        }

        private void PotionTest_Click(object sender, RoutedEventArgs e)
        {
            PotionFactory.Create();
        }

        private void SercetGift_Click(object sender, RoutedEventArgs e)
        {
            Method.Gift(player);
            MessageBox.Show("You get a potion!\nYour pokemons get a new skill!");
        }
    }
}
