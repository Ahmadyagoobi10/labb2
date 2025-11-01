
using Dungeoncrawler.Data;
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;
using Dungeoncrawler.Mechanics;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

      
        List<string> combatLog = new List<string>();

        while (true)
        {
            Console.Clear();

            
            for (int i = 0; i < combatLog.Count; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(combatLog[i].PadRight(Console.WindowWidth));
            }

        
            foreach (var wall in level.Elements.OfType<Wall>())
            {
                double dist = level.Distance(player.X, player.Y, wall.X, wall.Y);
                if (dist <= VISION) discovered[wall.X, wall.Y] = true;
                if (discovered[wall.X, wall.Y]) wall.Draw();
            }

            
            foreach (var enemy in level.Elements.OfType<Enemy>())
            {
                double dist = level.Distance(player.X, player.Y, enemy.X, enemy.Y);
                if (dist <= VISION && !enemy.IsDead) enemy.DrawIfVisible(true);
            }

            
            player.Draw();

            
            Console.SetCursorPosition(0, level.Height + 1);
            Console.Write($"Player HP: {player.HP}      ");

           
            int statusLine = Math.Min(level.Height + 2, Console.WindowHeight - 3);
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

           
            var enemiesHere = level.Elements
                                    .OfType<Enemy>()
                                    .Where(e => !e.IsDead && level.Distance(player.X, player.Y, e.X, e.Y) <= 1)
                                    .ToList();

            
            if (enemiesHere.Any())
            {
                
                combatLog.Clear();

                foreach (var enemy in enemiesHere)
                {
                    
                    var result = Combat.PlayerAttack(player, enemy);
                    combatLog.Add($"Du attackerar {enemy.Name}: ATK {result.atk} vs DEF {result.def} → Skada: {result.dmg}");

                   
                    var enemyResult = Combat.EnemyAttack(enemy, player);
                    combatLog.Add($"{enemy.Name} attackerar dig: ATK {enemyResult.atk} vs DEF {enemyResult.def} → Skada: {enemyResult.dmg}");

                    
                    if (enemy.IsDead)
                    {
                        combatLog.Add($"{enemy.Name} dog!");
                        level.Elements.Remove(enemy);
                    }
                }
            }

           
            foreach (var enemy in level.Elements.OfType<Enemy>().Where(e => !e.IsDead && !enemiesHere.Contains(e)).ToList())
            {
                enemy.Update(player, level);
            }

            
            if (player.IsDead)
            {
                Console.Clear();
                Console.WriteLine("Game Over! Du dog!");
                Console.WriteLine("Tryck på valfri tangent för att avsluta...");
                Console.ReadKey(true);
                break;
            }
        }
    }
}

// tack