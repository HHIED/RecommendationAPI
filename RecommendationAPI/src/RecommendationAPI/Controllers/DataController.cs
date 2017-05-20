using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecommendationAPI.Business;
using RecommendationAPI.Utility;
using System.Web.Http;
using System.Net.Http;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers {

    [Route("[controller]")]
    public class DataController : Controller {

        private IDataManager dm = new DataManager(new DatabaseEngine());
        private Factory f = new Factory();
        private CollaborativeFilter cf = new CollaborativeFilter();
        private IProductRecommender pr = new ProductRecommender(new DatabaseEngine());


        // PUT visitor/visitorUID/database
        [Route("/visitor/{visitorUID}/{database}")]
        [HttpPut("{visitorUID}/{database}")]
        public HttpStatusCode PutVisitor(string visitorUID, string database) {
            return dm.CreateVisitor(visitorUID.ToUpper(), database.ToUpper());
        }

        // PUT product/productUID/description/productGroup/database
        [Route("/product/{productUID}/{description}/{productgroup}/{database}")]
        [HttpPut("{productUID}/{description}/{productgroup}/{database}")]
        public HttpStatusCode PutProduct(int productUID, string description, int productgroup, string database) {
            return dm.CreateProduct(f.CreateProduct(productUID, description.ToUpper(), productgroup), database.ToUpper());
        }

        // PUT behavior/visitorUID/behaviorType/ItemID/database
        [Route("/behavior/{visitorUID}/{behaviorType}/{itemID}/{database}")]
        [HttpPut("{visitorUID}/{behaviorType}/{itemID}/{database}")]
        public HttpStatusCode PutBehavior(string visitorUID, string behaviorType, int itemID, string database) {
            HttpStatusCode status = dm.CreateBehavior(visitorUID, f.CreateBehavior(itemID, behaviorType.ToUpper()), database.ToUpper());
            if(status == HttpStatusCode.OK) {
                pr.CalculateTopProducts(visitorUID, database.ToUpper());
                cf.CalculateScoreForProduct(itemID, database.ToUpper());
            }

            return status;

        }

        // GET "Update/database/password
        [Route("/update/{database}/{password}")]
        [HttpPut("{database}/{password}")]
        public HttpStatusCode GetUpdate(string database, string password) {
            if (password == "supersecretpassword") {
                cf.BuildCollaborativeFilter(database.ToUpper());
            }
            return HttpStatusCode.OK;
        }

        // GET "Updatevisitortopproducts/database/password
        [Route("/updatevisitorTopProducts/{database}/{password}")]
        [HttpPut("{database}/{password}")]
        public HttpStatusCode GetUpdateVisitorTopProducts(string database, string password) {
            if (password == "supersecretpassword") {
                pr.CalculateAllTopProducts(database.ToUpper());
            }
            return HttpStatusCode.OK;
        }

        // GET "Updatevisitortopproducts/visitorUID/database/password
        [Route("/updatevisitortopproducts/{visitorUID}/{database}/{password}")]
        [HttpPut("{database}/{visitorUID}/{password}")]
        public HttpStatusCode GetUpdateVisitorTopProducts(string database, string visitorUID, string password) {
            if (password == "supersecretpassword") {
                pr.CalculateTopProducts(visitorUID.ToUpper(), database.ToUpper());
            }
            return HttpStatusCode.OK;
        }

        // GET "calculateTop20/database/password
        [Route("/calculatetop20/{database}/{password}")]
        [HttpPut("{database}/{password}")]
        public HttpStatusCode CalculateTop20(string database, string password) {
            if (password == "supersecretpassword") {
                DateTime today = DateTime.Now;
                pr.CalculateMonthlyTop(database.ToUpper(), today.AddDays(-30));
            }
            return HttpStatusCode.OK;
        }

        // GET "calculateTop20/database/password
        [Route("/calculatetop20test/{database}/{password}")]
        [HttpPut("{database}/{password}")]
        public HttpStatusCode CalculateTop20Test(string database, string password) {
            if (password == "supersecretpassword") {
                DateTime today = new DateTime(2017, 3, 1);
                pr.CalculateMonthlyTop(database.ToUpper(), today.AddDays(-30));
            }
            return HttpStatusCode.OK;
        }   
    }
}
