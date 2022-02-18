using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using SDES;

namespace Test
{
    public class TestMitm
    {
        private ISdes _test;
        private Mitm _mitm;

        [SetUp]
        public void Setup()
        {
            _test = new Sdes();
            _mitm = new Mitm();
        }

        /// <summary>
        /// Known plain texts
        /// </summary>
        private readonly List<BitArray> _plain = new()
        {
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x42", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x72", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x75", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x74", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x65", 16), 2).PadLeft(8, '0'))
        };

        /// <summary>
        /// Known cipher texts
        /// </summary>
        private readonly List<BitArray> _cipher = new()
        {
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x52", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0xf0", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0xbe", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x69", 16), 2).PadLeft(8, '0')),
            Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x8a", 16), 2).PadLeft(8, '0'))
        };

        /// <summary>
        /// Tests the Mitm Attack
        /// </summary>
        [Test]
        public void TestMitmAttack()
        {
            // times attack
            var clock = new Stopwatch();
            clock.Start();
            var keys = _mitm.MitmAttack(_plain, _cipher);
            clock.Stop();

            Console.WriteLine($"Elapsed time: {clock.ElapsedMilliseconds} ms");
            Helpers.PrintBinary(keys[0]); // DS-SDES key 1
            Helpers.PrintBinary(keys[1]); // DS-SDES key 2

            // asserts keys work
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(_test.DoubleCrypt(_plain[i], keys[0], keys[1], true), _cipher[i]);
                Console.WriteLine($"Plain {Helpers.BitToString(_plain[i])}");
                Console.WriteLine(
                    $"Cipher {Helpers.BitToString(_test.DoubleCrypt(_plain[i], keys[0], keys[1], true))}");
            }

            Console.WriteLine("");
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(_test.DoubleCrypt(_cipher[i], keys[0], keys[1], false), _plain[i]);
                Console.WriteLine(
                    $"Plain {Helpers.BitToString(_test.DoubleCrypt(_cipher[i], keys[0], keys[1], false))}");
                Console.WriteLine($"Cipher {Helpers.BitToString(_cipher[i])}");
            }
        }

        /// <summary>
        /// Tests the brute force Attack
        /// </summary>
        [Test]
        public void TestBruteForceAttack()
        {
            // times attack
            var clock = new Stopwatch();
            clock.Start();
            var keys = _mitm.BruteForceAttack(_plain, _cipher);
            clock.Stop();

            Console.WriteLine($"Elapsed time: {clock.ElapsedMilliseconds} ms");
            Helpers.PrintBinary(keys[0]); // DS-SDES key 1
            Helpers.PrintBinary(keys[1]); // DS-SDES key 2

            // Asserts keys work
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(_test.DoubleCrypt(_plain[i], keys[0], keys[1], true), _cipher[i]);
                Console.WriteLine($"Plain {Helpers.BitToString(_plain[i])}");
                Console.WriteLine(
                    $"Cipher {Helpers.BitToString(_test.DoubleCrypt(_plain[i], keys[0], keys[1], true))}");
            }

            Console.WriteLine("");
            for (var i = 0; i < 5; i++)
            {
                Assert.AreEqual(_test.DoubleCrypt(_cipher[i], keys[0], keys[1], false), _plain[i]);
                Console.WriteLine(
                    $"Plain {Helpers.BitToString(_test.DoubleCrypt(_cipher[i], keys[0], keys[1], false))}");
                Console.WriteLine($"Cipher {Helpers.BitToString(_cipher[i])}");
            }
        }
    }
}