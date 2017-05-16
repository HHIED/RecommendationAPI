using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    interface IDataManager
    {

        void CreateVisitor(string visitorUID, string database);
        void CreateProduct(Product p, string database);
        void CreateBehavior(string visitorUID, Behavior behavior, string database);
    }
}
