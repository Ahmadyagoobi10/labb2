using System;
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;

namespace Dungeoncrawler.Mechanics
{
    public static class Combat
    {
    
        public static (int atk, int def, int dmg) PlayerAttack(Player player, Enemy enemy)
        {
            int atk = player.AttackDice.Throw();
            int def = enemy.DefenceDice.Throw();
            int dmg = Math.Max(0, atk - def);
            enemy.HP -= dmg;
            player.AttackCount++;
            enemy.DefenceCount++;
            return (atk, def, dmg);
        }

     
        public static (int atk, int def, int dmg) EnemyAttack(Enemy enemy, Player player)
        {
            int atk = enemy.AttackDice.Throw();
            int def = player.DefenceDice.Throw();
            int dmg = Math.Max(0, atk - def);
            player.HP -= dmg;
            enemy.AttackCount++;
            player.DefenceCount++;
            return (atk, def, dmg);
        }

      
        public static void Resolve(LevelElement attacker, LevelElement defender)
        {
            if (attacker is Player p && defender is Enemy e)
            {
                PlayerAttack(p, e);
            }
            else if (attacker is Enemy en && defender is Player pl)
            {
                EnemyAttack(en, pl);
            }
        }
    }
}

