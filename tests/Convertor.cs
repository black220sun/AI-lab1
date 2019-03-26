using NUnit.Framework;

namespace Lab1_C.tests
{
    [TestFixture]
    public class Convertor
    {
        [Test]
        public void TestZeroToString()
        {
            var expected = "".PadLeft(BinaryConvertor.IntBytes, '0');
            var result = BinaryConvertor.IntsToBinaryString(0);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestIntToString()
        {
            var expected = "1011010".PadLeft(BinaryConvertor.IntBytes, '0');
            var result = BinaryConvertor.IntsToBinaryString(0x5A);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestZerosToString()
        {
            var expected = "".PadLeft(BinaryConvertor.IntBytes * 4, '0');
            var result = BinaryConvertor.IntsToBinaryString(0, 0, 0, 0);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestIntsToString()
        {
            var int1 = "1011010".PadLeft(BinaryConvertor.IntBytes, '0');
            var int2 = "".PadLeft(BinaryConvertor.IntBytes, '0');
            var int3 = "11111111".PadLeft(BinaryConvertor.IntBytes, '0');
            var int4 = "1010101111001101".PadLeft(BinaryConvertor.IntBytes, '0');
            var expected = int1 + int2 + int3 + int4;
            var result = BinaryConvertor.IntsToBinaryString(0x5A, 0x0, 0xFF, 0xABCD);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestStringToZero()
        {
            var input = "".PadLeft(BinaryConvertor.IntBytes, '0');
            var expected = new[] {0};
            var result = BinaryConvertor.BinaryStringToInts(input);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestStringToInt()
        {
            var input = "1011010".PadLeft(BinaryConvertor.IntBytes, '0');
            var expected = new[] {0x5A};
            var result = BinaryConvertor.BinaryStringToInts(input);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestStringToZeros()
        {
            var input = "".PadLeft(BinaryConvertor.IntBytes * 4, '0');
            var expected = new[] {0, 0, 0, 0};
            var result = BinaryConvertor.BinaryStringToInts(input);
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void TestStringToInts()
        {
            var int1 = "1011010".PadLeft(BinaryConvertor.IntBytes, '0');
            var int2 = "".PadLeft(BinaryConvertor.IntBytes, '0');
            var int3 = "11111111".PadLeft(BinaryConvertor.IntBytes, '0');
            var int4 = "1010101111001101".PadLeft(BinaryConvertor.IntBytes, '0');
            var input = int1 + int2 + int3 + int4;
            var expected = new[] {0x5A, 0x0, 0xFF, 0xABCD};
            var result = BinaryConvertor.BinaryStringToInts(input);
            
            Assert.AreEqual(expected, result);
        }
    }
}