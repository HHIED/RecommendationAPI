﻿using System;
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
            if (result.Count == 0) {
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
            insertBehaviorOnVisitor(visitorUID, behavior, database);
            if (behavior.Type == "PRODUCTVIEW") {
                insertBehaviorOnProduct(visitorUID, behavior, database);
            } else if (behavior.Type == "PRODUCTGROUPVIEW") {
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

        public async Task<BsonArray> GetVisitorsForProduct(int productUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("_id", productUID);
            var result = await collection.Find(filter).ToListAsync();
            BsonDocument bd = result[0];
            BsonArray visitors = bd["VisitorId"].AsBsonArray;

            return visitors;
        }

        public async Task<Product> GetProduct(int productUID, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("_id", productUID);
            var result = await collection.Find(filter).ToListAsync();

            return factory.CreateProduct(result[0]);
        }

        public async Task<List<int>> GetVisitorProducts(string visitorUID, string database) {
            List<int> visitorProducts = new List<int>();
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("_id", visitorUID);
            var result = await collection.Find(filter).ToListAsync();
            if (result != null) {
                BsonDocument visitor = result[0];
                foreach (BsonDocument behavior in visitor["Behaviors"].AsBsonArray) {
                    if (behavior["Type"].AsString == "ProductView") {
                        visitorProducts.Add(int.Parse(behavior["Id"].AsString));
                    }
                }
            }
            return visitorProducts;
        }


        public async void InsertScore(int productUID, Dictionary<int, double> productsAndScore, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", productUID);
            var clear = Builders<BsonDocument>.Update.Set("TopProducts", new BsonArray());
            var clearResult = await collection.UpdateOneAsync(filter, clear);
            foreach (int i in productsAndScore.Keys) {
                BsonDocument bsonBehavior = new BsonDocument {
                {"ProductUID", i},
                {"Score", productsAndScore[i]}
            };

                var update = Builders<BsonDocument>.Update
                    .Push("TopProducts", bsonBehavior);
                var result = await collection.UpdateOneAsync(filter, update);

            }

        }

        public async Task<List<int>> GetAllProducts(string database) {
            List<int> products = new List<int>();
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Product");

            var result = await collection.Find(_ => true).ToListAsync();
            for (int i = 0; i < result.Count; i++) {
                BsonDocument product = result[i];
                products.Add(product["_id"].AsInt32);
            }
            return products;
        }

        public async void InsertTopProduct(string visitorUID, List<int> topProducts, string database) {
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", visitorUID);
            var clear = Builders<BsonDocument>.Update.Set("TopProducts", new BsonArray());
            var clearResult = await collection.UpdateOneAsync(filter, clear);
            foreach (int i in topProducts) {
                var update = Builders<BsonDocument>.Update
                  .Push("TopProducts", i);
                var result = await collection.UpdateOneAsync(filter, update);

            }

            }

        public async Task<List<string>> GetAllVisitors(string database) {
            List<string> visitors = new List<string>();
            _database = _client.GetDatabase(database);
            var collection = _database.GetCollection<BsonDocument>("Visitor");

            var result = await collection.Find(_ => true).ToListAsync();
            for (int i = 0; i < result.Count; i++) {
                BsonDocument visitor = result[i];
                visitors.Add(visitor["_id"].AsString);
            }
            return visitors;
        }
    }
    }
}
