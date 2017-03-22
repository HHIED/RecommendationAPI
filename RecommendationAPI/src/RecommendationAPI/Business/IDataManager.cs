using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    interface IDataManager
    {

        void createVisitor(string visitorUID, string database);

    }
}
