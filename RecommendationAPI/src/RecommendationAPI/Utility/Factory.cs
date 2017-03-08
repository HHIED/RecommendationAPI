using MongoDB.Bson;
using RecommendationAPI.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static RecommendationAPI.Business.Behavior;

namespace RecommendationAPI.Utility
{
    public class Factory
    {
        public Visitor CreateVisitorTest(string visitorUID, string profileUID, string customerUID, List<Behavior> behaviors) {
            return new Visitor(visitorUID, profileUID, customerUID, behaviors);
        }

        public Behavior CreateBehaviorTest(string type, string id, string timeStamp) {
            return new Behavior(type, id, timeStamp);
        }

        public Visitor CreateVisitor(List<BsonDocument> result) {
            BsonDocument visitorDoc = result[0];

            List<Behavior> behaviors = new List<Behavior>();
            BsonArray bsonBehaviors = visitorDoc["Behaviors"].AsBsonArray;

            foreach(BsonDocument bd in bsonBehaviors.Values) {
                try {
                    behaviors.Add(new Behavior(bd["Type"].AsString, bd["Id"].AsString, bd["Timestamp"].AsString));
                } catch(InvalidCastException exception) {
                    Debug.WriteLine(exception.Data);
                }
            }

            return new Visitor(visitorDoc["_id"].AsString, visitorDoc["ProfileUID"].AsString, visitorDoc["CustomerUID"].AsString, behaviors);
        }
    }
}
