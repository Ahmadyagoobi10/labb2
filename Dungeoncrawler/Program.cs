
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;
using Dungeoncrawler.Data;
using Dungeoncrawler.Mechanics;

namespace Dungeoncrawler;



public abstract class LevelElement
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; protected set; }
    public ConsoleColor Color { get; protected set; }

    public LevelElement(int x, int y)
    {
        X = x;
        Y = y;
    }

    public virtual void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X, Y);
        Console.Write(Symbol);
        Console.ResetColor();
    }
}


public class Wall : LevelElement
{
    public Wall(int x, int y) : base(x, y)
    {
        Symbol = '#';
        Color = ConsoleColor.Gray;
    }
}


public class Player : LevelElement
{
    public int HP { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }
    public int AttackCount { get; set; } = 0;
    public int DefenceCount { get; set; } = 0;

    public Player(int x, int y) : base(x, y)
    {
        Symbol = '@';
        Color = ConsoleColor.Cyan;
        HP = 100;
        AttackDice = new Dice(2, 6, 2);
        DefenceDice = new Dice(2, 6, 0);
    }

    public bool IsDead => HP <= 0;
}


public class Dice
{
    private int num, sides, bonus;
    private static Random rnd = new Random();

    public Dice(int n, int s, int b)
    {
        num = n; sides = s; bonus = b;
    }

    public int Throw()
    {
        int total = bonus;
        for (int i = 0; i < num; i++)
            total += rnd.Next(1, sides + 1);
        return total;
    }

    public override string ToString()
    {
        return $"{num}d{sides}+{bonus}";
    }
}


public abstract class Enemy : LevelElement
{
    public string Name { get; protected set; }
    public int HP { get; set; }
    public Dice AttackDice { get; protected set; }
    public Dice DefenceDice { get; protected set; }
    public int AttackCount { get; set; } = 0;
    public int DefenceCount { get; set; } = 0;

    protected Enemy(int x, int y) : base(x, y) { }

    public abstract void Update(Player player, LevelData level);
    public bool IsDead => HP <= 0;
    public void DrawIfVisible(bool visible)
    {
        if (!visible) return;
        Draw();
    }

}


public class Rat : Enemy
{
    private static Random rnd = new Random();
    private static int counter = 1;

    public Rat(int x, int y) : base(x, y)
    {
        Symbol = 'r';
        Color = ConsoleColor.Red;
        Name = "Rat " + counter++;
        HP = 10;
        AttackDice = new Dice(1, 6, 3);
        DefenceDice = new Dice(1, 6, 1);
    }

    public override void Update(Player player, LevelData level)
    {
        if (IsDead) return;

        int dx = rnd.Next(-1, 2);
        int dy = rnd.Next(-1, 2);
        level.TryMoveEnemy(this, X + dx, Y + dy, player);
    }
}
public class Snake : Enemy
{
    private static int counter = 1;

    public Snake(int x, int y) : base(x, y)
    {
        Symbol = 's';
        Color = ConsoleColor.Green;
        Name = "Snake " + counter++;
        HP = 25;
        AttackDice = new Dice(3, 4, 2);
        DefenceDice = new Dice(1, 8, 5);
    }

    public override void Update(Player player, LevelData level)
    {
        if (IsDead) return;

        double dist = level.Distance(X, Y, player.X, player.Y);

        
        if (dist <= 2.0)
        {
            int dx = Math.Sign(X - player.X);
            int dy = Math.Sign(Y - player.Y);

            var directions = new List<(int nx, int ny)>
            {
                (X + dx, Y),
                (X, Y + dy),
                (X + dx, Y + dy),
                (X - dx, Y - dy),
                (X, Y)
            };

            foreach (var (nx, ny) in directions)
            {
                if (!level.IsBlockedAt(nx, ny) && !level.IsEnemyAt(nx, ny))
                {
                    X = nx;
                    Y = ny;
                    break;
                }
            }
        }

        
        if (dist <= 1.5)
        {
            Combat.Resolve(this, player);
        }
    }
}

public static class Combat
{
    public static void Resolve(LevelElement attacker, LevelElement defender)
    {
        if (attacker is Player p && defender is Enemy e)
            Fight(p, e);
        else if (attacker is Enemy en && defender is Player pl)
            Fight(pl, en);
    }

    internal static void Resolve(LevelElements.snake snake, LevelElements.Player player)
    {
        throw new NotImplementedException();
    }

    internal static void Resolve(LevelElements.Enemies.Snake snake, LevelElements.Player player)
    {
        throw new NotImplementedException();
    }

    private static void Fight(Player player, Enemy enemy)
    {
        int atk = player.AttackDice.Throw();
        int def = enemy.DefenceDice.Throw();
        int dmg = Math.Max(0, atk - def);
        enemy.HP -= dmg;
        player.AttackCount++;
        enemy.DefenceCount++;

        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Du attackerar {enemy.Name}: ATK {atk} vs DEF {def} → Skada: {dmg}      ");

        if (enemy.HP > 0)
        {
            atk = enemy.AttackDice.Throw();
            def = player.DefenceDice.Throw();
            dmg = Math.Max(0, atk - def);
            player.HP -= dmg;
            enemy.AttackCount++;
            player.DefenceCount++;
            Console.WriteLine($"{enemy.Name} attackerar dig: ATK {atk} vs DEF {def} → Skada: {dmg}      ");
        }
    }
}


