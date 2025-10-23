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
