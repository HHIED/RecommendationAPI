using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APITest
{
    public class IntegrationTests
    {
        private RecommendationController rc = new RecommendationController();
        private string validVisitorUID;
        private string nonExistingVisitorUID;
        private string validDatabaseName;
        private string uppercaseDatabaseName;
        private string lowercaseDatabaseName;
        private string nonExistingDatabaseName;
        private string visitorWithoutBehaviorData;

        public IntegrationTests() {
            validVisitorUID = "AAF995AE-1DD0-41C6-898B-9CBEE884E553";
            nonExistingVisitorUID = "InvalidUID";
            validDatabaseName = "Pandashop";
            nonExistingDatabaseName = "InvalidDatabase";
            visitorWithoutBehaviorData = "6820EDD0-E6A6-4105-A078-0000127E7AE1";
        }

        [Fact]
        public void InsertNonExistingVisitor() {
            
        }

    }
}