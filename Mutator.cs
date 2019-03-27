using System;

namespace Lab1_C
{
    public interface IMutator
    {
        string Mutate(string input, int lowerBound, int upperBound);
    }

    public class VoidMutator : IMutator
    {
        public string Mutate(string input, int lowerBound, int upperBound)
        {
            return input;
        }
    }

    public class SimpleMutator : IMutator
    {
        private readonly Random _random = new Random();
        
        public string Mutate(string input, int lowerBound, int upperBound)
        {
            var newValue = _random.Next(lowerBound, upperBound);
            var index = _random.Next(input.Length / BinaryConvertor.IntBytes);
            var replaceFrom = index * BinaryConvertor.IntBytes;
            var replaceTo = replaceFrom + BinaryConvertor.IntBytes;

            return input.Substring(0, replaceFrom) +
                   BinaryConvertor.IntsToBinaryString(newValue) +
                   input.Substring(replaceTo);
        }
    }

    public class RepeatableMutator : IMutator
    {
        private readonly int _times;
        private readonly double _chance;
        private readonly Random _random;
        private readonly IMutator _mutator;

        public RepeatableMutator(int times, double chance)
        {
            _times = times;
            _chance = chance;
            _random = new Random();
            _mutator = new SimpleMutator();
        }

        public string Mutate(string input, int lowerBound, int upperBound)
        {
            var result = input;
            for (var i = 0; i < _times; ++i)
            {
                if (_random.NextDouble() <= _chance)
                {
                    result = _mutator.Mutate(result, lowerBound, upperBound);
                }
            }

            return result;
        }
    }
    
    public class AdaptiveMutator : IMutator
    {
        private double _times;
        private double _chance;
        private readonly double _deltaTimes;
        private readonly double _deltaChance;
        private readonly int _threshold;
        private readonly Random _random;
        private readonly IMutator _mutator;
        private int _called;

        /*
         * For each `Mutate` call that is less or equal to `threshold` increments amount of mutations
         * (`times`) by `deltaTimes` and decrements mutation chance by `deltaChance`.
         * For calls after `threshold` amount of mutations is reduced by the same value per call,
         * mutation chance is increased.
         */
        public AdaptiveMutator(int times, double chance,
            double deltaTimes = 0.01, double deltaChance = 0.005, int threshold = 2000)
        {
            _times = times;
            _chance = chance;
            _deltaTimes = deltaTimes;
            _deltaChance = deltaChance;
            _threshold = threshold;
            _random = new Random();
            _mutator = new SimpleMutator();
        }

        public void Reset()
        {
            _called = 0;
        }

        private int GetTimes()
        {
            var result = _times;
            _times += _called <= _threshold ? _deltaTimes : _deltaChance;
            return (int) result;
        }

        private double GetChance()
        {
            var result = _chance;
            _chance += _called <= _threshold ? -_deltaChance : _deltaChance;
            return result;
        }

        public string Mutate(string input, int lowerBound, int upperBound)
        {
            var result = input;
            var times = GetTimes();
            for (var i = 0; i < times; ++i)
            {
                if (_random.NextDouble() <= GetChance())
                {
                    result = _mutator.Mutate(result, lowerBound, upperBound);
                }
            }

            ++_called;

            return result;
        }
    }

    public class HackMutator : IMutator
    {
        private readonly int _times;
        private readonly double _chance;
        private readonly Random _random;

        public HackMutator(int times, double chance)
        {
            _times = times;
            _chance = chance;
            _random = new Random();
        }

        // With such approach best decision will be randomized a bit faster
        public string Mutate(string input, int lowerBound, int upperBound)
        {
            var result = input;
            for (var i = 0; i < _times; ++i)
            {
                if (!(_random.NextDouble() <= _chance))
                    continue;
                
                var index = _random.Next(input.Length / BinaryConvertor.IntBytes);
                var replaceFrom = index * BinaryConvertor.IntBytes;
                var replaceTo = replaceFrom + BinaryConvertor.IntBytes;
                var inner = input.Substring(replaceFrom, BinaryConvertor.IntBytes);
                var oldValue = Math.Abs(BinaryConvertor.BinaryStringToInts(inner)[0]);
                var newValue = _random.Next(-oldValue, oldValue);

                result = input.Substring(0, replaceFrom) +
                         BinaryConvertor.IntsToBinaryString(newValue) +
                         input.Substring(replaceTo);
            }

            return result;
        }
    }
}