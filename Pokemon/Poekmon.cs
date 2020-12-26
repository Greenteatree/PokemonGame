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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Pokemon
{
    public class LinkedListWithInit<T> : LinkedList<T>
    {
        public void Add(T item)
        {
            ((ICollection<T>)this).Add(item);
        }
    }
    public static class PokemonDatabase
    {
        private static Random random = new Random();
        public static string[] SkillsName = new string[] { "Bite", "Body Slam", "Double Hit", "Double Slap", "Comet Punch" };
        public static Dictionary<string, int> SkillsDamage = new Dictionary<string, int>{
                                                        { "Bite", 15 },
                                                        { "Body Slam", 25 },
                                                        { "Double Hit", 20 },
                                                        { "Double Slap", 20 },
                                                        { "Comet Punch", 35 }
                                                        };
        private static string[] PokemonType = new string[] { "Pikachu", "Eevee", "Squirtle", "Charmander", "Bulbasaur" };
        private static Dictionary<string, LinkedList<string>> PokemonTypeTable = new Dictionary<string, LinkedList<string>>() 
        {
            { "Pikachu", new LinkedListWithInit<string>{ "Pikachu", "Raichu"} },
            { "Eevee", new LinkedListWithInit<string>{ "Eevee", "Flareon" } },
            { "Squirtle", new LinkedListWithInit<string>{ "Squirtle", "Blastoise" } },
            { "Charmander", new LinkedListWithInit<string>{ "Charmander", "Charizard" } },
            { "Bulbasaur", new LinkedListWithInit<string>{ "Bulbasaur", "Ivysaur" } },
        };
        public static string GetPokemonType()
        {
            return PokemonType[random.Next(PokemonType.Length)];
        }
        public static string GetPokemonSkill()
        {
            return SkillsName[random.Next(SkillsName.Length)];
        }
        public static string GetPokemonEvolveType(string type)
        {
            if (PokemonTypeTable.ContainsKey(type) == true)
                return PokemonTypeTable[type].Find(type).Next.Value;
            else
                return "";
        }
    }
    public class Player
    {
        public int money;
        Dictionary<int, Pokemon> pokemons = new Dictionary<int, Pokemon>();
        Dictionary<int, Potion> potions = new Dictionary<int, Potion>();
        private int pokemonsLimit = 5;
        private int potionsLimit = 5;
        public int PokemonsLimit
        {
            get { return pokemonsLimit; }
        }
        public int PotionsLimit
        {
            get { return potionsLimit; }
        }
        public Dictionary<int, Pokemon> Pokemons
        {
            get { return pokemons; }
            set { this.pokemons = value; }
        }
        public Dictionary<int, Potion> Potions
        {
            get { return potions; }
            set { this.potions = value; }
        }
        
        private Player() { }
        static private Player instance;
        static public Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player();
                return instance;
            }
        }

    }
    public class Pokemon
    {
        public string name;
        private string type = "Pokemon";
        private int skillSlots = 4;
        public string Type
        {
            get { return this.type; }
            set { type = value; }
        }
        List<Skill> skills = new List<Skill>(4);
        public int SkillSlots
        {
            get { return skillSlots; }
        }
        public List<Skill> Skills
        {
            get { return skills; }
            set { this.skills = value; }
        }
        int cp, exp, hp, fullhp = 0;
        public int CP
        {
            get
            {
                return cp;
            }
            set
            {
                this.cp = value;
            }
        }
        public int EXP { get { return this.exp; } set { this.exp = value; } }
        public int HP
        {
            get { return this.hp; }
            set { this.hp = value; }
        }
        public int FULLHP { get { return fullhp; } set { this.hp = this.fullhp = value; } }
        public Pokemon()
        {
            Random rand = new Random();
            this.cp = rand.Next(5, 10);
            this.fullhp = this.cp * 10;
            this.hp = fullhp;
            this.type = "Pikachu";
            this.exp = 0;
        }
        public Pokemon(int pCp)
        {
            this.cp = pCp;
            this.fullhp = pCp * 10;
            this.hp = fullhp;
            this.exp = 0;
            this.type = PokemonDatabase.GetPokemonType();
            this.name = type;
        }
        public Pokemon(int pCp, string pType)
        {
            this.cp = pCp;
            this.fullhp = pCp * 10;
            this.hp = fullhp;
            this.exp = 0;
            this.type = pType;
            this.name = type;
        }

    }
    public interface IUSING
    {
        void Using(Player player, int target);
        bool Check(Player player, int TargetPokemon);
    }
    public class Skill : IUSING
    {
        string name;
        int damage;
        static Random rand = new Random();

        int i = rand.Next(PokemonDatabase.SkillsName.Length);
        public int Damage
        {
            get { return damage; }
        }
        public string Name
        {
            get { return name; }
        }
        public Skill()
        {
            var s = PokemonDatabase.SkillsName[i];
            damage = PokemonDatabase.SkillsDamage[s];
            this.name = s;
        }

        public void Using(Player player, int TargetPokemon)
        {
            if (Check(player, TargetPokemon))
            {
                player.Pokemons[TargetPokemon].HP -= this.damage;
            }
            else return;//job stop
        }
        public bool Check(Player player, int TargetPokemon)
        {
            if (player.Pokemons[TargetPokemon] == null) return false;
            else return true;
        }
    }
    public class Potion : IUSING
    {
        string type;
        int effect = 50;
        public string Type
        {
            get { return type; }
        }
        public Potion(int ptype)
        {
            if (ptype%2 == 0)
            {
                this.type = "HealingPotion";
            }
            else if (ptype%2 == 1)
            {
                this.type = "DamagePotion";
                effect *= -1;
            }
        }
        //required the using end sure the potion exist 
        public void Using(Player player, int TargetPokemon)
        {
            if (Check(player, TargetPokemon))
            {
                player.Pokemons[TargetPokemon].HP += this.effect;
            }
            else return;//job stop
        }
        public bool Check(Player player, int TargetPokemon)
        {
            if (player.Pokemons[TargetPokemon] == null) return false;
            else return true;
        }

    }
    public static class PotionFactory
    {
        static Random rnd = new Random();
        static int UID = 0;
        public static void Create()
        {
            if (Player.Instance.Potions.Count < Player.Instance.PotionsLimit)
            {
                Player.Instance.Potions.Add(UID, new Potion(rnd.Next(1, 11)));
                UID++;
                return;
            }
            MessageBox.Show("You have no more Potion slot!");
            return;
        }
    }
    public static class PokemonFactory 
    {
        static Random rnd = new Random();
        static int UID = 0;
        public static void Create()
        {
            if (Player.Instance.Pokemons.Count < Player.Instance.PokemonsLimit)
            {
                Player.Instance.Pokemons.Add(UID, new Pokemon(rnd.Next(1, 11)));
                Player.Instance.Pokemons[UID].Skills.Add(new Skill());
                UID++;
                return ;
            }                
            MessageBox.Show("You have no more pokemon ball!");
            return ;
        }

        public static void Create(string type)
        {
            if (Player.Instance.Pokemons.Count < Player.Instance.PokemonsLimit)
            {
                Player.Instance.Pokemons.Add(UID, new Pokemon(rnd.Next(1, 11), type));
                Player.Instance.Pokemons[UID].Skills.Add(new Skill());
                UID++;
                return;
            }
            MessageBox.Show("You have no more pokemon ball!");
            return;
        }
        public static void Evolve(int uid)
        {
            if (Player.Instance.Pokemons.TryGetValue(uid, out Pokemon pokemon) == true && pokemon.EXP >= 30 && PokemonDatabase.GetPokemonEvolveType(pokemon.Type) != "")
            {
                pokemon.Type = PokemonDatabase.GetPokemonEvolveType(pokemon.Type);
                pokemon.CP *= 2;
                pokemon.FULLHP *= 2;
                pokemon.name = pokemon.Type;
                pokemon.EXP -= 30;
                MessageBox.Show("It evolve to " + pokemon.Type + "!!");
            }
            else
            {
                MessageBox.Show("Cannot evolve anymore!!");
            }
        }
    }

    public static class Method
    {
        public static Viewbox ImageChange(string Target)
        {
            string Path = "/Image/";
            Target += ".png";
            Viewbox vb = new Viewbox();
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(Path + Target, UriKind.Relative))
            };
            vb.Child = image;
            return vb;
        }
        public static void TextChange(TextBlock textBlock, int Target, Player player)
        {
            textBlock.Text = "Pokemon : \n" + player.Pokemons[Target].name + "\n";
            textBlock.Text += "CP : " + player.Pokemons[Target].CP + "\n";
            textBlock.Text += "HP : " + player.Pokemons[Target].HP + "\n";
            textBlock.Text += "EXP : " + player.Pokemons[Target].EXP + "\n";
            //textBlock.Text += "Target : " + Target + "\n";//For debug
        }
        public static void Shake(dynamic obj)
        {
            DoubleAnimation shake = new DoubleAnimation()
            {
                From = 0,
                To = 15,
                RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 0, 0, 150)),
                AutoReverse = true,
                SpeedRatio = 25
            };
            try
            {
                obj.RenderTransform = new TranslateTransform();
                obj.RenderTransform.BeginAnimation(TranslateTransform.XProperty, shake);
            }
            catch (Exception)
            {

            }

        }

        public static void Gift(Player player)
        {
            PotionFactory.Create();
            foreach (KeyValuePair<int, Pokemon> pokemon in player.Pokemons)
            {
                if (pokemon.Value.Skills.Count < pokemon.Value.SkillSlots) pokemon.Value.Skills.Add(new Skill());
            }
        }

        public static string CheckLocaton(double lx, double ly)
        {
            if (ly > -2 && ly < 328 && lx > -6 && lx < 386)
            {
                if (lx == 226 && ly == 228)
                {
                    return "gym battle";
                }
                if (ly > 38 && ly < 118 && lx > 66 && lx < 136)
                {
                    Random random = new Random();
                    if (random.Next(1, 20) < 3)
                    {
                        return "found pokemon";
                    }
                }
                if (lx > 76 && lx < 126 && ly > 218 && ly < 268)
                { 
                    return "Landmark";
                }
                return "OK";
            }
            else
            {
                return "Crash";
            }
        }

    }
}