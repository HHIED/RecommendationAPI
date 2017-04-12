using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business {
    public class ProductRecommender : IProductRecommender {

        private IDatabaseEngine _db;

        public ProductRecommender(IDatabaseEngine db) {
            _db = db;
        }

        public string[] GetProductRecommendations(string visitorUID, int numberOfRecommendations, string database) {

            Visitor visitor = _db.GetVisitor(visitorUID, database).Result;
            if(visitor == null) {
                return null;
            }
            List<int> topVisitorProducts = _db.GetTopProducts(visitorUID, database).Result;
            return GetRecommendations(topVisitorProducts, numberOfRecommendations, database);
        }

        private string[] GetRecommendations(List<int> topVisitorProducts, int numberOfRecommendations, string database) {
            Dictionary<int, double> recommendations = new Dictionary<int, double>();
            foreach(int product in topVisitorProducts) {
                Dictionary<int, double> topProductRecommendation = _db.GetTopProductRecommendation(product, database).Result;
                foreach(int recommendation in topProductRecommendation.Keys) {
                    if(recommendations.Keys.Contains(recommendation)) {
                        recommendations[recommendation] += topProductRecommendation[recommendation];
                    } else {
                        recommendations.Add(recommendation, topProductRecommendation[recommendation]);

                    }
                }
            }
            Dictionary<int, double> finalRecommendation = SortRecommendation(recommendations);
            if(finalRecommendation.Count < numberOfRecommendations) {
                numberOfRecommendations = finalRecommendation.Count;
            }
            string[] finalResult = new string[numberOfRecommendations];
            for (int i = 0; i < numberOfRecommendations; i++) {
                finalResult[i] = finalRecommendation.Keys.ElementAt(i).ToString();
            }
            return finalResult;
        }

        private List<Visitor> GetSimilarVisitors(List<int> productUIDs, string database) {

            if(productUIDs.Count == 0) {
                throw new InvalidOperationException();
            }

            List<BsonArray> productVisitors = _db.GetVisitors(productUIDs, database).Result;
            List<string> allMatchingVisitors = new List<string>();
            foreach (BsonArray b in productVisitors) {
                foreach (BsonValue bv in b) {
                    allMatchingVisitors.Add(bv.AsString.ToUpper());
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

        private string[] GetMostPopularProducts(List<Visitor> visitors, int numberOfRecommendations) {

            Dictionary<string, int> products = new Dictionary<string, int>();

            int finalNumberOfRecommendations = 0;

            foreach (Visitor v in visitors) {
                products = countAndSortBehavior(v.Behaviors, products);
            }
            try {
                if (numberOfRecommendations > products.Count) {
                    finalNumberOfRecommendations = products.Count;
                } else {
                    finalNumberOfRecommendations = numberOfRecommendations;
                }
                string[] finalRecommendations = new string[finalNumberOfRecommendations];
                for (int i = 0; i < finalNumberOfRecommendations; i++) {
                    finalRecommendations[i] = products.ElementAt(i).Key;
                }
                return finalRecommendations;
            } catch (ArgumentOutOfRangeException ex) {
                return new string[] {""};
            }
        }

        private Dictionary<string, int> countAndSortBehavior(List<Behavior> behaviors, Dictionary<string, int> products) {

            foreach (Behavior b in behaviors) {
                if (b.Type == "PRODUCTVIEW") {
                    if (products.ContainsKey(b.Id)) {
                        products[b.Id]++;
                    } else {
                        products.Add(b.Id, 1);
                    }
                }
            }

            var temp = from entry in products orderby entry.Value descending select entry;

            Dictionary<string, int> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDic;
        }


        private Dictionary<int, double> SortRecommendation(Dictionary<int, double> products) {



            var temp = from entry in products orderby entry.Value descending select entry;

            Dictionary<int, double> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDic;
        }

        public List<int> GetTopThreeProducts(Visitor visitor) {
            return null;
            
        }

        public void CalculateTopProducts(string visitorUID, string database) {

            Visitor v = _db.GetVisitor(visitorUID, database).Result;
            Dictionary<string, int> products = new Dictionary<string, int>();

            Dictionary<string, int> sortedBehaviors = countAndSortBehavior(v.Behaviors, products);

            List<int> TopProducts = new List<int>();

            for (int i = 0; i < 5; i++) {
                if (sortedBehaviors.Count < i + 1) {
                    break;
                }
                TopProducts.Add(int.Parse(sortedBehaviors.ElementAt(i).Key));
            }
            _db.InsertTopProduct(visitorUID, TopProducts, database);
        }

        public void CalculateAllTopProducts(string database) {
            List<string> allVisitors = _db.GetAllVisitors(database).Result;
            int count = 0;
            foreach(string visitorUID in allVisitors) {
                CalculateTopProducts(visitorUID, database);
                Debug.WriteLine("Visitor " + count + " Updated");
                count++;
            }
        }
    }
}


