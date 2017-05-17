using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationAPI.Controllers;
using RecommendationAPI.Business;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APITest
{
    public class IntegrationTests
    {
        private RecommendationController rc = new RecommendationController();
        private DataController dc = new DataController();
        private DatabaseEngine db = new DatabaseEngine();
        
        private string validVisitorUID;
        private string nonExistingVisitorUID;
        private string validDatabaseName;
        private string uppercaseDatabaseName;
        private string lowercaseDatabaseName;
        private string nonExistingDatabaseName;
        private int existingProduct = 52550;
        private int existingProductGroup = 825;

        public IntegrationTests() {
            validVisitorUID = "AAF995AE-1DD0-41C6-898B-9CBEE884E553";
            nonExistingVisitorUID = "InvalidUID";
            validDatabaseName = "PANDASHOP";
            nonExistingDatabaseName = "InvalidDatabase";
            
        }

        [Fact]
        public void createVisitor() {
            dc.PutVisitor("TestVisitorUID", validDatabaseName);
            Visitor v = db.GetVisitor("TESTVISITORUID", validDatabaseName).Result;
            Assert.NotNull(v);
        }

        [Fact]
        public void createVisitorDuplicate() {
            dc.PutVisitor(validVisitorUID, validDatabaseName);
        }

        [Fact]
        public void createVisitorNonExistingDatabase() {
            dc.PutVisitor(validVisitorUID, nonExistingDatabaseName);
        }

        [Fact]
        public void createProduct() {
            dc.PutProduct(123, "description", existingProductGroup, validDatabaseName);
            Product p = db.GetProduct(123, validDatabaseName).Result;
            Assert.NotNull(p);
        }

        [Fact]
        public void createProductDuplicate() {
            dc.PutProduct(existingProduct, "description", existingProductGroup, validDatabaseName);
        }

        [Fact]
        public void createProductNonExistingDatabase() {
            dc.PutProduct(existingProduct, "description", existingProductGroup, nonExistingDatabaseName);
        }

        [Fact]
        public void createBehavior() {
            dc.PutBehavior("TESTVISITOR", "productview", 123, validDatabaseName);
            Visitor v = db.GetVisitor("TESTVISITOR", validDatabaseName).Result;
            Assert.NotNull(v.Behaviors);
        }

        [Fact]
        public void createBehaviorNonExistingDatabase() {
            dc.PutBehavior("TESTVISITOR", "productview", 123, nonExistingDatabaseName);
        }


    }
}