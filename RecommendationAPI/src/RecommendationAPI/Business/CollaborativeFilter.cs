using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business {
    public class CollaborativeFilter {
        IDatabaseEngine db = new DatabaseEngine();

        public void BuildCollaborativeFilter(string database) {
            List<int> allProducts = db.GetAllProducts(database).Result;
            foreach (int product in allProducts) {
                CalculateScoreForProduct(product, database);
            }


        }

        public void CalculateScoreForProduct(int productUID, string database) {
            Product initialProduct = db.GetProduct(productUID, database).Result;
            if (initialProduct != null) {
                Dictionary<int, double> productScores = new Dictionary<int, double>();
                BsonArray visitors = db.GetVisitorsForProduct(productUID, database).Result;
                foreach (BsonString visitorId in visitors.Values) {
                    List<int> visitorProducts = db.GetVisitorProducts(visitorId.AsString, database).Result;
                    if (visitorProducts != null) {
                        foreach (int product in visitorProducts) {
                            if (productScores.Keys.Contains(product)) {
                                productScores[product]++;
                            } else {
                                productScores.Add(product, 1);
                            }
                        }
                    }
                }
                Dictionary<int, double> sortedScores = CountAndSortBehavior(productScores);

                if (sortedScores.Count > 100) {
                    //Only calculate top 100 product scores
                    for (int i = 0; i < 100; i++) {
                        int productId = sortedScores.ElementAt(i).Key;
                        Product compareProduct = db.GetProduct(productId, database).Result;
                        sortedScores[productId] = CalculateSimilarityScore(initialProduct, compareProduct, sortedScores[productId]);
                    }
                } else {
                    foreach (int productId in productScores.Keys) {
                        Product compareProduct = db.GetProduct(productId, database).Result;
                        sortedScores[productId] = CalculateSimilarityScore(initialProduct, compareProduct, sortedScores[productId]);
                    }
                }

                sortedScores = CountAndSortBehavior(sortedScores);
                Dictionary<int, double> top10ProductsAndScores = sortedScores.Take(10).ToDictionary(pair => pair.Key, pair => pair.Value);

                db.InsertScore(initialProduct.ProductUID, top10ProductsAndScores, database);

                Debug.WriteLine("Product " + initialProduct.ProductUID + " updated");
            }
        }


        private Dictionary<int, double> CountAndSortBehavior(Dictionary<int, double> products) {


            var temp = from entry in products orderby entry.Value descending select entry;

            Dictionary<int, double> sortedDic = temp.ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDic;
        }

        private double CalculateSimilarityScore(Product p1, Product p2, double currentScore) {
            double similarAttributeFactor = 0.02;
            double productGroupFactor = 0.00;
            double numOfSimAttributes = 0;
            if (p1.ProductGroup == p2.ProductGroup) {
                productGroupFactor = 0.02;
            }
            string[] wordsToMatch = p1.Description.Split(' ');
            foreach (string s in wordsToMatch) {
                if (p2.Description.Contains(s)) {
                    numOfSimAttributes++;
                }
            }
            if (numOfSimAttributes == 0) {
                similarAttributeFactor = 0;
            }

            return currentScore * (1 + productGroupFactor) * (1 + Math.Pow(similarAttributeFactor, numOfSimAttributes));

        }

        public void RecalculateProductScores(string visitorUID, string database) {
            List<int> visitorProducts = db.GetVisitorProducts(visitorUID, database).Result;
            foreach(int product in visitorProducts) {
                CalculateScoreForProduct(product, database);
            }
        }


    }
}
