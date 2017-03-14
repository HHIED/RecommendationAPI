using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

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

        public IntegrationTests() {
            validVisitorUID = "AAF995AE-1DD0-41C6-898B-9CBEE884E553";
            nonExistingVisitorUID = "InvalidUID";
            validDatabaseName = "Pandashop";
            nonExistingDatabaseName = "InvalidDatabase";
        }

        [Fact]
        public void GetProductRecommendationValidArguments() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID, 5, validDatabaseName));
        }

        [Fact]
        public void GetProductRecommendationNonExistingVisitorUID() {
            Assert.IsType<BadRequestResult>(rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, validDatabaseName));
        }

        [Fact]
        public void GetProductRecommendationUppercaseVisitorUID() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID.ToUpper(), 5, validDatabaseName));
        }

        [Fact]
        public void GetProductRecommendationLowercaseVisitorUID() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID.ToLower(), 5, validDatabaseName));
        }

        [Fact]
        public void GetProductRecommendationUppercaseDatabaseName() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID, 5, validDatabaseName.ToUpper()));
        }

        [Fact]
        public void GetProductRecommendationLowercaseDatabaseName() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID, 5, validDatabaseName.ToLower()));
        }

    }
}
