﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RecommendationAPI.Business;

namespace RecommendationAPI.Dummy {
    public class DummyDatabase : IDatabaseEngine {

        private Visitor validVisitor;
        private Visitor visitorNoBehavior;

        public DummyDatabase() {
            List<Behavior> behvaviors = new List<Behavior>();
            behvaviors.Add(new Behavior("ProductView", "1234", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1234", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1235", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1235", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1236", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1237", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1238", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1238", "Time"));
            behvaviors.Add(new Behavior("ProductView", "1239", "Time"));
            validVisitor = new Visitor("ValidVisitor", "ProfileUID", "CustomerUID", behvaviors);

            visitorNoBehavior = new Visitor("NoBehavior", "ProfileUID", "CustomerUID", new List<Behavior>());
        }

        public Task<List<int>> GetAllProducts(string database) {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllVisitors(string database) {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(int productUID, string database) {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, double>> GetTopProductRecommendation(int productUID, string database) {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetTopProducts(string visitorUID, string database) {
            throw new NotImplementedException();
        }

        public Task<Visitor> GetVisitor(string visitorUID, string database) {
            if (database.ToUpper() == "VALIDDATABASE") {
                if (visitorUID.ToUpper()=="VALIDVISITOR") {
                    var taskSource = new TaskCompletionSource<Visitor>();
                    taskSource.SetResult(validVisitor);
                    return taskSource.Task;
                } else if (visitorUID.ToUpper()=="VALIDVISITORNOBEHAVIOR") {
                    var taskSource = new TaskCompletionSource<Visitor>();
                    taskSource.SetResult(visitorNoBehavior);
                    return taskSource.Task;
                } else {
                    throw new InvalidOperationException();
                }
            } else {
                throw new InvalidOperationException();
            }
        }

        public Task<List<int>> GetVisitorProducts(string productUID, string database) {
            throw new NotImplementedException();
        }

        public Task<List<BsonArray>> GetVisitors(List<int> productUID, string database) {
            if(productUID.Count==0) {
                var taskSource = new TaskCompletionSource<List<BsonArray>>();
                List<BsonArray> visitorList = new List<BsonArray>();
                visitorList.Add(new BsonArray(new string[] { "ValidVisitorNoBehavior" }));
                taskSource.SetResult(visitorList);
                return taskSource.Task;
            }
            if (database.ToUpper() == "VALIDDATABASE") {
                var taskSource = new TaskCompletionSource<List<BsonArray>>();
                List<BsonArray> visitorList = new List<BsonArray>();
                visitorList.Add(new BsonArray(new string[] { "ValidVisitor" }));
                taskSource.SetResult(visitorList);
                return taskSource.Task;
            } else {
                throw new InvalidOperationException();
            }
        }

        public Task<BsonArray> GetVisitorsForProduct(int productUID, string database) {
            throw new NotImplementedException();
        }

        public void InsertBehavior(string visitorUID, Behavior behavior, string database) {
            throw new NotImplementedException();
        }

        public void InsertProduct(Product p, string database) {
            throw new NotImplementedException();
        }

        public void InsertScore(int productUID, Dictionary<int, double> productsAndScore, string database) {
            throw new NotImplementedException();
        }

        public void InsertTopProduct(string visitorUID, List<int> topProducts, string database) {
            throw new NotImplementedException();
        }

        public void insertVisitor(string visitorUID, string database) {
            throw new NotImplementedException();
        }
    }
}
