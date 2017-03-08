using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using RecommendationAPI.Utility;

namespace RecommendationAPI.Business
{
    public class DatabaseEngine : IDatabaseEngine {

        private MongoServerAddress address = new MongoServerAddress("mongodb://127.0.0.1");
        private IMongoClient _client;
        private IMongoDatabase _database;
        private Factory factory;
        

        public DatabaseEngine() {
            _client = new MongoClient("mongodb://localhost");
            factory = new Factory();
        }
        public async Task<Visitor> GetVisitor(string visitorUID, string database) {
            _database = _client.GetDatabase("Pandashop");
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return factory.CreateVisitor(result);
        }

        public Visitor[] GetVisitors(string productUID, string database) {
            throw new NotImplementedException();
        }
    }
}
