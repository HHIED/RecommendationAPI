using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    interface IDataManager
    {

        HttpStatusCode CreateVisitor(string visitorUID, string database);
        HttpStatusCode CreateProduct(Product p, string database);
        HttpStatusCode CreateBehavior(string visitorUID, Behavior behavior, string database);
    }
}
