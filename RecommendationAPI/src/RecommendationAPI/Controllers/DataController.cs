using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecommendationAPI.Business;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationAPI.Controllers
{

    [Route("api/[controller]")]
    public class DataController : Controller
    {

        private IDataManager dm = new DataManager(new DatabaseEngine());


        // PUT visitorUID/database
        [Route("/{visitorUID}/{database}")]
        [HttpPut("{visitorUID}/{database}")]
        public void Put(string visitorUID, string database)
        {
            dm.createVisitor(visitorUID.ToUpper(), database.ToUpper());
        }
    }
}
