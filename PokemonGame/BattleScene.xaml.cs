using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pokemon;

namespace PokemonGame
{
    /// <summary>
    /// BattleScene.xaml 的互動邏輯
    /// </summary>
    public partial class BattleScene : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int counter, playerPokemon, enemyPokemon, skill, choosingPokemon = -1, status = 0, choosingPotion = -1;
        public BattleScene(int p1, int p2)
        {
            InitializeComponent();
            this.Loaded += PotionInit;
            this.Loaded += SkillsInit;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            playerPokemon = p1;
            enemyPokemon = p2;
            Background.Source = new BitmapImage(new Uri("/Image/Background.png", UriKind.Relative));
            Pokemon1_Image.Source = new BitmapImage(new Uri("/Image/" + Player.Instance.Pokemons[p1].Type + ".png", UriKind.Relative));
            Pokemon2_Image.Source = new BitmapImage(new Uri("/Image/" + Player.Instance.Pokemons[p2].Type + ".png", UriKind.Relative));
            Pokemon1_Target.Content = Method.ImageChange(Player.Instance.Pokemons[playerPokemon].Type);
            Pokemon2_Target.Content = Method.ImageChange(Player.Instance.Pokemons[enemyPokemon].Type);
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
        private void SkillsInit(object sender, RoutedEventArgs e)
        {
            Button[] buttons = new Button[SkillsPanel.Children.Count];
            int i = 0;
            foreach (Skill skill in Player.Instance.Pokemons[playerPokemon].Skills)
            {
                buttons[i] = SkillsPanel.Children[i] as Button;
                buttons[i].Click += SkillsChoose;
                buttons[i].Content = skill.Name;
                buttons[i].IsEnabled = true;
                buttons[i].DataContext = i;
                i++;
            }
        }
        private void PotionStatus(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            PotionUse.Visibility = Visibility.Visible;
            choosingPotion = (int) button.DataContext;
            ChoosingStatus.Text = "Choosing Potion : ";
            ChoosingStatus.Text += Player.Instance.Potions[choosingPotion].Type ;
            if (Player.Instance.Potions[choosingPotion].Type == "HealingPotion") ChoosingStatus.Text += "\nIt can heal 50 hp";
            else ChoosingStatus.Text += "\nIt can deal 50 damage";
            if (choosingPokemon == -1) ChoosingStatus.Text += "\nCurrent no target!";
            else ChoosingStatus.Text += "\nCurrent target : " + Player.Instance.Pokemons[choosingPokemon].Type;
        }
        private void StartTimer_Click()
        {
            counter = 0;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(250);
            dispatcherTimer.Start();
            Attack(playerPokemon, choosingPokemon);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter == 1)
            {
                EnemyAttack();
                StopTimerButton_Click();
            }
        }
        private void StopTimerButton_Click()
        {
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Stop();
        }
        private void SkillsChoose(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            skill = (int)button.DataContext;
            PotionUse.Visibility = Visibility.Hidden;
            ChoosingStatusUpdate(skill, choosingPokemon);
        }
        private void ChoosingStatusUpdate(int Choosingskill, int ChoosingTarget)
        {
            string target;
            if (ChoosingTarget == playerPokemon) target = "Player";
            else if (ChoosingTarget == -1) target = "No target";
            else target = "Enemy";
            ChoosingStatus.Text = "Current Choosing skill : " + Player.Instance.Pokemons[playerPokemon].Skills[Choosingskill].Name
                                + "\nCurrent Choosing Pokemon : " + target;
        }
        private void Attack(int Source, int Target)
        {
            status++;
            if (status >= 6)
            {
                status %= 3;
                BattleState.Text = "";
            }
            if (Source == Target)
            {
                BattleState.Text += "Your pokemon cannot hit himself!\nYou passed this round!\n";
                return;
            }
            BattleState.Text += Player.Instance.Pokemons[Target].name + " got hit " + Player.Instance.Pokemons[Source].Skills[skill].Damage;
            BattleState.Text += "\nRemaining Hp " + Player.Instance.Pokemons[Target].HP + "\n";
            if (Target == enemyPokemon) Method.Shake(Pokemon2_Canvas);
            else Method.Shake(Pokemon1_Canvas);
            BattleCheck();
        }
        private void Pokemon1_Target_Click(object sender, RoutedEventArgs e)
        {
            choosingPokemon = playerPokemon;
            ChoosingStatusUpdate(skill, choosingPokemon);
        }
        private void PotionUse_Click(object sender, RoutedEventArgs e)
        {
            if (choosingPokemon == -1) MessageBox.Show("You have to choose a pokemon as target1");
            else
            {
                Button button = PotionsPanel.Children[choosingPotion] as Button;

                Player.Instance.Potions[choosingPotion].Using(Player.Instance, choosingPokemon);
                BattleState.Text += "You throw a " + Player.Instance.Potions[choosingPotion].Type + " to " + Player.Instance.Pokemons[choosingPokemon].Type;
                if (Player.Instance.Potions[choosingPotion].Type == "HealingPotion") BattleState.Text += "\nIt heal 50 hp";
                else BattleState.Text += "\nIt deal 50 damage";
                BattleState.Text += " to " + Player.Instance.Pokemons[choosingPokemon].Type + "\n";
                BattleState.Text += "Remaining hp : " + Player.Instance.Pokemons[choosingPokemon].HP + "\n";

                button.IsEnabled = false;
                button.Content = Method.ImageChange("");
                Player.Instance.Potions.Remove(choosingPotion);
                BattleCheck();
            }
        }
        private void Pokemon2_Target_Click(object sender, RoutedEventArgs e)
        {
            choosingPokemon = enemyPokemon;
            ChoosingStatusUpdate(skill, choosingPokemon);
        }
        private void Attack_Button_Click(object sender, RoutedEventArgs e)
        {
            if (choosingPokemon != -1 && Player.Instance.Pokemons[playerPokemon].Skills[skill] != null && skill != -1)
            {
                if (choosingPokemon != playerPokemon) Player.Instance.Pokemons[playerPokemon].Skills[skill].Using(Player.Instance, enemyPokemon);
                StartTimer_Click();
                
            }
            else MessageBox.Show("You have to choose a target and a skill everytimes!");
        }
        private void EnemyAttack()
        {
            if (Player.Instance.Pokemons[playerPokemon].HP > 0 && Player.Instance.Pokemons[enemyPokemon].HP > 0)
            {
                Player.Instance.Pokemons[enemyPokemon].Skills[0].Using(Player.Instance, playerPokemon);
                Attack(enemyPokemon, playerPokemon);
            }
        }
        private void BattleCheck()
        {
            bool end = false;
            int winner = 1;
            if (choosingPokemon != -1 && Player.Instance.Pokemons[enemyPokemon].HP <= 0)
            {
                MessageBox.Show("The winner is player");
                winner++;
                end = true;
            }
            else if (choosingPokemon != -1 && Player.Instance.Pokemons[playerPokemon].HP <= 0)
            {
                MessageBox.Show("The winner is AI");
                end = true;
            }
            if (end)
            {
                Reset(winner);
                this.Close();
            }
        }
        private void Reset(int winner)
        {
            Player.Instance.Pokemons[playerPokemon].EXP += winner * 15;
            Player.Instance.Pokemons[enemyPokemon].EXP += winner * 15;
            Player.Instance.Pokemons[playerPokemon].HP = Player.Instance.Pokemons[playerPokemon].FULLHP;
            Player.Instance.Pokemons[enemyPokemon].HP = Player.Instance.Pokemons[enemyPokemon].FULLHP;
        }
    }
}
