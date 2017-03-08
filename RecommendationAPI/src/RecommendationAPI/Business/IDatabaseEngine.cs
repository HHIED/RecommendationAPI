using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public interface IDatabaseEngine
    {

        Task<Visitor> GetVisitor(string visitorUID, string database);

        Visitor[] GetVisitors(string productUID, string database);

    }
}
