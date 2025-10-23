
using System;
using System.Collections.Generic;
using Dungeoncrawler.LevelElements;  
using Dungeoncrawler.LevelElements.Enemies; 
using Dungeoncrawler.Data;           
using Dungeoncrawler.Mechanics;      

namespace Dungeoncrawler.LevelElements.Enemies
{
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
}
