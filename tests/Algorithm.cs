using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Lab1_C.tests
{
    [TestFixture]
    public class Algorithm
    {
        [Test]
        public void TestTournament()
        {
            var candidates = new[]
            {
                BinaryConvertor.IntsToBinaryString(1, 1, 1, 1),
                BinaryConvertor.IntsToBinaryString(0, 0, 0, 0),
                BinaryConvertor.IntsToBinaryString(1, 0, 1, 0),
                BinaryConvertor.IntsToBinaryString(1, 1, 0, 1)
            }.ToList();
            var expected = candidates[1];
            var result = new Lab1_C.Algorithm().Tournament(new Sphere(), candidates, 4);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestBinaryTournament()
        {
            var candidates = new[]
            {
                BinaryConvertor.IntsToBinaryString(12, 1, 10, 11),
                BinaryConvertor.IntsToBinaryString(10, 0, 13, 0),
                BinaryConvertor.IntsToBinaryString(8, 8, 0, 5)
            }.ToList();
            var algorithm = new Lab1_C.Algorithm();
            
            for (var i = 0; i < 100; ++i)
            {
                var result = algorithm.Tournament(new Sphere(), candidates);
                Assert.AreNotEqual(candidates[0], result);
            }

        }

        [Test]
        public void TestMutate()
        {
            var input = BinaryConvertor.IntsToBinaryString(4, -123, 7);
            var expected = input.Length;
            var algorithm = new Lab1_C.Algorithm();
            for (var i = 1; i < 20; ++i)
            {
                var result = algorithm.Mutate(input).Length;
                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void TestCrossover()
        {
            const string x = "000";
            const string y = "010";
            var expected = new KeyValuePair<string, string>(y, x);
            var result = new Lab1_C.Algorithm(new AlgorithmOptions(strictCrossover: false)).Crossover(x, y);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestCrossoverLength()
        {
            var x = BinaryConvertor.IntsToBinaryString(1, 4, 6);
            var y = BinaryConvertor.IntsToBinaryString(211, 0, 42);
            var expected = x.Length;
            var algorithm = new Lab1_C.Algorithm();

            for (var i = 0; i < 100; ++i)
            {
                var result1 = algorithm.Crossover(x, y).Key.Length;
                var result2 = algorithm.Crossover(x, y).Value.Length;
                
                Assert.AreEqual(expected, result1);
                Assert.AreEqual(expected, result2);
            }
        }
    }
}