using System;
using System.Linq;
using Lab1_C.tests;

namespace Lab1_C
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            var convertorTest = new Convertor();
            var functionsTest = new Functions();
            var algorithmTest = new tests.Algorithm();
            
            convertorTest.TestIntsToString();
            convertorTest.TestStringToInts();
            
            for (var i = 0; i < 100; ++i)
            {
                functionsTest.TestFunction<Sphere>(-100, 100);
                functionsTest.TestFunction<Ackley>(-32, 32);
                functionsTest.TestFunction<Griewank>(-600, 600);
                functionsTest.TestFunction<Rastrigin>(-5, 5);
                functionsTest.TestFunction<Rosenbrock>(-5, 10);
            }
            functionsTest.TestCache<Sphere>(-100, 100);
            functionsTest.TestCache<Ackley>(-32, 32);
            functionsTest.TestCache<Griewank>(-600, 600);
            functionsTest.TestCache<Rastrigin>(-5, 5);
            functionsTest.TestCache<Rosenbrock>(-5, 10);
            
            algorithmTest.TestTournament();
            algorithmTest.TestBinaryTournament();
            algorithmTest.TestMutate();
            algorithmTest.TestCrossover();

            Compute<Sphere>(new AlgorithmOptions(
                lowerBound: -100, 
                upperBound: 100
            ));
            Compute<Ackley>(new AlgorithmOptions(
                lowerBound: -32, 
                upperBound: 32,
                // something like -4.44e-16
                expectedValue: new Ackley().Compute(Enumerable.Range(0, 20).Select(v => 0).ToArray()),
                arity: 20
            ));
            Compute<Griewank>(new AlgorithmOptions(
                lowerBound: -600, 
                upperBound: 600,
                populationSize: 90,
                tournamentSize: 12,
                mutator: new RepeatableMutator(38, 0.04)
            ));
            Compute<Rastrigin>(new AlgorithmOptions(
                lowerBound: -5, 
                upperBound: 5,
                mutator: new RepeatableMutator(10, 0.02),
                arity: 30
            ));
            Compute<Rosenbrock>(new AlgorithmOptions(
                lowerBound: -5, 
                upperBound: 10,
                arity: 30
            ));
        }

        private static void Compute<TF>(AlgorithmOptions options, bool useCache = true) where TF : IFunction, new()
        {
            var function = useCache ? (IFunction) new Cached<TF>() : new TF();
            var solved = new Algorithm(options).Execute(function);
            var result = BinaryConvertor.BinaryStringToInts(solved);
            Console.WriteLine($"Computed {function.GetType()}: {string.Join(", ", result)}");
        }
    }
}