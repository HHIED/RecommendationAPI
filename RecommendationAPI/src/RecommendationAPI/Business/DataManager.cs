using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class DataManager : IDataManager {

        private IDatabaseEngine _db;

        public DataManager(IDatabaseEngine db) {
            _db = db;
        }

        public void createVisitor(string visitorUID, string database) {
            _db.insertVisitor(visitorUID, database);
        }
    }
}
