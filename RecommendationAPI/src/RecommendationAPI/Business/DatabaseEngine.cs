using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class DatabaseEngine : IDatabaseEngine {
        public Visitor GetVisitor(string visitorUID) {
            throw new NotImplementedException();
        }

        public Visitor[] GetVisitors(string productUID) {
            throw new NotImplementedException();
        }
    }
}
