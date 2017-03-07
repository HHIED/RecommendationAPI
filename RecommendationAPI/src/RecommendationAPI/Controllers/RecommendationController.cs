using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecommendationController : Controller
    {
        // GET api/recommendation/getRecommendation?visitorUID=""
        [Route("getRecommendation")]
        [HttpGet("{visitorID}")]
        public string[] GetRecommendationForVisitor(string visitorUID) {
            return new string[0];
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
