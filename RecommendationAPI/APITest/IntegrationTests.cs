using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationAPI.Controllers;
using RecommendationAPI.Business;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

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
            HttpStatusCode status = dc.PutVisitor("TestVisitorUID", validDatabaseName);
            Assert.Equal(HttpStatusCode.Created, status);
        }

        [Fact]
        public void createVisitorDuplicate() {
            HttpStatusCode status = dc.PutVisitor(validVisitorUID, validDatabaseName);
            Assert.Equal(HttpStatusCode.BadRequest, status);
        }

        [Fact]
        public void createVisitorNonExistingDatabase() {
            HttpStatusCode status = dc.PutVisitor(validVisitorUID, nonExistingDatabaseName);
            Assert.Equal(HttpStatusCode.BadRequest, status);
        }

        [Fact]
        public void createProduct() {
            HttpStatusCode status = dc.PutProduct(123, "description", existingProductGroup, validDatabaseName);
            Assert.Equal(HttpStatusCode.Created, status);
        }

        [Fact]
        public void createProductDuplicate() {
            HttpStatusCode status = dc.PutProduct(existingProduct, "description", existingProductGroup, validDatabaseName);
            Assert.Equal(HttpStatusCode.BadRequest, status);
        }

        [Fact]
        public void createProductNonExistingDatabase() {
            HttpStatusCode status = dc.PutProduct(existingProduct, "description", existingProductGroup, nonExistingDatabaseName);
            Assert.Equal(HttpStatusCode.BadRequest, status);
        }

        [Fact]
        public void createBehavior() {
            HttpStatusCode status = dc.PutBehavior("TESTVISITOR", "productview", 123, validDatabaseName);
            Assert.Equal(HttpStatusCode.OK, status);
        }

        [Fact]
        public void createBehaviorNonExistingDatabase() {
            HttpStatusCode status= dc.PutBehavior("TESTVISITOR", "productview", 123, nonExistingDatabaseName);
            Assert.Equal(HttpStatusCode.BadRequest, status);
        }


    }
}