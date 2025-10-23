using System;
namespace Dungeoncrawler.LevelElements;

public class Wall : LevelElement
{
    public Wall(int x, int y) : base(x, y)
    {
        Symbol = '#';
        Color = ConsoleColor.Gray;
    }
}

