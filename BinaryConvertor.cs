using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab1_C
{
    public class BinaryConvertor
    {
        public const int IntBytes = sizeof(int) * 8;
        public static string IntsToBinaryString(params int[] values)
        {
            return string.Join("",
                values.Select(v => Convert.ToString(v, 2))
                .Select(v => v.PadLeft(IntBytes, '0')));
        }

        private static IEnumerable<string> Slice(string input)
        {
            var startIndex = -IntBytes;
            return Enumerable.Range(0, input.Length / IntBytes)
                .Select(_ => input.Substring(startIndex += IntBytes, IntBytes));
        }
        
        public static int[] BinaryStringToInts(string value)
        {
            if (value.Length % IntBytes != 0)
                throw new InvalidDataException($"Wrong input size, expected {IntBytes}*X");
            return Slice(value).Select(v => Convert.ToInt32(v, 2)).ToArray();
        }
    }
}