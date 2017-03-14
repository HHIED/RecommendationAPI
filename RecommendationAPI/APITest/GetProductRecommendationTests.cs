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

        public Tests() {
            pr = new ProductRecommender(new DatabaseEngine());
        }
    }
}
