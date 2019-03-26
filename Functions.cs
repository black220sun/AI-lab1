using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace Lab1_C
{
    public interface IFunction
    {
        double Compute(params int[] values);
    }

    public class Sphere : IFunction
    {
        public double Compute(params int[] values)
        {
            return values.Select(v => (double)v * v).Sum();
        }
    }

    public class Ackley : IFunction
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public Ackley(): this(20) {}
        
        public Ackley(double a = 20, double b = 0.2, double c = Math.PI * 2)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public double Compute(params int[] values)
        {
            double d = values.Length;
            var tmp = values.Select(v => (double)v * v).Average();
            var part1 = -_a * Math.Exp(-_b * Math.Sqrt(tmp));
            var part2 = - Math.Exp(values.Select(v => Math.Cos(_c * v)).Sum() / d);
            return part1 + part2 + _a + Math.E;
        }

       
    }
    
    public class Griewank : IFunction
    {
        public double Compute(params int[] values)
        {
            var sum = values.Select(v => v * v / 4000d).Sum();
            var product = values.Select((v, i) => Math.Cos(v / Math.Sqrt(i + 1)))
                .Aggregate(1d, (res, v) => res * v);
            return sum - product + 1;
        }
    }
        
    public class Rastrigin : IFunction
    {
        public double Compute(params int[] values)
        {
            double d = values.Length;
            const double pi2 = Math.PI * 2;
            return 10 * d + values.Select(v => v * v - 10 * Math.Cos(pi2 * v)).Sum();
        }
    }

    public class Rosenbrock : IFunction
    {
        public double Compute(params int[] values)
        {
            var result = 0d;
            for (var i = 0; i < values.Length - 1; ++i)
            {
                var tmp = values[i];
                var part1 = values[i + 1] - tmp * tmp;
                --tmp;
                var part2 = tmp * tmp;
                result += 100 * part1 * part1 + part2;
            }
            return result;
        }
    }

    public class Cached<T> : IFunction where T : IFunction, new()
    {
        private readonly Hashtable _cache = new Hashtable();
        private readonly IFunction _function = new T();
        public double Compute(params int[] values)
        {
            var result = _cache[values];
            if (result != null)
            {
                return (double)result;
            }

            result = _function.Compute(values);
            _cache[values] = result;
            return (double)result;
        }
    }
}