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
        void CreateOrder(string profileUID, string customerUID, string itemUID, string created, string database);
        void UpdateProfileData(string visitorUID, string profileUID, string database);
        void UpdateCustomerData(string visitorUID, string customerUID, string database);
        void CreateProductGroup(string productGroupUID, string attributeValue, string database);
    }
}
