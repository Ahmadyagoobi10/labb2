
using System;
using Dungeoncrawler.LevelElements;          
using Dungeoncrawler.LevelElements.Enemies;  

namespace Dungeoncrawler.Mechanics
{
    public static class Combat
    {
        
        public static void Resolve(LevelElement attacker, LevelElement defender)
        {
            if (attacker is Player p && defender is Enemy e)
                Fight(p, e);
            else if (attacker is Enemy en && defender is Player pl)
                Fight(pl, en);
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
}
