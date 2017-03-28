using MongoDB.Bson;
using System;
using System.Collections.Generic;
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
            Dictionary<string, double> weightedProducts = GetCalculatedProductWeights(visitor);
            List<int> orderedProducts = new List<int>();
            try {
                foreach(string s in weightedProducts.Keys) {
                    orderedProducts.Add(int.Parse(s));
                }
                HashSet<Visitor> similarVisitors = new HashSet<Visitor>();
                similarVisitors = GetSimilarVisitors(orderedProducts, database, similarVisitors);

                return GetMostPopularProducts(similarVisitors, numberOfRecommendations);

            } catch(InvalidOperationException ioe) {
                throw new InvalidOperationException();
            }
        }

        private HashSet<Visitor> GetSimilarVisitors(List<int> productUIDs, string database, HashSet<Visitor> similarVisitors) {

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
            foreach (string visitorUID in similarVisitorsUID) {
                similarVisitors.Add(_db.GetVisitor(visitorUID, database).Result);
            }

            productUIDs.RemoveAt(productUIDs.Count - 1);

            GetSimilarVisitors(productUIDs, database, similarVisitors);

            return similarVisitors;
        }

        private string[] GetMostPopularProducts(HashSet<Visitor> visitors, int numberOfRecommendations) {

            Dictionary<string, double> products = new Dictionary<string, double>();

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

        private Dictionary<string, double> countAndSortBehavior(List<Behavior> behaviors, Dictionary<string, double> products) {

            foreach (Behavior b in behaviors) {
                if (b.Type == "ProductView") {
                    if (products.ContainsKey(b.Id)) {
                        products[b.Id]++;
                    } else {
                        products.Add(b.Id, 1);
                    }
                }
            }

            var temp = from entry in products orderby entry.Value descending select entry;

            Dictionary<string, double> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDic;


        }

        private Dictionary<string, double> GetCalculatedProductWeights(Visitor visitor) {

            Dictionary<string, double> products = new Dictionary<string, double>();

            Dictionary<string, double> sortedBehaviors = countAndSortBehavior(visitor.Behaviors, products);

            Dictionary<string, double> productGroupWeight = GetProductGroupWeight(visitor.Behaviors);

            foreach(string product in sortedBehaviors.Keys) {
                products.Add(product, sortedBehaviors[product] * productGroupWeight[product]);
            }

            sortedBehaviors = countAndSortBehavior(visitor.Behaviors, products);

            return sortedBehaviors;
        }

        private Dictionary<string, double> GetProductGroupWeight(List<Behavior> behaviors) {
            Dictionary<string, int> productGroups = new Dictionary<string, int>();

            foreach (Behavior b in behaviors) {
                if (b.Type == "ProductGroupView") {
                    if (productGroups.ContainsKey(b.Id)) {
                        productGroups[b.Id]++;
                    } else {
                        productGroups.Add(b.Id, 1);
                    }
                }
            }

            Dictionary<string, double> productGroupWeight = calculateGroupWeight(productGroups);

            return productGroupWeight;
        }

        private Dictionary<string, double> calculateGroupWeight(Dictionary<string, int> productGroups) {
            int numberOfGroupViews = 0;
            Dictionary<string, double> productGroupWeight = new Dictionary<string, double>();

            foreach(string group in productGroups.Keys) {
                numberOfGroupViews += productGroups[group];
            }

            foreach (string group in productGroups.Keys) {
                productGroupWeight.Add(group, productGroups[group] / numberOfGroupViews);
            }

            return productGroupWeight;
        }
    }
}


