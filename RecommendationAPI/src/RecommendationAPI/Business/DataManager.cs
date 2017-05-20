using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Net;

namespace RecommendationAPI.Business
{
    public class DataManager : IDataManager {

        private IDatabaseEngine _db;

        public DataManager(IDatabaseEngine db) {
            _db = db;
        }

        public HttpStatusCode CreateBehavior(string visitorUID, Behavior behavior, string database) {
            return _db.InsertBehavior(visitorUID, behavior, database);
        }

        public HttpStatusCode CreateProduct(Product p, string database) {
            return _db.InsertProduct(p, database).Result;
        }

        public HttpStatusCode CreateVisitor(string visitorUID, string database) {
            return _db.insertVisitor(visitorUID, database).Result;

        }
    }
}
