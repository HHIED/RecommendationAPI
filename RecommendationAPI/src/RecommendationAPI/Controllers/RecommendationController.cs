using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecommendationAPI.Business;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers
{
    [Route("[controller]")]
    public class RecommendationController : Controller
    {

        private ProductRecommender pr = new ProductRecommender(new DatabaseEngine());

        // GET api/recommendation/getRecommendation?visitorUID=""
        [Route("/{visitorUID}/{numberOfRecommendations}/{database}")]
        [HttpGet("{visitorUID}/{numberOfRecommendations}/{database}")]
        public string[] GetRecommendationForVisitor(string visitorUID, int numberOfRecommendations, string database) {

            if (numberOfRecommendations <= 0 || numberOfRecommendations > int.MaxValue) {
                    return null;
                }

            try {
                string[] productRecommendation = pr.GetProductRecommendations(visitorUID.ToUpper(), numberOfRecommendations, database.ToUpper());
                return productRecommendation;
            } catch(InvalidOperationException ioe) {
                return null;
            }
            
        }
       
    }
}
