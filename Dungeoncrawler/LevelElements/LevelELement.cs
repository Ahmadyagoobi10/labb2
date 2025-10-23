
using System;
namespace Dungeoncrawler.LevelElements;
public abstract class LevelElement
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; protected set; }
    public ConsoleColor Color { get; protected set; }

    public LevelElement(int x, int y) { X = x; Y = y; }

    public virtual void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X, Y);
        Console.Write(Symbol);
        Console.ResetColor();
    }
}
