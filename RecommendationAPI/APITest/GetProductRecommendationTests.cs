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

        ProductRecommender pr;

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

        public Tests() {
            pr = new ProductRecommender(new DummyDatabase());
        }

        [Fact]
        public void noBehavior() {

            Assert.Equal(new string[] { "" }, pr.GetProductRecommendations("ValidVisitorNoBehavior", 5, "ValidDatabase"));
        }

        [Fact]
        public void nonExistingDatabase() {
            Exception ex = Assert.Throws<InvalidOperationException>(() => pr.GetProductRecommendations("validVisitor", 5, "notValidDatabase"));

            Assert.IsType<InvalidOperationException>(ex);
        }

    }
    }
