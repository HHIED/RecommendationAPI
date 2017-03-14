using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace RecommendationAPI.Business
{
    public interface IDatabaseEngine
    {

        Task<Visitor> GetVisitor(string visitorUID, string database);

        Task<List<BsonArray>> GetVisitors(List<int> productUID, string database);

    }
}