public class LevelData
{
    public List<LevelElement> Elements { get; private set; } = new List<LevelElement>();
    public Player Player { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Load(string filename)
    {
        Elements.Clear();
        string[] lines = File.ReadAllLines(filename);
        Height = lines.Length;
        Width = lines.Max(l => l.Length);

        for (int y = 0; y < Height; y++)
        {
            string line = lines[y].PadRight(Width);
            for (int x = 0; x < Width; x++)
            {
                char c = line[x];
                switch (c)
                {
                    case '#': Elements.Add(new Wall(x, y)); break;
                    case 'r': Elements.Add(new Rat(x, y)); break;
                    case 's': Elements.Add(new Snake(x, y)); break;
                    case '@': Player = new Player(x, y); break;
                }
            }
        }

        if (Player == null)
            Player = new Player(1, 1); 
    }

    public bool IsBlockedAt(int x, int y)
    {
        return Elements.Any(e => e.X == x && e.Y == y && e is Wall);
    }

    public bool IsEnemyAt(int x, int y)
    {
        return Elements.Any(e => e.X == x && e.Y == y && e is Enemy);
    }

    public void TryMovePlayer(int x, int y)
    {
        var enemy = Elements.OfType<Enemy>().FirstOrDefault(e => e.X == x && e.Y == y);
        if (enemy != null)
        {
            Combat.Resolve(Player, enemy);
            if (enemy.HP <= 0)
            {
                Console.WriteLine($"{enemy.Name} dog!");
                Elements.Remove(enemy);
            }
            return;
        }

        if (!IsBlockedAt(x, y))
        {
            Player.X = x;
            Player.Y = y;
        }
    }

    public void TryMoveEnemy(Enemy enemy, int x, int y, Player player)
    {
        if (player.X == x && player.Y == y)
        {
            Combat.Resolve(enemy, player);
            return;
        }

        if (!IsBlockedAt(x, y) && !IsEnemyAt(x, y))
        {
            enemy.X = x;
            enemy.Y = y;
        }
    }

    public double Distance(int x1, int y1, int x2, int y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }

    internal void TryMoveEnemy(LevelElements.Enemies.Rat rat, int v1, int v2, LevelElements.Player player)
    {
        throw new NotImplementedException();
    }
}


  class Program
  {
    static void Main()
    {
        Console.CursorVisible = false;

        string path = Path.Combine(Environment.CurrentDirectory, "Level1.txt");
        if (!File.Exists(path))
        {
            Console.WriteLine($"Level1.txt hittades inte! Se till att filen finns i {path}");
            Console.ReadKey();
            return;
        }

        LevelData level = new LevelData();
        level.Load(path);

        Player player = level.Player;
        const int VISION = 5;

        
        bool[,] discovered = new bool[level.Width, level.Height];

        while (true)
        {
            Console.Clear();

            
            foreach (var wall in level.Elements.OfType<Wall>())
            {
                double dist = level.Distance(player.X, player.Y, wall.X, wall.Y);
                if (dist <= VISION)
                    discovered[wall.X, wall.Y] = true;

                if (discovered[wall.X, wall.Y])
                    wall.Draw();
            }

           
            foreach (var enemy in level.Elements.OfType<Enemy>())
            {
                double dist = level.Distance(player.X, player.Y, enemy.X, enemy.Y);
                if (dist <= VISION && !enemy.IsDead)
                    enemy.DrawIfVisible(true);
            }

            
            player.Draw();

           
            Console.SetCursorPosition(0, level.Height + 1);
            Console.Write($"Player HP: {player.HP}      ");

            
            int statusLine = level.Height + 2;
            foreach (var enemy in level.Elements.OfType<Enemy>())
            {
                if (!enemy.IsDead && statusLine < Console.WindowHeight)
                {
                    Console.SetCursorPosition(0, statusLine++);
                    Console.Write($"{enemy.Name} HP: {enemy.HP}  Attacks: {enemy.AttackCount}  Defences: {enemy.DefenceCount}      ");
                }
            }


            
            ConsoleKey key = Console.ReadKey(true).Key;
            int nx = player.X, ny = player.Y;

            switch (key)
            {
                case ConsoleKey.W: ny--; break;
                case ConsoleKey.S: ny++; break;
                case ConsoleKey.A: nx--; break;
                case ConsoleKey.D: nx++; break;
                case ConsoleKey.Escape: return;
            }

            
            level.TryMovePlayer(nx, ny);

            
            foreach (var enemy in level.Elements.OfType<Enemy>().ToList())
            {
                enemy.Update(player, level);
                if (enemy.IsDead)
                {
                    Console.SetCursorPosition(0, statusLine++);
                    Console.WriteLine($"{enemy.Name} dog!");
                    level.Elements.Remove(enemy);
                }
            }

            
            if (player.IsDead)
            {
                Console.Clear();
                Console.WriteLine("Game Over! Du dog!");
                break;
            }
        }
    }
  }





