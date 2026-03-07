
using Dungeoncrawler.Data;
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.Mechanics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dungeoncrawler.LevelElements.Enemies
{
    public abstract class Enemy : LevelElement
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; protected set; }
        public int HP { get; set; }
        public int XPos { get => X; set => X = value; }
        public int YPos { get => Y; set => Y = value; }
        public Dice AttackDice { get; protected set; }
        public Dice DefenceDice { get; protected set; }
        public int AttackCount { get; set; } = 0;
        public int DefenceCount { get; set; } = 0;

        protected Enemy(int x, int y) : base(x, y) { }

        public abstract void Update(Player player, LevelData level);

        public bool IsDead => HP <= 0;

        public void DrawIfVisible(bool visible)
        {
            if (visible) Draw();
        }
    }

    public class Goblin : Enemy
    {
        public Goblin(int x, int y) : base(x, y)
        {
            Name = "Goblin";
            HP = 50;
            AttackDice = new Dice(1, 6, 1);
            DefenceDice = new Dice(1, 6, 0);
        }

        public override void Update(Player player, LevelData level)
        {
           
        }
    }

    public class Orc : Enemy
    {
        public Orc(int x, int y) : base(x, y)
        {
            Name = "Orc";
            HP = 80;
            AttackDice = new Dice(2, 6, 2);
            DefenceDice = new Dice(1, 6, 1);
        }

        public override void Update(Player player, LevelData level)
        {
            
        }
    }
}