
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.Mechanics;
using Dungeoncrawler.Data;

namespace Dungeoncrawler.LevelElements.Enemies
{
    public abstract class Enemy : LevelElement
    {
        public string Name { get; protected set; }
        public int HP { get; set; }
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
}
