﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business {
    public class ProductRecommender : IProductRecommender {

        public IDatabaseEngine _db;

        public ProductRecommender(IDatabaseEngine db) {
            _db = db;
        }

        public string[] GetProductRecommendations(string visitorUID, int numberOfRecommendations, string database) {
            Visitor visitor = _db.GetVisitor(visitorUID, database).Result;
            List<int> topThreeProducts = GetTopThreeProducts(visitor);
            List<Visitor> similarVisitors = GetSimilarVisitors(topThreeProducts, database);

            return GetMostPopularProducts(similarVisitors, numberOfRecommendations);
        }

        private List<Visitor> GetSimilarVisitors(List<int> productUIDs, string database) {


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

            foreach (Visitor v in visitors) {
                products = countAndSortBehavior(v.Behaviors, products);
            }
            try {
                string[] finalRecommendations = new string[numberOfRecommendations];
                for (int i = 0; i < numberOfRecommendations; i++) {
                    finalRecommendations[i] = products.ElementAt(i).Key;
                }
                return finalRecommendations;
            } catch (ArgumentOutOfRangeException ex) {
                return new string[] {""};
            }
        }

        private Dictionary<string, int> countAndSortBehavior(List<Behavior> behaviors, Dictionary<string, int> products) {

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

            Dictionary<string, int> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDic;


        }

        public List<int> GetTopThreeProducts(Visitor visitor) {

            Dictionary<string, int> products = new Dictionary<string, int>();

            Dictionary<string, int> sortedBehaviors = countAndSortBehavior(visitor.Behaviors, products);

            List<int> TopProducts = new List<int>();

            for (int i = 0; i < 3; i++) {
                if (sortedBehaviors.Count < i + 1) {
                    break;
                }
                TopProducts.Add(int.Parse(sortedBehaviors.ElementAt(0).Key));
            }
            return TopProducts;
        }
    }
}


