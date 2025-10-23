

using System;
namespace Dungeoncrawler.Mechanics;

public class Dice
{
    private int num, sides, bonus;
    private static Random rnd = new Random();

    public Dice(int n, int s, int b)
    {
        num = n;
        sides = s;
        bonus = b;
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
