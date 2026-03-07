

using Dungeoncrawler.LevelElements;
using Dungeoncrawler.LevelElements.Enemies;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Dungeoncrawler.Database
{
    public class EnemyData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MongoService
    {
        private readonly IMongoDatabase db;

        public MongoService(string databaseName = "AhmadYagoobi")
        {
            var client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Player> Players => db.GetCollection<Player>("players");
        public IMongoCollection<EnemyData> Enemies => db.GetCollection<EnemyData>("enemies");

        public void SavePlayer(Player player)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);

            if (string.IsNullOrEmpty(player.Id))
            {
                Players.InsertOne(player);
            }
            else
            {
               
                Players.ReplaceOne(filter, player, new ReplaceOptions { IsUpsert = true });
            }
        }

        public Player GetPlayer(string name)
        {
            return Players.Find(p => p.Name == name).FirstOrDefault();
        }

        
        public void SaveEnemies(List<Enemy> enemies)
        {
            List<EnemyData> dataList = new List<EnemyData>();
            foreach (var e in enemies)
            {
                dataList.Add(new EnemyData
                {
                    Id = e.Id,
                    Name = e.Name,
                    HP = e.HP,
                    X = e.X,
                    Y = e.Y
                });
            }

            foreach (var data in dataList)
            {
                Enemies.ReplaceOne(e => e.Id == data.Id, data, new ReplaceOptions { IsUpsert = true });
            }
        }

        public List<Enemy> GetEnemies()
        {
            var enemyDocs = Enemies.Find(_ => true).ToList();
            List<Enemy> list = new List<Enemy>();

            foreach (var e in enemyDocs)
            {
                if (e.Name == "Goblin") list.Add(new Goblin(e.X, e.Y) { HP = e.HP });
                else if (e.Name == "Orc") list.Add(new Orc(e.X, e.Y) { HP = e.HP });
            }

            return list;
        }
    }
}