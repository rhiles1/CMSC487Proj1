using System;
using System.Collections;
using System.Collections.Generic;
using SDES;
using NUnit.Framework;

namespace Test
{
    public class Tests
    {
        private ISdes _sdes;

        [SetUp]
        public void Setup()
        {
            _sdes = new Sdes();
        }

        /// <summary>
        /// Test GetKeys method
        /// </summary>
        [Test]
        public void TestGetKeys()
        {
            var key = Helpers.StringToBit("1111100000"); // 10 bit key

            var keys = _sdes.GetKeys(key);

            foreach (var x in keys)
            {
                Helpers.PrintBinary(x);
            }
        }

        private const string EncDecKey = "1011001111";
        private const string EncDecPlain = "00000000";
        private const string EncDecCipher = "01100000";

        /// <summary>
        /// Simple Encryption test
        /// </summary>
        [Test]
        public void TestEnc()
        {
            var key = Helpers.StringToBit(EncDecKey); // 10 bit key

            var plainText = Helpers.StringToBit(EncDecPlain); // 8 plain text
            var outPut = _sdes.Crypt(plainText, key, true);

            Helpers.PrintBinary(outPut);
            Assert.AreEqual(outPut, Helpers.StringToBit(EncDecCipher));
        }

        /// <summary>
        /// Simple Decryption test
        /// </summary>
        [Test]
        public void TestDec()
        {
            var key = Helpers.StringToBit(EncDecKey); // 10 bit key

            var plainText = Helpers.StringToBit(EncDecCipher); // 8 plain text
            var outPut = _sdes.Crypt(plainText, key, false);

            Helpers.PrintBinary(outPut);
            Assert.AreEqual(outPut, Helpers.StringToBit(EncDecPlain));
        }

