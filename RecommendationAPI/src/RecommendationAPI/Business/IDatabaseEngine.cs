using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RecommendationAPI.Business {
    public interface IDatabaseEngine {

        Task<Visitor> GetVisitor(string visitorUID, string database);

        Task<List<BsonArray>> GetVisitors(List<int> productUID, string database);
        Task<BsonArray> GetVisitorsForProduct(int productUID, string database);
        Task<List<int>> GetAllProducts(string database);
        Task<Product> GetProduct(int productUID, string database);
        void insertVisitor(string visitorUID, string database);
        void InsertBehavior(string visitorUID, Behavior behavior, string database);
        Task<List<int>> GetTopProducts(string visitorUID, string database);
        Task<List<int>> GetVisitorProducts(string productUID, string database);
        void InsertProduct(Product p, string database);
        void InsertScore(int productUID, Dictionary<int, double> productsAndScore, string database);
        void InsertTopProduct(string visitorUID, List<int> topProducts, string database);
        Task<List<string>> GetAllVisitors(string database);
        Task<Dictionary<int, double>> GetTopProductRecommendation(int productUID, string database);
    }
}
