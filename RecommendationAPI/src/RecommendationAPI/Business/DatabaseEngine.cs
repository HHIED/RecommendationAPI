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

        public async void insertVisitor(string visitorUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            BsonDocument newVisitor = new BsonDocument {
                {"_id",  visitorUID},
                {"Behaviors", new BsonArray()},
                {"ProfileUID",  ""},
                {"CustomerUID", "" }
            };
            await collection.InsertOneAsync(newVisitor);
        }

        public async void InsertProduct(Product p, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            BsonDocument newProduct = new BsonDocument {
                {"_id",  p.ProductUID},
                {"VisitorId", new BsonArray() },
                {"Description", p.Description},
                {"Created",  p.Created}
            };
            await collection.InsertOneAsync(newProduct);
        }

        public void InsertBehavior(string visitorUID, Behavior behavior, string database) {
            insertBehaviorOnVisitor(visitorUID,  behavior, database);
            if(behavior.Type=="PRODUCTVIEW") {
                insertBehaviorOnProduct(visitorUID, behavior, database);
            } else if(behavior.Type=="PRODUCTGROUPVIEW") {
                insertBehaviorOnProductGroup(visitorUID, behavior, database);
            }
            
            
        }

        private async void insertBehaviorOnProductGroup(string visitorUID, Behavior behavior, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("ProductGroup");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", behavior.Id);
            var update = Builders<BsonDocument>.Update
                .Push("VisitorId", visitorUID);
            var result = await collection.UpdateOneAsync(filter, update);
        }

        private async void insertBehaviorOnProduct(string visitorUID, Behavior behavior, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", behavior.Id);
            var update = Builders<BsonDocument>.Update
                .Push("VisitorId", visitorUID);
            var result = await collection.UpdateOneAsync(filter, update);
        }

        private async void insertBehaviorOnVisitor(string visitorUID, Behavior behavior, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            BsonDocument bsonBehavior = new BsonDocument {
                {"Timestamp", behavior.TimeStamp},
                {"Type", behavior.Type},
                {"Id", behavior.Id}
            };
            var update = Builders<BsonDocument>.Update
                .Push("Behaviors", bsonBehavior);
            var result = await collection.UpdateOneAsync(filter, update);
        }

        public async void InsertOrder(string profileUID, string customerUID, string itemUID, string created, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Customer");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", customerUID);
            BsonDocument bsonBehavior = new BsonDocument {
                {"ItemID", itemUID},
                {"Created", created}
            };
            var update = Builders<BsonDocument>.Update
                .Push("Orders", bsonBehavior);
            var result = await collection.UpdateOneAsync(filter, update);
        }

        public async void UpdateProfileData(string visitorUID, string profileUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            var update = Builders<BsonDocument>.Update
                .Set("ProfileUID", profileUID);
            var result = await collection.UpdateOneAsync(filter, update);
        }

        public async void UpdateCustomerData(string visitorUID, string customerUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            var update = Builders<BsonDocument>.Update
                .Set("CustomerUID", customerUID);
            var result = await collection.UpdateOneAsync(filter, update);

            var customerCollection = _database.GetCollection<BsonDocument>("Customer");
            BsonDocument newCustomer = new BsonDocument {
                {"_id", customerUID},
                {"Orders", new BsonArray()}
            };
            await collection.InsertOneAsync(newCustomer);
        }

        public async void InsertProductGroup(string productGroupUID, string attributeValue, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("ProductGroup");
            BsonDocument newProduct = new BsonDocument {
                {"_id",  productGroupUID},
                {"AttributeValue", attributeValue},
                {"VisitorId", new BsonArray()}
            };
            await collection.InsertOneAsync(newProduct);
        }
    }
}
