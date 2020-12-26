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
using Pokemon;

namespace PokemonGame
{
    /// <summary>
    /// Battle.xaml 的互動邏輯
    /// </summary>
    public partial class Battle : Window
    {
        public Battle()
        {
            InitializeComponent();
            this.Loaded += Init;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //Player = player;
        }
        int Choose = 0;
        int chosePokemon1 = -1, chosePokemon2 = -1;
        private void Init(object sender, RoutedEventArgs e)
        {
            Button[] buttons = new Button[5];
            int i = 0;
            foreach (KeyValuePair<int, Pokemon.Pokemon> pokemon in Player.Instance.Pokemons)
            {
                buttons[i] = wrapPanel.Children[i] as Button;
                buttons[i].DataContext = pokemon.Key;
                buttons[i].Click += TextChange;
                buttons[i].IsEnabled = true;
                buttons[i].Content = Method.ImageChange(pokemon.Value.Type);
                i++;
            }
        }
        private void TextChange(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (Choose == 0)
            {
                chosePokemon1 = (int)button.DataContext;
                Method.TextChange(Pokemon1_TextBlock, chosePokemon1, Player.Instance);
                Choose++;
            }
            else if (Choose == 1)
            {
                chosePokemon2 = (int)button.DataContext;
                Method.TextChange(Pokemon2_TextBlock, chosePokemon2, Player.Instance);
                Choose--;
            }
        }

        private void StartBattle_Click(object sender, RoutedEventArgs e)
        {
            if (chosePokemon1 == -1 || chosePokemon2 == -1) MessageBox.Show("You have to choose two pokemons!");
            else if (chosePokemon1 == chosePokemon2) MessageBox.Show("You are not allowed to choose same pokemon!");
            else
            {
                BattleScene battleScene = new BattleScene(chosePokemon1, chosePokemon2);
                battleScene.ShowDialog();
            }
        }
    }
}
