using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public interface IDatabaseEngine
    {

        Visitor GetVisitor(string visitorUID);

        Visitor[] GetVisitors(string productUID);

    }
}
