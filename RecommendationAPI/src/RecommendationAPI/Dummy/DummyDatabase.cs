using System;
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

        public Task<Visitor> GetVisitor(string visitorUID, string database) {
            if (database == "ValidDatabase") {
                if (visitorUID == "ValidVisitor" || visitorUID=="VALIDVISITOR") {
                    var taskSource = new TaskCompletionSource<Visitor>();
                    taskSource.SetResult(validVisitor);
                    return taskSource.Task;
                } else if (visitorUID == "ValidVisitorNoBehavior" || visitorUID=="VALIDVISITORNOBEHAVIOR") {
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

        public Task<List<BsonArray>> GetVisitors(List<int> productUID, string database) {
            if(productUID.Count==0) {
                var taskSource = new TaskCompletionSource<List<BsonArray>>();
                List<BsonArray> visitorList = new List<BsonArray>();
                visitorList.Add(new BsonArray(new string[] { "ValidVisitorNoBehavior" }));
                taskSource.SetResult(visitorList);
                return taskSource.Task;
            }
            if (database == "ValidDatabase") {
                var taskSource = new TaskCompletionSource<List<BsonArray>>();
                List<BsonArray> visitorList = new List<BsonArray>();
                visitorList.Add(new BsonArray(new string[] { "ValidVisitor" }));
                taskSource.SetResult(visitorList);
                return taskSource.Task;
            } else {
                throw new InvalidOperationException();
            }
        }
    }
}
