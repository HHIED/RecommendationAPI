using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    interface IProductRecommender
    {
        string[] GetProductRecommendations(string VisitorUID, string database);
    }
}
