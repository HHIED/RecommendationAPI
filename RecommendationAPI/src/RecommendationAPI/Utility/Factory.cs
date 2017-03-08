using MongoDB.Bson;
using RecommendationAPI.Business;
using System;
using System.Collections.Generic;
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

        public Behavior CreateBehaviorTest(string type, string id, DateTime timeStamp) {
            return new Behavior(type, id, timeStamp);
        }

        public Visitor CreateVisitor(List<BsonDocument> result) {
            BsonDocument visitorDoc = result[0];

            List<Behavior> behaviors = new List<Behavior>();
            BsonArray bsonBehaviors = visitorDoc["Behaviors"].AsBsonArray;

            foreach(BsonArray ba in bsonBehaviors) {
                try {
                    behaviors.Add(new Behavior(ba["Type"].AsString, ba["Id"].AsString, ba["Timestamp"].AsDateTime));
                } catch(InvalidCastException exception) {
                    Console.WriteLine(exception.Data);
                }
            }

            return new Visitor(visitorDoc["_id"].AsString, visitorDoc["ProfileUID"].AsString, visitorDoc["CustomerUID"].AsString, behaviors);
        }
    }
}
