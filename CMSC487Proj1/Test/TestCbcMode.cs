using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SDES;

namespace Test
{
    /// <summary>
    /// Tests Cipher Block Chaining mode of the DS-SDES implementation
    /// </summary>
    public class TestCbcMode
    {
        private ISdes _sdes;

        [SetUp]
        public void Setup()
        {
            _sdes = new Sdes();
        }

        /// <summary>
        /// Ciphertext to be decoded
        /// </summary>
        private const string Cipher = "586519b031aaee9a235247601fb37baefbcd54d8c3763f8523d2a1315ed8bdcc";

        private const string CipherBinary =
            "0101100001100101000110011011000000110001101010101110111010011010001000110101001001000111011000000001111110110011011110111010111011111011110011010101010011011000110000110111011000111111100001010010001111010010101000010011000101011110110110001011110111001100";

        /// <summary>
        /// Expected plaintext that was decoded from CBC mode decryption
        /// </summary>
        private const string Plain = "Congratulations on your success!";

        private const string PlainHex = "436F6E67726174756C6174696F6E73206F6E20796F7572207375636365737321";

        /// <summary>
        /// Tests CBC Mode Decryption with ciphertext and key
        /// </summary>
        [Test]
        public void TestCbcModeDecrypt()
        {
            var message = Helpers.HexStringToBits(Cipher);
            var iv = Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x9c", 16), 2).PadLeft(8, '0'));

            // keys from Mitm attack
            var key1 = Helpers.StringToBit("1100111111");
            var key2 = Helpers.StringToBit("0101010011");
            var outPut = _sdes.CipherBlockChainingMode(iv, key1, key2, message, false);

            var outStr = new StringBuilder();
            foreach (var str in outPut.Select(Helpers.BitToString))
            {
                outStr.Append(Encoding.ASCII.GetString(Helpers.GetBytesFromBinaryString(str)));
                Console.Write(Encoding.ASCII.GetString(Helpers.GetBytesFromBinaryString(str)));
            }

            Assert.AreEqual(Plain, outStr.ToString());
        }

        /// <summary>
        /// Tests CBC Mode Encryption with plaintext (found in previous test) and key
        /// </summary>
        [Test]
        public void TestCbcModeEncrypt()
        {
            var message = Helpers.HexStringToBits(PlainHex);
            var iv = Helpers.StringToBit(Convert.ToString(Convert.ToInt64("0x9c", 16), 2).PadLeft(8, '0'));

            // keys from Mitm attack
            var key1 = Helpers.StringToBit("1100111111");
            var key2 = Helpers.StringToBit("0101010011");
            var outPut = _sdes.CipherBlockChainingMode(iv, key1, key2, message, true);

            var outStr = new StringBuilder();
            foreach (var str in outPut.Select(Helpers.BitToString))
            {
                outStr.Append(str);
            }

            Console.WriteLine(outStr);
            Assert.AreEqual(CipherBinary, outStr.ToString());
        }
    }
}