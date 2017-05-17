using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace RecommendationAPI.Business
{
    public class DataManager : IDataManager {

        private IDatabaseEngine _db;

        public DataManager(IDatabaseEngine db) {
            _db = db;
        }

        public void CreateBehavior(string visitorUID, Behavior behavior, string database) {
            _db.InsertBehavior(visitorUID, behavior, database);
        }

        public void CreateProduct(Product p, string database) {
            _db.InsertProduct(p, database);
        }

        public void CreateVisitor(string visitorUID, string database) {
            _db.insertVisitor(visitorUID, database);

        }
    }
}
