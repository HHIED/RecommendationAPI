using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using RecommendationAPI.Business;

namespace RecommendationAPI.Dummy
{
    public class DummyDatabase : IDatabaseEngine {
        public Task<Visitor> GetVisitor(string visitorUID, string database) {
            if (database == "ValidDatabase") {
                if (visitorUID == "ValidVisitor") {
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
                    Visitor validVisitor = new Visitor(visitorUID, "ProfileUID", "CustomerUID0", behvaviors);
                    var taskSource = new TaskCompletionSource<Visitor>();
                    taskSource.SetResult(validVisitor);
                    return taskSource.Task;
                } else {
                    throw new InvalidOperationException();
                }
            } else {
                throw new InvalidOperationException();
            }
        }

        public Task<List<BsonArray>> GetVisitors(int[] productUID, string database) {
            if(database=="ValidDatabase") {
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
