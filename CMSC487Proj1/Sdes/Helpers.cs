using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDES
{
    /// <summary>
    /// Helper class
    /// Contains printers and convertors
    /// </summary>
    public static class Helpers
    {
        public static void PrintBinary(BitArray arr)
        {
            for (var i = 0; i < arr.Count; i++)
            {
                Console.Write(arr[i] ? "1" : "0");
            }

            Console.WriteLine("");
        }

        public static BitArray StringToBit(string input) => new BitArray(input.Select(c => c == '1').ToArray());

        public static string BitToString(BitArray arr)
        {
            var returnStr = new StringBuilder();
            for (var i = 0; i < arr.Count; i++)
            {
                returnStr.Append(arr[i] ? "1" : "0");
            }

            return returnStr.ToString();
        }

        public static string DecimalToBinary(string input) => Convert.ToString(Convert.ToInt32(input, 10), 2);

        public static bool AreEqual(BitArray one, BitArray two)
        {
            if (one.Count != two.Count)
            {
                return false;
            }

            for (var i = 0; i < one.Count; i++)
            {
                if (one[i] != two[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static List<BitArray> HexStringToBits(string hex)
        {
            const int size = 2;
            var split = hex.Select((elem, i) => new {elem, i})
                .GroupBy(x => x.i / size)
                .Select(group => group.Select(x => x.elem))
                .Select(x => new string(x.ToArray()));

            var asList = split.ToList();
            var twoByteList = new List<BitArray>(new BitArray[hex.Length / size]);
            for (var i = 0; i < asList.Count; i++)
            {
                twoByteList[i] = StringToBit(Convert.ToString(Convert.ToInt64(asList[i], 16), 2)
                    .PadLeft(8, '0'));
            }

            return twoByteList;
        }

        public static byte[] GetBytesFromBinaryString(string binary)
        {
            var list = new List<byte>();
            for (var i = 0; i < binary.Length; i += 8)
            {
                var t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }
    }
}