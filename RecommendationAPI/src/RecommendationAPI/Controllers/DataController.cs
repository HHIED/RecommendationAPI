using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecommendationAPI.Business;
using RecommendationAPI.Utility;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers {

    [Route("[controller]")]
    public class DataController : Controller {

        private IDataManager dm = new DataManager(new DatabaseEngine());
        private Factory f = new Factory();
        private CollaborativeFilter cf = new CollaborativeFilter();
        private IProductRecommender pr = new ProductRecommender(new DatabaseEngine());


        [Route("/{visitorUID}")]
        [HttpGet("{visitorUID}")]
        public string[] GetRecommendationForVisitor(string visitorUID) {

            return new string[] { visitorUID };

        }

        // PUT visitor/visitorUID/database
        [Route("/visitor/{visitorUID}/{database}")]
        [HttpPut("{visitorUID}/{database}")]
        public void Put(string visitorUID, string database) {
            dm.CreateVisitor(visitorUID.ToUpper(), database.ToUpper());
        }

        // PUT product/productUID/description/productGroup/database
        [Route("/product/{productUID}/{description}/{productgroup}/{database}")]
        [HttpPut("{productUID}/{description}/{productgroup}/{database}")]
        public void Put(int productUID, string description, int productgroup, string database) {
            dm.CreateProduct(f.CreateProduct(productUID, description.ToUpper(), productgroup), database.ToUpper());
        }

        // PUT behavior/visitorUID/behaviorType/ItemID/database
        [Route("/behavior/{visitorUID}/{behaviorType}/{itemID}/{database}")]
        [HttpPut("{visitorUID}/{behaviorType}/{itemID}/{database}")]
        public void Put(string visitorUID, string behaviorType, string itemID, string database) {
            dm.CreateBehavior(visitorUID, f.CreateBehavior(itemID.ToUpper(), behaviorType.ToUpper()), database.ToUpper());
          //  pr.CalculateTopProducts(visitorUID, database);
          //  cf.RecalculateProductScores(visitorUID, database);
        }

        // GET "Update/database/password
        [Route("/update/{database}/{password}")]
        [HttpGet("{database}/{password}")]
        public void GetUpdate(string database, string password) {
            if (password == "supersecretpassword") {
                cf.BuildCollaborativeFilter(database.ToUpper());
            }
        }

        // GET "Updatevisitortopproducts/database/password
        [Route("/updatevisitortopproducts/{database}/{password}")]
        [HttpGet("{database}/{password}")]
        public void GetUpdateVisitorTopProducts(string database, string password) {
            if (password == "supersecretpassword") {
                pr.CalculateAllTopProducts(database.ToUpper());
            }
        }

        // GET "calculateTop20/database/password
        [Route("/calculatetop20/{database}/{password}")]
        [HttpGet("{database}/{password}")]
        public void CalculateTop20(string database, string password) {
            if (password == "supersecretpassword") {
                DateTime today = DateTime.Now;
                pr.CalculateMonthlyTop(database.ToUpper(), today.AddDays(-30));
            }
        }

        // GET "calculateTop20/database/password
        [Route("/calculatetop20test/{database}/{password}")]
        [HttpGet("{database}/{password}")]
        public void CalculateTop20Test(string database, string password) {
            if (password == "supersecretpassword") {
                DateTime today = new DateTime(2017, 3, 1);
                pr.CalculateMonthlyTop(database.ToUpper(), today.AddDays(-30));
            }
        }
    }
}
