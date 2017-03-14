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
using RecommendationAPI.Dummy;

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

        ProductRecommender pr;

        public Tests() {
            pr = new ProductRecommender(new DummyDatabase());
        }
        
        [Fact]
        public void NonExistingVisitor() {
            Exception ex = Assert.Throws<InvalidOperationException>(() => pr.GetProductRecommendations("noneExistingVisitorUID", 5, "ValidDatabase"));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ValidArguments() {
            string[] validProductRecommendations = pr.GetProductRecommendations("ValidVisitor", 3, "ValidDatabase");

            Assert.Equal(new string[] { "1234", "1235", "1238" }, validProductRecommendations);
        }

        [Fact]
        public void LowerCaseVisitorUID() {
            string[] validProductRecommendations = pr.GetProductRecommendations("validvisitor", 3, "ValidDatabase");

            Assert.Equal(new string[] { "1234", "1235", "1238" }, validProductRecommendations);
        }

        [Fact]
        public void UpperCaseVisitorUID() {
            string[] validProductRecommendations = pr.GetProductRecommendations("VALIDVISITOR", 3, "ValidDatabase");

            Assert.Equal(new string[] { "1234", "1235", "1238" }, validProductRecommendations);
        }

        [Fact]
        public void LowerCaseDatabase() {
            string[] validProductRecommendations = pr.GetProductRecommendations("ValidVisitor", 3, "validdatabase");

            Assert.Equal(new string[] { "1234", "1235", "1238" }, validProductRecommendations);
        }

        [Fact]
        public void UpperCaseDatabase() {
            string[] validProductRecommendations = pr.GetProductRecommendations("ValidVisitor", 3, "VALIDDATABASE");

            Assert.Equal(new string[] { "1234", "1235", "1238" }, validProductRecommendations);
        }
    }
}
