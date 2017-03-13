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

        public string[] GetProductRecommendations(string visitorUID, string database) {
            Visitor visitor = _db.GetVisitor(visitorUID, database).Result;
            string[] topThreeProducts = GetTopThreeProducts(visitor);
            Visitor[] similarVisitors = GetSimilarVisitors(topThreeProducts, database);

            return GetMostPopularProducts(similarVisitors);
        }

        private Visitor[] GetSimilarVisitors(string[] productUIDs, string database) {
            throw new NotImplementedException();
        }

        private string[] GetMostPopularProducts(Visitor[] visitors) {
            throw new NotImplementedException();
        }

        public string[] GetTopThreeProducts(Visitor visitor) {
            Dictionary<string, int> products = new Dictionary<string, int>();
            foreach(Behavior b in visitor.Behaviors){
                if (b.Type == "ProductView") {
                    if (products.ContainsKey(b.Id)) {
                        products[b.Id]++;
                    } else {
                        products.Add(b.Id, 1);
                    }
                }
            }

            var temp = from entry in products orderby entry.Value descending select entry;
            Dictionary<string, int> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            List<string> result = new List<string>();

            foreach(string s in sortedDic.Keys) {
                result.Add(s);
            }

            return new string[] { sortedDic.Keys.ElementAt(0), sortedDic.Keys.ElementAt(1), sortedDic.Keys.ElementAt(2) };
        }
    }
}
