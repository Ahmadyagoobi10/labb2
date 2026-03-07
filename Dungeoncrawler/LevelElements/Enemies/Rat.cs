
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
