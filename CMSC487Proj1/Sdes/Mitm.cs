using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SDES
{
    /// <summary>
    /// Service class that implements "meet-in-the-middle" attack against DS-DES
    /// </summary>
    public class Mitm
    {
        private readonly ISdes _sdes;

        public Mitm()
        {
            _sdes = new Sdes();
        }

        /// <summary>
        /// Method that implements "meet-in-the-middle" attack.
        /// Exploits the "middle" ciphertext "x"
        /// </summary>
        /// <param name="plain">known plain text list</param>
        /// <param name="cipher">known cipher text list</param>
        /// <returns>List of the 2 10-bit SDES keys or null</returns>
        public List<BitArray> MitmAttack(List<BitArray> plain, List<BitArray> cipher)
        {
            // 1. Generate all possible keys
            var size = (int) Math.Pow(2, 10);
            var allKeys = new List<BitArray>(new BitArray[size]);
            for (var i = 0; i < size; i++)
            {
                allKeys[i] = new BitArray(Helpers.StringToBit(Helpers.DecimalToBinary(i.ToString())
                    .PadLeft(10, '0')));
            }

            // 2. Take known plain text and store all possible K1 X combinations
            var allX1 = new List<BitArray>(new BitArray[size]);
            for (var i = 0; i < size; i++)
            {
                allX1[i] = _sdes.Crypt(plain[0], allKeys[i], true);
            }

            // 3. Take known cipher text and store all possible X K2 combinations
            var allX2 = new List<BitArray>(new BitArray[size]);
            for (var i = 0; i < size; i++)
            {
                allX2[i] = _sdes.Crypt(cipher[0], allKeys[i], false);
            }

            // 4. Check candidates
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (!Helpers.AreEqual(allX1[i], allX2[j])) continue;
                    if (plain.TakeWhile((t, k) =>
                            Helpers.AreEqual(_sdes.Crypt(t, allKeys[i], true),
                                _sdes.Crypt(cipher[k], allKeys[j], false)))
                        .Where((t, k) => k == plain.Count - 1).Any())
                    {
                        return new List<BitArray>()
                        {
                            allKeys[i],
                            allKeys[j]
                        };
                    }
                }
            }

            // Key not found
            return null;
        }

        /// <summary>
        /// Method that implements a brute force attack against DS-DES
        /// </summary>
        /// <param name="plain">known plain text list</param>
        /// <param name="cipher">known cipher text list</param>
        /// <returns>List of the 2 10-bit SDES keys or null</returns>
        public List<BitArray> BruteForceAttack(List<BitArray> plain, List<BitArray> cipher)
        {
            // 1. Generate all possible keys
            var size = (int) Math.Pow(2, 10);
            var allKeys = new List<BitArray>(new BitArray[size]);
            for (var i = 0; i < size; i++)
            {
                allKeys[i] = new BitArray(Helpers.StringToBit(Helpers.DecimalToBinary(i.ToString()).PadLeft(10, '0')));
            }

            // 2. Check keys
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (!Helpers.AreEqual(_sdes.DoubleCrypt(cipher[0], allKeys[i], allKeys[j], false), plain[0]))
                        continue;
                    if (plain.TakeWhile((t, k) =>
                            Helpers.AreEqual(_sdes.DoubleCrypt(cipher[k], allKeys[i], allKeys[j], false), t))
                        .Where((t, k) => k == plain.Count - 1).Any())
                    {
                        return new List<BitArray>()
                        {
                            allKeys[i],
                            allKeys[j]
                        };
                    }
                }
            }

            // Key not found
            return null;
        }
    }
}