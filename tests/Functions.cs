using System;
using System.Linq;
using NUnit.Framework;

namespace Lab1_C.tests
{
    [TestFixture]
    public class Functions
    {
        [Test]
        public void TestFunction<T>(int min, int max) where T : IFunction, new()
        {
            var function = new T();
            var random = new Random();
            var input = Enumerable.Range(0, 100).Select(_ => random.Next(min, max)).ToArray();
            var result = function.Compute(input);

            Assert.GreaterOrEqual(result, 0,
                $"Assertion failed for {function.GetType()}({string.Join(", ", input)})");
        }

        [Test]
        public void TestCache<T>(int min, int max) where T : IFunction, new()
        {
            var function = new T();
            var cachedFunction = new Cached<T>();
            var random = new Random();
            for (var i = 0; i < 4000; ++i)
            {
                var input = Enumerable.Range(0, 100).Select(_ => random.Next(min, max)).ToArray();
                var expected = function.Compute(input);
                var result = cachedFunction.Compute(input);
                var cachedResult = cachedFunction.Compute(input);

                Assert.AreEqual(expected, cachedResult);
                Assert.AreEqual(result, cachedResult);
            }
        }
    }
}