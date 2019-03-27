using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1_C
{
    public interface IDebugFormatter
    {
        void Print(IEnumerable<string> population, Func<string, double> fitnessLambda);
    }

    public class VoidDebugFormatter : IDebugFormatter
    {
        public void Print(IEnumerable<string> population, Func<string, double> fitnessLambda) { }
    }

    public class CycleCounter : IDebugFormatter
    {
        private int _cycle;
        private double _expected;

        public CycleCounter(double expected)
        {
            _expected = expected;
        }

        public void Print(IEnumerable<string> population, Func<string, double> fitnessLambda)
        {
            if (population.Select(fitnessLambda).Min() <= _expected)
            {
                Console.WriteLine($"{_cycle} cycles");
            }

            ++_cycle;
        }
    }
    
    public class SimpleDebugFormatter : IDebugFormatter
    {
        private int _cycle;
        private double _lastFitness = int.MaxValue;
        
        public void Print(IEnumerable<string> population, Func<string, double> fitnessLambda)
        {
            var values = population.Select(fitnessLambda).ToArray();
            var average = values.Average();
            var best = values.Min();
            var diff = (average - _lastFitness) / _lastFitness * 100;
            Console.WriteLine($"CYCLE {_cycle}: average fitness = {average} ({diff}%), best: {best}");
            _lastFitness = average;
            ++_cycle;
        }
    }

    public class ExtendedDebugFormatter : IDebugFormatter
    {
        private int _cycle;
        private double _lastFitness = int.MaxValue;

        public void Print(IEnumerable<string> population, Func<string, double> fitnessLambda)
        {
            Console.WriteLine($"CYCLE {_cycle}");
            var total = 0d;
            foreach (var child in population)
            {
                foreach (var integer in BinaryConvertor.BinaryStringToInts(child))
                {
                    Console.Write($"{integer} ");
                }

                var fitness = fitnessLambda(child);
                total += fitness;
                Console.WriteLine($": {fitness}");
            }

            var average = total / population.Count();
            var diff = (average - _lastFitness) / _lastFitness * 100;
            Console.WriteLine($"Average fitness: {average} ({diff}%)\n");
            _lastFitness = average;
            ++_cycle;
        }
    }
}