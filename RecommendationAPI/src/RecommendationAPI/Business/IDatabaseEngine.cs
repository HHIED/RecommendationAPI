using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RecommendationAPI.Business
{
    public interface IDatabaseEngine
    {

        Task<Visitor> GetVisitor(string visitorUID, string database);

        Task<List<BsonArray>> GetVisitors(List<int> productUID, string database);

        void insertVisitor(string visitorUID, string database);
        void InsertBehavior(string visitorUID, Behavior behavior, string database);
        void InsertProduct(Product p, string database);
        void InsertOrder(string profileUID, string customerUID, string itemUID, string created, string database);
        void UpdateProfileData(string visitorUID, string profileUID, string database);
        void UpdateCustomerData(string visitorUID, string customerUID, string database);
        void InsertProductGroup(string productGroupUID, string attributeValue, string database);
    }
}
