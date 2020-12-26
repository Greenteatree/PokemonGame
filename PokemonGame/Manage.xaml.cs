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
    /// Manage.xaml 的互動邏輯
    /// </summary>
    public partial class Manage : Window
    {
        private int currentTarget = -1;
        private bool isPokemon = true;
        public Manage()
        {
            InitializeComponent();
            this.Loaded += PokemonInit;
            this.Loaded += PotionInit;
            NameButton.Click += TextUpdate;
            ManageBackground.Source = new BitmapImage(new Uri("/Image/ManageBackground.png", UriKind.Relative));
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        
        private void PokemonInit(object sender, RoutedEventArgs e)
        {
            Button[] buttons = new Button[PokemonsPanel.Children.Count];
            int i = 0;
            foreach (KeyValuePair<int, Pokemon.Pokemon> pokemon in Player.Instance.Pokemons)
            {
                buttons[i] = PokemonsPanel.Children[i] as Button;
                buttons[i].DataContext = pokemon.Key;
                buttons[i].Click += TextUpdate;
                buttons[i].Content = Method.ImageChange(pokemon.Value.Type);
                buttons[i].IsEnabled = true;
                i++;
            }

        }
        private void PotionInit(object sender, RoutedEventArgs e)
        {
            Button[] buttons = new Button[PotionsPanel.Children.Count];
            int i = 0;
            foreach (KeyValuePair<int, Potion> potion in Player.Instance.Potions)
            {
                buttons[i] = PotionsPanel.Children[i] as Button;
                buttons[i].Click += PotionStatus;
                buttons[i].Content = Method.ImageChange(potion.Value.Type);
                buttons[i].IsEnabled = true;
                buttons[i].DataContext = potion.Key;
                i++;
            }
        }

        private void PotionStatus(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int target = (int)button.DataContext;
            isPokemon = false;
            Textblock.Text = "This is a " + Player.Instance.Potions[target].Type;
        }

        private void TextUpdate(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentTarget = (int) button.DataContext;
            isPokemon = true;
            if (Check(currentTarget))
                Method.TextChange(Textblock, currentTarget, Player.Instance);
        }

        private bool Check(int targetPokemon)
        {
            if (targetPokemon > -1 &&  Player.Instance.Pokemons[targetPokemon] == null)
            {
                MessageBox.Show("You dont have it!");
                return false;
            }
            return targetPokemon > -1;
        }
        private void Done_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Name_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            button.DataContext = currentTarget;
            if (Check(currentTarget) == false || NameBox.Text == "")
            {
                MessageBox.Show("Please choose a pokemon and type a new name");
            }
            else
            {
                Player.Instance.Pokemons[currentTarget].name = NameBox.Text;
                NameBox.Text = "";
            }

        }
        private void Sell(Player player, int target)
        {
            if (isPokemon == true)
            {
                Button button = PokemonsPanel.Children[target] as Button;
                button.Content = "EmptyPokemon" + target;
                button.IsEnabled = false;
                Player.Instance.Pokemons.Remove(target);
                player.money += 50;
                Textblock.Text = "Your current money = " + player.money;
            }
            else
            {
                Button button = PotionsPanel.Children[target] as Button;
                button.Content = "EmptyPotionSlot";
                button.IsEnabled = false;
                Player.Instance.Potions.Remove(target);
                player.money += 30;
                Textblock.Text = "Your current money = " + player.money;
            }
            currentTarget = -1;
        }
        private void Sell_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Check(currentTarget))
                Sell(Player.Instance, currentTarget);
        }
        private void Evolve_Button_Click(object sender, RoutedEventArgs e)
        {
            Evolve(currentTarget);
        }
        private void Evolve(int targetPokemon)
        {
            if (Check(targetPokemon))
            {
                PokemonFactory.Evolve(targetPokemon);
                for (int i = 0; i < PokemonsPanel.Children.Count; i++)
                {
                    Button button = PokemonsPanel.Children[i] as Button;
                    if (button.DataContext != null && (int)button.DataContext == targetPokemon) button.Content = Method.ImageChange(Player.Instance.Pokemons[targetPokemon].Type);
                }
                Method.TextChange(Textblock, targetPokemon, Player.Instance);
            }
            else MessageBox.Show("You have to choose one pokemon and it have 30 exp!");
        }
        private void Powerup_Button_Click(object sender, RoutedEventArgs e)
        {
            Powerup(currentTarget);
        }
        private void Powerup(int targetPokemon)
        {
            if (Check(targetPokemon) && Player.Instance.Pokemons[targetPokemon].EXP >= 15)
            {
                Player.Instance.Pokemons[targetPokemon].FULLHP += 20;
                Player.Instance.Pokemons[targetPokemon].EXP -= 15;
                Method.TextChange(Textblock, targetPokemon, Player.Instance);
            }
            else MessageBox.Show("You have to choose one pokemon and it have 15 exp!");
        }
    }
}
