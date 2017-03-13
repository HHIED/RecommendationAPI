using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _database = _client.GetDatabase("Pandashop");
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return factory.CreateVisitor(result);
        }

        public async Task<List<Visitor>> GetVisitors(int[] productUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            List<BsonArray> visitorLists = new List<BsonArray>();
            List<string> visitorListsFinal = new List<string>();
            Stopwatch s = new Stopwatch();
            s.Start();
            foreach (int productId in productUID) {
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("_id", productId);
                var result = await collection.Find(filter).ToListAsync();
                BsonDocument bd = result.ElementAt(0);
                BsonArray vistorIds = bd["VisitorId"].AsBsonArray;
                visitorLists.Add(vistorIds);
            }
            foreach (BsonArray b in visitorLists) {
                foreach (BsonValue bv in b) {
                    visitorListsFinal.Add(bv.AsString);
                }
            }
            var duplicateKeys = visitorListsFinal.GroupBy(x => x)
                        .Where(group => group.Count() > visitorLists.Count-1)
                        .Select(group => group.Key);
            List<Visitor> MatchingVisitors = new List<Visitor>();
            foreach(string visitorUID in duplicateKeys) {
                MatchingVisitors.Add(GetVisitor(visitorUID, database).Result);
            }
            Debug.WriteLine("Matching visitors: " + MatchingVisitors.Count);
            Debug.WriteLine("Time: " + s.ElapsedMilliseconds);
            return MatchingVisitors;
        }
    }
}
