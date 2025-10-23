

using System;
using Dungeoncrawler.LevelElements;
namespace Dungeoncrawler.LevelElements;

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
