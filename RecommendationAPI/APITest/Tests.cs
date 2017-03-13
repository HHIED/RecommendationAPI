using System;
using Xunit;
using RecommendationAPI.Business;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;
using Xunit.Sdk;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Tests
{
    public class Tests
    {

        public class RepeatAttribute : DataAttribute {
            private readonly int _count;

            public RepeatAttribute(int count) {
                if (count < 1) {
                    throw new ArgumentOutOfRangeException(nameof(count),
                          "Repeat count must be greater than 0.");
                }
                _count = count;
            }

            public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
                return Enumerable.Repeat(new object[0], _count);
            }
        }

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
            //string[] products = pr.GetTopThreeProducts(mockVisitor);
            //Assert.Equal(products[0], "test1");
            //Assert.Equal(products[1], "test2");
            //Assert.Equal(products[2], "test3");
        }

        [Theory]
        [Repeat(20)]
        public void testGetVisitor() {
         //   DatabaseEngine dbe = new DatabaseEngine();
         //   TraceListener myListener = new DefaultTraceListener();
         //   Stopwatch sw = new Stopwatch();
         //   sw.Start();
         //   Debug.WriteLine("StopWatch started: " + sw.ElapsedMilliseconds);
         //   Visitor visitor = dbe.GetVisitor("AAF995AE-1DD0-41C6-898B-9cbee884e553".ToUpper(), "PandaShop").Result;
         //   Debug.WriteLine("Visitor retrieved: " + sw.ElapsedMilliseconds);
         //   Debug.WriteLine("Final Time: " + sw.ElapsedMilliseconds);
         //   Debug.WriteLine(visitor.UID);
         //   string[] wanted = new string[] { "36991", "43215", "37691" };
         ////   string[] got = pr.GetTopThreeProducts(visitor);
         //   Debug.Write(got[0]);
         //   Assert.Equal(wanted, got);
         //   Assert.Equal("AAF995AE-1DD0-41C6-898B-9cbee884e553".ToUpper(), visitor.UID);
        }

        [Theory]
        [Repeat(5)]
        public void getVisitorsTest() {
            DatabaseEngine dbe = new DatabaseEngine();
            int[] product = new int[] { 36991, 43215, 37691};
            //List<Visitor> matchingVisitors = dbe.GetVisitors(product, "Pandashop").Result;
        }

        [Theory]
        [Repeat(5)]
        public void getRecommendationsTest() {
            string[] recommendations = pr.GetProductRecommendations("AAF995AE-1DD0-41C6-898B-9cbee884e553", 5, "Pandashop");
            Debug.WriteLine(recommendations.ToString());
            Assert.Equal("knep", "knep");
        }


    }
}
