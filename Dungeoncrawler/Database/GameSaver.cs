
using Dungeoncrawler.Database;
using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Dungeoncrawler.Data
{
    public class GameSaver
    {
        private readonly MongoService mongo;

        public GameSaver()
        {
            mongo = new MongoService("AhmadYagoobi"); 
        }

        public void Save(Player player, List<Enemy> enemies)
        {
            mongo.SavePlayer(player);
            mongo.SaveEnemies(enemies);
        }

       
        public Player LoadPlayer(string playerName)
        {
            return mongo.GetPlayer(playerName);
        }

        public List<Enemy> LoadEnemies()
        {
            var enemyDocs = mongo.Enemies.Find(_ => true).ToList();
            List<Enemy> list = new List<Enemy>();

            foreach (var doc in enemyDocs)
            {
                if (doc.Name.StartsWith("Rat")) list.Add(new Rat(doc.X, doc.Y) { HP = doc.HP, Id = doc.Id });
                else if (doc.Name.StartsWith("Snake")) list.Add(new Snake(doc.X, doc.Y) { HP = doc.HP, Id = doc.Id });
                else if (doc.Name == "Goblin") list.Add(new Goblin(doc.X, doc.Y) { HP = doc.HP, Id = doc.Id });
                else if (doc.Name == "Orc") list.Add(new Orc(doc.X, doc.Y) { HP = doc.HP, Id = doc.Id });
            }

            return list;
        }
    }
}