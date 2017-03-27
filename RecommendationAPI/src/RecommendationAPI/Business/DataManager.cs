using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void CreateOrder(string profileUID, string customerUID, string itemUID, string created, string database) {
            _db.InsertOrder(profileUID, customerUID, itemUID, created, database);
        }

        public void CreateProduct(Product p, string database) {
            _db.InsertProduct(p, database);
        }

        public void CreateProductGroup(string productGroupUID, string attributeValue, string database) {
            _db.InsertProductGroup(productGroupUID, attributeValue, database);
        }

        public void CreateVisitor(string visitorUID, string database) {
            _db.insertVisitor(visitorUID, database);
        }

        public void UpdateCustomerData(string visitorUID, string customerUID, string database) {
            _db.UpdateCustomerData(visitorUID, customerUID, database);
        }

        public void UpdateProfileData(string visitorUID, string profileUID, string database) {
            _db.UpdateProfileData(visitorUID, profileUID, database);
        }
    }
}
