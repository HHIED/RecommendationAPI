using MongoDB.Bson;
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
            int[] topThreeProducts = GetTopThreeProducts(visitor);
            List<Visitor> similarVisitors = GetSimilarVisitors(topThreeProducts, database);

            return GetMostPopularProducts(similarVisitors);
        }

        private List<Visitor> GetSimilarVisitors(int[] productUIDs, string database) {

            
            List<BsonArray> productVisitors = _db.GetVisitors(productUIDs, database).Result;
            List<string> allMatchingVisitors = new List<string>();
            foreach (BsonArray b in productVisitors) {
                foreach (BsonValue bv in b) {
                    allMatchingVisitors.Add(bv.AsString);
                }
            }
            var similarVisitorsUID = allMatchingVisitors.GroupBy(x => x)
                        .Where(group => group.Count() > productVisitors.Count - 1)
                        .Select(group => group.Key);
            List<Visitor> similarVisitors = new List<Visitor>();
            foreach (string visitorUID in similarVisitorsUID) {
                similarVisitors.Add(_db.GetVisitor(visitorUID, database).Result);
            }

            return similarVisitors;
        }

        private string[] GetMostPopularProducts(List<Visitor> visitors) {
            throw new NotImplementedException();
        }

        public int[] GetTopThreeProducts(Visitor visitor) {
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

            return new int[] { int.Parse(sortedDic.Keys.ElementAt(0)), int.Parse(sortedDic.Keys.ElementAt(1)), int.Parse(sortedDic.Keys.ElementAt(2)) };
        }
    }
}
