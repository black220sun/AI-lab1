using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab1_C
{
    public class Algorithm
    {
        private readonly Random _random;
        private AlgorithmOptions _options;

        public Algorithm() : this(new AlgorithmOptions()) { }
        public Algorithm(AlgorithmOptions options) : this(new Random(), options) { }

        public Algorithm(Random random, AlgorithmOptions options)
        {
            _random = random;
            _options = options;
        }

        public string GenerateDecision(int lowerBound, int upperBound, int size)
        {
            var values = new int[size];
            for (var i = 0; i < size; ++i)
            {
                values[i] = _random.Next(lowerBound, upperBound);
            }

            return BinaryConvertor.IntsToBinaryString(values);
        }

        private bool IsValid(string population)
        {
            var ints = BinaryConvertor.BinaryStringToInts(population);
            return ints.All(i => i >= _options.LowerBound && i <= _options.UpperBound);
        }
        
        public string Mutate(string input)
        {
            var newValue = _random.Next(_options.LowerBound, _options.UpperBound);
            var index = _random.Next(input.Length / BinaryConvertor.IntBytes);
            var replaceFrom = index * BinaryConvertor.IntBytes;
            var replaceTo = replaceFrom + BinaryConvertor.IntBytes;

            return input.Substring(0, replaceFrom) +
                   BinaryConvertor.IntsToBinaryString(newValue) +
                   input.Substring(replaceTo);
        }

        private int[] DistinctValues(int amount, int max, int min = 0)
        {
            var values = new List<int>();
            for (var i = 0; i < amount; ++i)
            {
                int next;
                do
                {
                    next = _random.Next(min, max);
                } while (values.Contains(next));
                
                values.Add(next);

            }

            return values.ToArray();
        }

        public double Fitness(IFunction function, string input)
        {
            return function.Compute(BinaryConvertor.BinaryStringToInts(input));
        }

        private sealed class Pair : IComparable
        {
            internal int First { get; }
            private double Second { get; }

            internal Pair(int first, double second)
            {
                First = first;
                Second = second;
            }

            public int CompareTo(object other)
            {
                var otherPair = other as Pair;
                if (ReferenceEquals(this, otherPair)) return 0;
                if (ReferenceEquals(null, otherPair)) return 1;
                if (ReferenceEquals(null, this)) return -1;
                return Second.CompareTo(otherPair.Second);
            }
        }

        public string Tournament(IFunction function, List<string> candidates, int amount = 2)
        {
            var size = candidates.Count;
            if (size == 0)
                throw new InvalidDataException("No candidates passed");
            if (amount > size)
                amount = size;
            var indexes = DistinctValues(amount, size);
            var winnerIndex = indexes.Select(index => new Pair(index, Fitness(function, candidates[index])))
                .Min().First;
            return candidates[winnerIndex];
        }

        public KeyValuePair<string, string> Crossover(string x, string y)
        {
            var size = x.Length;
            if (size != y.Length)
                throw new InvalidDataException("Inputs must have the same size");
            if (size <= 2)
                throw new InvalidDataException("Input size must be greater than 2");
            while (true)
            {
                var firstIndex = _random.Next(1, size - 2);
                var secondIndex = _random.Next(1, size - firstIndex - 1);
                var first = x.Substring(0, firstIndex) +
                       y.Substring(firstIndex, secondIndex) +
                       x.Substring(secondIndex + firstIndex);
                var second = y.Substring(0, firstIndex) +
                            x.Substring(firstIndex, secondIndex) +
                            y.Substring(secondIndex + firstIndex);
                if (_options.StrictCrossover)
                {
                    if (IsValid(first) && IsValid(second))
                    {
                        return new KeyValuePair<string, string>(first, second);
                    }
                }
                else
                {
                    return new KeyValuePair<string, string>(first, second);
                }
            }
        }

        public string Execute(IFunction function)
        {
            var population = Enumerable.Range(0, _options.PopulationSize)
                .Select(_ => GenerateDecision(_options.LowerBound, _options.UpperBound, _options.Arity))
                .ToList();
            _options.DebugFormatter.Print(population, s => Fitness(function, s));
            var expected = _options.ExpectedValue;
            string result;
            do
            {
                population = Crossover(function, population);
                _options.DebugFormatter.Print(population, s => Fitness(function, s));
                result = population.FirstOrDefault(s => Fitness(function, s) <= expected);
            } while (result == null);

            return result;
        }

        private List<string> Crossover(IFunction function, List<string> population)
        {
            var size = population.Count;
            var results = new List<string>(population.Count);
            for (var i = 0; i < size / 2; ++i)
            {
                var first = Tournament(function, population, _options.TournamentSize);
                var second = Tournament(function, population, _options.TournamentSize);

                var result = Crossover(first, second);
                var mutated1 = _options.Mutator.Mutate(result.Key, _options.LowerBound, _options.UpperBound);
                var mutated2 = _options.Mutator.Mutate(result.Value, _options.LowerBound, _options.UpperBound);
                results.Add(mutated1);
                results.Add(mutated2);
            }

            return results;
        }
    }

    public class AlgorithmOptions
    {
        internal int Arity { get; }
        internal int LowerBound { get; }
        internal int UpperBound { get; }
        internal int PopulationSize { get; }
        internal int TournamentSize { get; }
        internal IMutator Mutator { get; }
        internal bool StrictCrossover { get; }
        internal double ExpectedValue { get; }
        internal IDebugFormatter DebugFormatter { get; }

        public AlgorithmOptions(
            // input array size
            int arity = 50,
            int lowerBound = -10,
            int upperBound = 10,
            int populationSize = 200,
            int tournamentSize = 3,
            // `false` may speed-up computation but generate out-of-range value
            bool strictCrossover = true,
            IMutator mutator = null,
            IDebugFormatter debugFormatter = null,
            double expectedValue = 0d
        )
        {
            Arity = arity;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            PopulationSize = populationSize;
            TournamentSize = tournamentSize;
            StrictCrossover = strictCrossover;
            Mutator = mutator ?? new RepeatableMutator(20, 0.015);
            DebugFormatter = debugFormatter ?? new CycleCounter(expectedValue);
            ExpectedValue = expectedValue;
        }
    }
}