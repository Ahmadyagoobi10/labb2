
using Dungeoncrawler.LevelElements;       
using Dungeoncrawler.LevelElements.Enemies; 
using Dungeoncrawler.Mechanics;            


namespace Dungeoncrawler.Data
{
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

        public bool IsBlockedAt(int x, int y) =>
            Elements.Any(e => e.X == x && e.Y == y && e is Wall);

        public bool IsEnemyAt(int x, int y) =>
            Elements.Any(e => e.X == x && e.Y == y && e is Enemy);


        public void TryMovePlayer(int x, int y)
        {
            var enemy = Elements.OfType<Enemy>().FirstOrDefault(e => e.X == x && e.Y == y);
            if (enemy != null)
            {
                Combat.Resolve(Player, enemy);
                if (enemy.HP <= 0) Elements.Remove(enemy);
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

        public double Distance(int x1, int y1, int x2, int y2) =>
            Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }
}
