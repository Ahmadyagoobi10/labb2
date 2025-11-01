/*
using System;
using Dungeoncrawler;
using Dungeoncrawler.Mechanics;
namespace Dungeoncrawler.LevelElements.Enemies;
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
*/
using Dungeoncrawler.Data;
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;
using Dungeoncrawler.Mechanics;
using System;

namespace Dungeoncrawler.LevelElements.Enemies
{
    public class Rat : Enemy
    {
        public Rat(int x, int y) : base(x, y)
        {
            Name = "Rat";
            HP = 10;
            AttackDice = new Dice(1, 6, 0);
            DefenceDice = new Dice(1, 4, 0);
            Symbol = 'r';
            Color = ConsoleColor.Red;
        }

        public override void Update(Player player, LevelData level)
        {
            
            int dx = player.X - X;
            int dy = player.Y - Y;

            if (Math.Abs(dx) + Math.Abs(dy) <= 5)
            {
                if (Math.Abs(dx) > Math.Abs(dy))
                    X += Math.Sign(dx);
                else
                    Y += Math.Sign(dy);
            }
        }
    }
}
