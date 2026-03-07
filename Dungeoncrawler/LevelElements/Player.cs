
using System;
using Dungeoncrawler.Mechanics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dungeoncrawler.LevelElements
{
    public class Player : LevelElement
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public int Turns { get; set; } = 0;
        public int HP { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }
        public int AttackCount { get; set; } = 0;
        public int DefenceCount { get; set; } = 0;

        public Player(int x, int y, string name = "Spelare") : base(x, y)
        {
            Symbol = '@';
            Color = ConsoleColor.Cyan;
            HP = 100;
            AttackDice = new Dice(2, 6, 2);
            DefenceDice = new Dice(2, 6, 0);
            Name = name;
        }

        public bool IsDead => HP <= 0;
    }
}