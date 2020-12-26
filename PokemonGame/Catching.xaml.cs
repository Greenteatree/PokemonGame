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
    /// Interaction logic for Catching.xaml
    /// </summary>

    public static class CatchingGame
    {
        public static bool BallMoving;
        public static void BallThrowing(double By)
        {
            if (By < 0)
            {
                BallMoving = true;
            }
        }
        public static bool BallMove(double By)
        {
            if (By > 230)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool Touched(double Bx, double By, double Px, double Py)
        {
            if (((Bx - Px) < 20 && (Bx - Px) > -20 && ((By - Py) < 20) && (Py - By) > -20))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static class PokemonInForest
    {
        public static double PokemonMovngX(double Px)
        {
            Random random = new Random();
            double NewPx = Px + random.Next(-50, 51);
            if (Px >= 0 && Px <= 485)
            {
                return NewPx;
            }
            else
            {
                return Px;
            }
        }
        public static double PokemonMovngY(double Py)
        {
            Random random = new Random();
            double NewPy = Py + random.Next(-50, 51);
            if (Py >= 0 && Py <= 230)
            {
                return NewPy;
            }
            else
            {
                return Py;
            }
        }

        public static string PokemonType()
        {
            Random random = new Random();
            List<string> PokemonType = new List<string>() { "Pikachu.png", "Charmander.png", "Squirtle.png", "Bulbasaur.png", "Eevee.png" };
            return PokemonType[random.Next(0, 5)];
        }

    }

    public static class BallPosition
    {
        public static double GetBallPositionY(double BallBottom)
        {
            return BallBottom + 20;
        }
        public static double GetBallPositionX(double BallLeft)
        {
            return BallLeft + 20;
        }
        public static double SetBallPositionY(double PositionX)
        {
            return PositionX - 20;
        }
        public static double SetBallPositionX(double PositionY)
        {
            return PositionY - 20;
        }
    }



    public partial class Catching : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public Catching()
        {
            InitializeComponent();
            Inti();
        }
        public int counter = 10;
        static string thisPokemon = PokemonDatabase.GetPokemonType();
        string ThisPokemon = thisPokemon + ".png";
        public void Inti()
        {
            Canvas.SetLeft(NormalBall, 270);
            Canvas.SetTop(NormalBall, 248);
            CatchingGame.BallMoving = true;

            Pokemon_Img.Source = new BitmapImage(new Uri("/Image/" + ThisPokemon, UriKind.Relative));
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();

        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Stop();
            this.Close();
        }

        private void ForestCanvanClicked(object sender, MouseButtonEventArgs e)
        {
            CatchingGame.BallMoving = false;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Canvas.SetLeft(Pokemon_Img, PokemonInForest.PokemonMovngX(Canvas.GetLeft(Pokemon_Img)));
            Canvas.SetTop(Pokemon_Img, PokemonInForest.PokemonMovngY(Canvas.GetTop(Pokemon_Img)));
            ForestLocation.Text = "x:" + PokemonInForest.PokemonMovngX(Canvas.GetLeft(Pokemon_Img)) + ", y:" + PokemonInForest.PokemonMovngX(Canvas.GetTop(Pokemon_Img));
        }

        private void ForestCanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(ForestCanvas);
            if (CatchingGame.BallMoving == true)
            {

                if (CatchingGame.BallMove(p.Y))
                {
                    Canvas.SetLeft(NormalBall, BallPosition.SetBallPositionX(p.X));
                    Canvas.SetTop(NormalBall, BallPosition.SetBallPositionY(p.Y));
                }
            }
            else
            {
                Canvas.SetTop(NormalBall, Canvas.GetTop(NormalBall) - 10);
                CatchingGame.BallThrowing(BallPosition.SetBallPositionY(Canvas.GetTop(NormalBall)));
            }
            if (CatchingGame.Touched(Canvas.GetLeft(NormalBall), Canvas.GetTop(NormalBall), Canvas.GetLeft(Pokemon_Img), Canvas.GetTop(Pokemon_Img)))
            {
                MessageBox.Show("You got a " + ThisPokemon);
                PokemonFactory.Create(thisPokemon);
                this.Close();
            }
        }
    }
}
