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

            if (!_db.CheckForDatabase(database)) {
                return new string[0];
            }

            try {
                Visitor visitor = _db.GetVisitor(visitorUID, database).Result;

                List<int> topVisitorProducts = _db.GetTopProducts(visitorUID, database).Result;

                if (topVisitorProducts.Count <= 0) {
                    return GetMonthlyTopProducts(database, numberOfRecommendations).ToArray();
                }

                return GetRecommendations(topVisitorProducts, numberOfRecommendations, database);
            }catch(AggregateException ae) {
                return GetMonthlyTopProducts(database, numberOfRecommendations).ToArray();
            }
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
                finalRecommendation = FillFromDefault(finalRecommendation, database, numberOfRecommendations);
                if(finalRecommendation.Count < numberOfRecommendations) {
                    numberOfRecommendations = finalRecommendation.Count;
                }
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

        public void CalculateMonthlyTop(string database, DateTime from) {
            List<Behavior> monthlyBehaviors = _db.GetMonthlyBehaviors(database, from).Result;
            Dictionary<string, int> behaviors = new Dictionary<string, int>();
            Dictionary<string, int> sortedBehaviors = countAndSortBehavior(monthlyBehaviors, behaviors);
            int length = 20;
            if(sortedBehaviors.Count < length) {
                length = sortedBehaviors.Count;
            }
            List<string> top20Products = new List<string>();
            for (int i = 0; i < length; i++) {
                top20Products.Add(sortedBehaviors.Keys.ElementAt(i));
            }
            _db.StoreTop20Products(top20Products, database);
        }

        private List<string> GetMonthlyTopProducts(string database, int numberOfRecommendations) {
            List<string> top20Products = _db.GetMonthlyTopProducts(database).Result;
            List<string> result = new List<string>();

            if (numberOfRecommendations > 20) {
                numberOfRecommendations = 20;
            }

            for(int i = 0; i < numberOfRecommendations; i++) {
                result.Add(top20Products[i]);
            }

            return result;
        }

        private Dictionary<int, double> FillFromDefault(Dictionary<int, double> recommendations, string database, int numberOfRecommendations) {
            List<string> topProducts = GetMonthlyTopProducts(database, 20);

            int i = 0;

            while(recommendations.Count < numberOfRecommendations) {
                if (i >= topProducts.Count) {
                    break;
                }

                if (!recommendations.ContainsKey(int.Parse(topProducts[i]))) {
                    recommendations.Add(int.Parse(topProducts[i]), 1);
                }
                
                i++;
            }

            return recommendations;
        }
    }
}


