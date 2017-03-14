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
        public void GetProductRecommendationValidArguments() {
            Assert.Equal(new string[] { "36991", "40786", "38104", "41594", "31573" }, rc.GetRecommendationForVisitor(validVisitorUID, 5, validDatabaseName));
        }

        [Fact]
        public void GetProductRecommendationNonExistingVisitorUID() {
            Assert.True(rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, validDatabaseName) == null);
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

        [Fact]
        public void NumberOfProductRecommendationsLargerThanAvailable() {
            Assert.Equal(56, rc.GetRecommendationForVisitor(validVisitorUID, 1000, validDatabaseName).Count<string>());
        }

        [Fact]
        public void NumberOfProductRecommendationsLessOrEqualZero() {
            Assert.True(rc.GetRecommendationForVisitor(validVisitorUID, 0, validDatabaseName) == null);
            Assert.True(rc.GetRecommendationForVisitor(validVisitorUID, -15, validDatabaseName) == null);
        }

        [Fact]
        public void NonExistingDatabase() {
            Assert.True(rc.GetRecommendationForVisitor(validVisitorUID, 5, nonExistingDatabaseName) == null);
        }

        [Fact]
        public void ExistingVisitorWithNoBehaviorData() {
            Assert.Equal(null, rc.GetRecommendationForVisitor(visitorWithoutBehaviorData, 5, validDatabaseName));
        }
    }
}