        /// <summary>
        /// Encryption test
        /// </summary>
        [Test]
        public void TestBench1()
        {
            var expectedOut = new List<BitArray>(new BitArray[8])
            {
                [0] = Helpers.StringToBit("10101000"),
                [1] = Helpers.StringToBit("10111110"),
                [2] = Helpers.StringToBit("00010110"),
                [3] = Helpers.StringToBit("01001010"),
                [4] = Helpers.StringToBit("01001001"),
                [5] = Helpers.StringToBit("01001110"),
                [6] = Helpers.StringToBit("00010101"),
                [7] = Helpers.StringToBit("01101000")
            };

            for (var i = 0; i < 8; i++)
            {
                var key = Helpers.StringToBit("0000000000"); // 10 bit key

                var plainText = new BitArray(8, false) {[i] = true};
                var outPut = _sdes.Crypt(plainText, key, true);

                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Encryption test
        /// </summary>
        [Test]
        public void TestBench2()
        {
            var expectedOut = new List<BitArray>(new BitArray[8])
            {
                [0] = Helpers.StringToBit("10000000"),
                [1] = Helpers.StringToBit("01000000"),
                [2] = Helpers.StringToBit("00100000"),
                [3] = Helpers.StringToBit("00010000"),
                [4] = Helpers.StringToBit("00001000"),
                [5] = Helpers.StringToBit("00000100"),
                [6] = Helpers.StringToBit("00000010"),
                [7] = Helpers.StringToBit("00000001")
            };

            var plainText = new List<BitArray>(new BitArray[8])
            {
                [0] = Helpers.StringToBit("10101000"),
                [1] = Helpers.StringToBit("10111110"),
                [2] = Helpers.StringToBit("00010110"),
                [3] = Helpers.StringToBit("01001010"),
                [4] = Helpers.StringToBit("01001001"),
                [5] = Helpers.StringToBit("01001110"),
                [6] = Helpers.StringToBit("00010101"),
                [7] = Helpers.StringToBit("01101000")
            };

            for (var i = 0; i < 8; i++)
            {
                var key = Helpers.StringToBit("0000000000"); // 10 bit key

                var outPut = _sdes.Crypt(plainText[i], key, true);

                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Encryption test
        /// </summary>
        [Test]
        public void TestBench3()
        {
            var expectedOut = new List<BitArray>(new BitArray[10])
            {
                [0] = Helpers.StringToBit("01100001"),
                [1] = Helpers.StringToBit("00010011"),
                [2] = Helpers.StringToBit("01001111"),
                [3] = Helpers.StringToBit("11100101"),
                [4] = Helpers.StringToBit("01100101"),
                [5] = Helpers.StringToBit("01011100"),
                [6] = Helpers.StringToBit("10101110"),
                [7] = Helpers.StringToBit("11011001"),
                [8] = Helpers.StringToBit("10101010"),
                [9] = Helpers.StringToBit("01001110")
            };

            for (var i = 0; i < 10; i++)
            {
                var key = new BitArray(10, false) {[i] = true};

                Console.Write("Key: ");
                Helpers.PrintBinary(key);

                var plainText = Helpers.StringToBit("00000000"); // 8 plain text
                var outPut = _sdes.Crypt(plainText, key, true);

                Console.Write("Cipher text: ");
                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Encryption test
        /// </summary>
        [Test]
        public void TestBench4()
        {
            var expectedOut = new List<BitArray>(new BitArray[10])
            {
                [0] = Helpers.StringToBit("00000011"),
                [1] = Helpers.StringToBit("00100010"),
                [2] = Helpers.StringToBit("01000000"),
                [3] = Helpers.StringToBit("01100000")
            };

            var key = new List<BitArray>(new BitArray[4])
            {
                [0] = Helpers.StringToBit("0000000011"),
                [1] = Helpers.StringToBit("0011001010"),
                [2] = Helpers.StringToBit("0001011001"),
                [3] = Helpers.StringToBit("1011001111")
            };

            for (var i = 0; i < 4; i++)
            {
                Console.Write("Key: ");
                Helpers.PrintBinary(key[i]);

                var plainText = Helpers.StringToBit("00000000"); // 8 plain text
                var outPut = _sdes.Crypt(plainText, key[i], true);

                Console.Write("Cipher text: ");
                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Encryption test
        /// </summary>
        [Test]
        public void TestBench5()
        {
            var expectedOut = new List<BitArray>(new BitArray[7])
            {
                [0] = Helpers.StringToBit("10000111"),
                [1] = Helpers.StringToBit("10110110"),
                [2] = Helpers.StringToBit("10110100"),
                [3] = Helpers.StringToBit("00110011"),
                [4] = Helpers.StringToBit("11011001"),
                [5] = Helpers.StringToBit("10001101"),
                [6] = Helpers.StringToBit("00010001")
            };

            var key = new List<BitArray>(new BitArray[7])
            {
                [0] = Helpers.StringToBit("0001101101"),
                [1] = Helpers.StringToBit("0001101110"),
                [2] = Helpers.StringToBit("0001110000"),
                [3] = Helpers.StringToBit("0001110001"),
                [4] = Helpers.StringToBit("0001110110"),
                [5] = Helpers.StringToBit("0001111000"),
                [6] = Helpers.StringToBit("0001111001")
            };

            for (var i = 0; i < 6; i++)
            {
                Console.Write("Key: ");
                Helpers.PrintBinary(key[i]);

                var plainText = Helpers.StringToBit("00000000"); // 8 plain text
                var outPut = _sdes.Crypt(plainText, key[i], true);

                Console.Write("Cipher text: ");
                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Decryption test
        /// </summary>
        [Test]
        public void TestBench6()
        {
            var expectedOut = new List<BitArray>(new BitArray[8]);
            expectedOut[0] = Helpers.StringToBit("10101000");
            expectedOut[1] = Helpers.StringToBit("10111110");
            expectedOut[2] = Helpers.StringToBit("00010110");
            expectedOut[3] = Helpers.StringToBit("01001010");
            expectedOut[4] = Helpers.StringToBit("01001001");
            expectedOut[5] = Helpers.StringToBit("01001110");
            expectedOut[6] = Helpers.StringToBit("00010101");
            expectedOut[7] = Helpers.StringToBit("01101000");

            for (var i = 0; i < 8; i++)
            {
                var key = Helpers.StringToBit("0000000000"); // 10 bit key

                var plainText = new BitArray(8, false) {[i] = true};
                var outPut = _sdes.Crypt(plainText, key, false);

                Helpers.PrintBinary(outPut);
                Assert.AreEqual(outPut, expectedOut[i]);
            }
        }

        /// <summary>
        /// Tests the FindWeakKeys() method
        /// </summary>
        [Test]
        public void TestWeakKeys()
        {
            var weakKeys = _sdes.FindWeakKeys();
            foreach (var x in weakKeys)
            {
                Helpers.PrintBinary(x);
            }
            // Four weak keys:
            // 0000000000
            // 0111101000
            // 1000010111
            // 1111111111
        }
    }
}