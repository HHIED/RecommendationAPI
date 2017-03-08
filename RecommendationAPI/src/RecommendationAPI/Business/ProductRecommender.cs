using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class ProductRecommender : IProductRecommender {

        public IDatabaseEngine _db;

        public ProductRecommender(IDatabaseEngine db) {
            _db = db;
        }

        public string[] GetProductRecommendations(string VisitorUID) {
            throw new NotImplementedException();
        }

        private Visitor[] GetSimilarVisitors() {
            throw new NotImplementedException();
        }

        private string[] GetMostPopularProducts() {
            throw new NotImplementedException();
        }
    }
}
