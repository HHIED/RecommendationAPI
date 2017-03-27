using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecommendationAPI.Business;
using RecommendationAPI.Utility;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers
{

    [Route("[controller]")]
    public class DataController : Controller
    {

        private IDataManager dm = new DataManager(new DatabaseEngine());
        private Factory f = new Factory();


        [Route("/{visitorUID}")]
        [HttpGet("{visitorUID}")]
        public string[] GetRecommendationForVisitor(string visitorUID) {

                return new string[] { visitorUID };

        }

        // PUT visitor/visitorUID/database
        [Route("/visitor/{visitorUID}/{database}")]
        [HttpPut("{visitorUID}/{database}")]
        public void Put(string visitorUID, string database)
        {
            dm.CreateVisitor(visitorUID.ToUpper(), database.ToUpper());
        }

        // PUT product/productUID/created/description/database
        [Route("/product/{productUID}/{created}/{description}/{database}")]
        [HttpPut("{productUID}/{created}/{description}/{database}")]
        public void Put(string productUID, string created, string description, string database) {
            dm.CreateProduct(f.CreateProduct(productUID.ToUpper(), created.ToUpper(), description.ToUpper()), database.ToUpper());
        }

        // PUT visitorUID/database
        [Route("/behavior/{visitorUID}/{timestamp}/{behaviorType}/{itemID}/{database}")]
        [HttpPut("{visitorUID}/{timestamp}/{behaviorType}/{itemID}/{database}")]
        public void Put(string visitorUID, string timestamp, string behaviorType, string itemID, string database) {
            dm.CreateBehavior(visitorUID, f.CreateBehavior(itemID.ToUpper(), behaviorType.ToUpper(), timestamp.ToUpper()), database.ToUpper());
        }

        // PUT visitorUID/database
        [Route("/order/{profileUID}/{customerUID}/{itemUID}/{created}/{database}")]
        [HttpPut("{profileUID}/{customerUID}/{itemUID}/{created}/{database}")]
        public void PutOrder(string profileUID, string customerUID, string itemUID, string created, string database) {
            dm.CreateOrder(profileUID.ToUpper(), customerUID.ToUpper(), itemUID.ToUpper(), created.ToUpper(), database.ToUpper());
        }

        [Route("/profile/{visitorUID}/{profileUID}/{database}")]
        [HttpPut("{visitorUID}/{profileUID}/{database}")]
        public void PutProfile(string visitorUID, string profileUID, string database) {
            dm.UpdateProfileData(visitorUID.ToUpper(), profileUID.ToUpper(), database.ToUpper());
        }

        [Route("/customer/{visitorUID}/{customerUID}/{database}")]
        [HttpPut("{visitorUID}/{customerUID}/{database}")]
        public void PutCustomer(string visitorUID, string customerUID, string database) {
            dm.UpdateCustomerData(visitorUID.ToUpper(), customerUID.ToUpper(), database.ToUpper());
        }

        [Route("/productGroup/{productGroupUID}/{attributeValue}/{database}")]
        [HttpPut("{productGroupUID}/{attributeValue}/{database}")]
        public void PutProductGroup(string productGroupUID, string attributeValue, string database) {
            dm.CreateProductGroup(productGroupUID.ToUpper(), attributeValue.ToUpper(), database.ToUpper());
        }
        
    }
}
