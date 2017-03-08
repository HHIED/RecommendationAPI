using System;
using Xunit;
using RecommendationAPI.Business;
using System.Diagnostics;

namespace Tests
{
    public class Tests
    {
        private Visitor mockVisitor;
        private Behavior[] mockBehavior;
        private ProductRecommender pr;

        public Tests() {
            //Behavior b1 = new Behavior(Behavior.@string.ProductView, "test1", new DateTime());
            //Behavior b2 = new Behavior(Behavior.@string.ProductView, "test4", new DateTime());
            //Behavior b3 = new Behavior(Behavior.@string.ProductView, "test3", new DateTime());
            //Behavior b4 = new Behavior(Behavior.@string.ProductView, "test1", new DateTime());
            //Behavior b5 = new Behavior(Behavior.@string.ProductView, "test1", new DateTime());
            //Behavior b6 = new Behavior(Behavior.@string.ProductView, "test2", new DateTime());
            //Behavior b7 = new Behavior(Behavior.@string.ProductView, "test2", new DateTime());
            //Behavior b8 = new Behavior(Behavior.@string.ProductView, "test3", new DateTime());
            //Behavior b9 = new Behavior(Behavior.@string.ProductView, "test1", new DateTime());
            //Behavior b10 = new Behavior(Behavior.@string.ProductView, "test2", new DateTime());
            //mockBehavior = new Behavior[] { b1, b2, b3, b4, b5, b6, b7, b8, b9, b10 };

            //mockVisitor = new Visitor("visitor1", "profile1", "customer1", mockBehavior);

            pr = new ProductRecommender(new DatabaseEngine());
        }
        [Fact]
        public void getTopThreeProductsTest() 
        {
            string[] products = pr.GetTopThreeProducts(mockVisitor);
            Assert.Equal(products[0], "test1");
            Assert.Equal(products[1], "test2");
            Assert.Equal(products[2], "test3");
        }

        [Fact]
        public void testGetVisitor() {
            DatabaseEngine dbe = new DatabaseEngine();
            Visitor visitor = dbe.GetVisitor("AAF995AE-1DD0-41C6-898B-9cbee884e553".ToUpper(), "PandaShop").Result;
            string[] wanted = new string[] { "241", "1014", "43215" };
            Assert.Equal(wanted, pr.GetTopThreeProducts(visitor));
            Assert.Equal("AAF995AE-1DD0-41C6-898B-9cbee884e553".ToUpper(), visitor.UID);
        }
    }
}
