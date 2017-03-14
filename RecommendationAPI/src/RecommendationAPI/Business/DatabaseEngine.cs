using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using RecommendationAPI.Utility;
using System.Diagnostics;

namespace RecommendationAPI.Business {
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
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();
            if(result.Count == 0) {
                return null;
            }

            return factory.CreateVisitor(result);
        }

        public async Task<List<BsonArray>> GetVisitors(List<int> productUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            List<BsonArray> visitorLists = new List<BsonArray>();

            foreach (int productId in productUID) {
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("_id", productId);
                var result = await collection.Find(filter).ToListAsync();

                if (result.Count > 0) {
                    BsonDocument bd = result.ElementAt(0);
                    BsonArray vistorIds = bd["VisitorId"].AsBsonArray;
                    visitorLists.Add(vistorIds);
                }
            }

            if (visitorLists.Count == 0) {
                return null;
            }

            return visitorLists;
        }
    }
}
