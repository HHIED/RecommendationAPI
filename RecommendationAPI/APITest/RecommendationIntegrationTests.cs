using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APITest {
    public class RecommendationIntegrationTests {
        private RecommendationController rc = new RecommendationController();
        private string validVisitorUID;
        private string nonExistingVisitorUID;
        private string validDatabaseName;
        private string uppercaseDatabaseName;
        private string lowercaseDatabaseName;
        private string nonExistingDatabaseName;
        private string visitorWithoutBehaviorData;

        public RecommendationIntegrationTests() {
            validVisitorUID = "AAF995AE-1DD0-41C6-898B-9CBEE884E553";
            nonExistingVisitorUID = "InvalidUID";
            validDatabaseName = "Pandashop";
            nonExistingDatabaseName = "InvalidDatabase";
            visitorWithoutBehaviorData = "6820EDD0-E6A6-4105-A078-0000127E7AE1";
        }

        [Fact]
        public void NonExistingVisitorRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, validDatabaseName);

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void NonExistingDatabaseRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, nonExistingDatabaseName);

            Assert.True(result.Length == 0);
        }

        [Fact]
        public void ValidArgumentsRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(validVisitorUID, 5, validDatabaseName);

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void UppercaseVisitorRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(validVisitorUID.ToUpper(), 5, validDatabaseName);

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void LowercaseVisitorRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(validVisitorUID.ToLower(), 5, validDatabaseName);

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void UppercaseDatabaseRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, validDatabaseName.ToUpper());

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void LowercaseDatabaseRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, 5, validDatabaseName.ToLower());

            Assert.True(result.Length == 5);
        }

        [Fact]
        public void TooLargeNumberOfRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, 100, validDatabaseName.ToUpper());

            Assert.True(result.Length<100 & result.Length>0);
        }

        [Fact]
        public void VisitorWithNoBehaviorRecommendations() {
            string[] result = rc.GetRecommendationForVisitor(visitorWithoutBehaviorData, 5, validDatabaseName.ToUpper());

            Assert.True(result.Length == 5);
        }

       /* [Fact]
        public void NumberOfProductRecommendationsLargerThanMaxInt() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, int.MaxValue+1, validDatabaseName.ToUpper());

            Assert.True(result.Length < int.MaxValue & result.Length > 0);
        }

        [Fact]
        public void NumberOfProductRecommendationsLessThanMinInt() {
            string[] result = rc.GetRecommendationForVisitor(nonExistingVisitorUID, int.MinValue-1, validDatabaseName.ToUpper());

            Assert.True(result.Length < 100 & result.Length > 0);
        }*/
    }
}