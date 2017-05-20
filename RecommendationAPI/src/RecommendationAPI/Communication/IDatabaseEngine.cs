using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Net;

namespace RecommendationAPI.Business {
    public interface IDatabaseEngine {

        Task<Visitor> GetVisitor(string visitorUID, string database);

        Task<List<BsonArray>> GetVisitors(List<int> productUID, string database);
        Task<BsonArray> GetVisitorsForProduct(int productUID, string database);
        Task<List<int>> GetAllProducts(string database);
        Task<Product> GetProduct(int productUID, string database);
        Task<HttpStatusCode> insertVisitor(string visitorUID, string database);
        HttpStatusCode InsertBehavior(string visitorUID, Behavior behavior, string database);
        Task<List<int>> GetTopProducts(string visitorUID, string database);
        Task<List<int>> GetVisitorProducts(string productUID, string database);
        Task<HttpStatusCode> InsertProduct(Product p, string database);
        void InsertScore(int productUID, Dictionary<int, double> productsAndScore, string database);
        void InsertTopProduct(string visitorUID, List<int> topProducts, string database);
        Task<List<string>> GetAllVisitors(string database);
        Task<Dictionary<int, double>> GetTopProductRecommendation(int productUID, string database);
        Task<List<Behavior>> GetMonthlyBehaviors(string database, DateTime from);
        void StoreTop20Products(List<int> top20Products, string database);
        Task<List<string>> GetMonthlyTopProducts(string database);
        bool CheckForDatabase(string databse);
    }
}
