using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    interface IProductRecommender
    {
        string[] GetProductRecommendations(string VisitorUID, int numberOfRecommendations, string database);
        void CalculateTopProducts(string visitorUID, string database);
        void CalculateAllTopProducts(string database);
    }
}
