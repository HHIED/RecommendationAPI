using MongoDB.Bson;
using RecommendationAPI.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static RecommendationAPI.Business.Behavior;

namespace RecommendationAPI.Utility {
    public class Factory {
        public Visitor CreateVisitorTest(string visitorUID, string profileUID, string customerUID, List<Behavior> behaviors) {
            return new Visitor(visitorUID, profileUID, customerUID, behaviors);
        }

        public Behavior CreateBehaviorTest(string type, int id, DateTime timeStamp) {
            return new Behavior(type, id, timeStamp);
        }

        public Visitor CreateVisitor(List<BsonDocument> result) {
            BsonDocument visitorDoc = result[0];

            List<Behavior> behaviors = new List<Behavior>();
            BsonArray bsonBehaviors = visitorDoc["Behaviors"].AsBsonArray;

            foreach (BsonDocument bd in bsonBehaviors.Values) {
                if (bd["Type"] == "PRODUCTVIEW") {
                    try {
                        behaviors.Add(new Behavior(bd["Type"].AsString, bd["Id"].AsInt32, bd["Timestamp"].ToUniversalTime()));
                    } catch (InvalidCastException exception) {
                        Debug.WriteLine(exception.Data);
                    }   
                }
            }
            if (visitorDoc["ProfileUID"] != BsonNull.Value && visitorDoc["CustomerUID"] != BsonNull.Value) {
                return new Visitor(visitorDoc["_id"].AsString, visitorDoc["ProfileUID"].AsString, visitorDoc["CustomerUID"].AsString, behaviors);
            } else {
                return new Visitor(visitorDoc["_id"].AsString, null, null, behaviors);
            }
        }

        public Product CreateProduct(int productUID, string description, int productGroup) {
            return new Product(productUID, DateTime.Now, description, productGroup);
        }

        public Product CreateProduct(BsonDocument product) {
            return new Product(product["_id"].AsInt32, product["Created"].ToUniversalTime(), product["Description"].AsString, product["ProductGroupId"].AsInt32);
        }

        public Behavior CreateBehavior(int itemID, string behaviorType) {
            return new Behavior(behaviorType, itemID, DateTime.Now);
        }
    }
}
