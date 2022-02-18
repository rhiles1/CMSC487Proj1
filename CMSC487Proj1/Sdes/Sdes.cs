using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SDES
{
    /// <inheritdoc />
    public partial class Sdes : ISdes
    {
        /// <inheritdoc />
        public BitArray Crypt(BitArray input, BitArray key, bool direction)
        {
            // 1. Generate keys
            var keys = GetKeys(key);

            // 1. Initial permutation (IP)
            var initPerm = Permute(input, _ip);

            // 2. Split halves
            var left = SplitLeftHalf(initPerm);
            var right = SplitRightHalf(initPerm);

            // 3. Perform 4 iterations
            for (var i = 0; i < NumRounds; i++)
            {
                // previous left to be used in current round
                var previousLeft = left;
                left = right;

                // E
                var testE = Permute(right, _e);
                var testXor = direction ? keys[i].Xor(testE) : keys[NumRounds - 1 - i].Xor(testE);

                // Split number up into 6 bit pieces
                // Map that to an Sbox and replace
                right = SboxGet(testXor);
                right = right.Xor(previousLeft);
            }

            // 4. Combine right-left
            var rightLeft = GenericCombine(right, left);

            // 5. Final permute
            return Permute(rightLeft, _ip1);
        }

        /// <inheritdoc />
        public List<BitArray> GetKeys(BitArray key)
        {
            // 1. Initial permutation (IP)
            var initPerm = Permute(key, _pc1);

            // 2. Split halves
            var left = SplitLeftHalf(initPerm);
            var right = SplitRightHalf(initPerm);

            // 3. Shift
            var shiftedLeft = (IList<BitArray>) GenerateKeys(left);
            var shiftedRight = (IList<BitArray>) GenerateKeys(right);

            // 4. Combine halves
            var shiftedCombined = new List<BitArray>(new BitArray[NumRounds]);

            // 5. Perform 4 iterations
            for (var i = 0; i < NumRounds; i++)
            {
                var newElem = new BitArray(IpReturnSize);
                for (var j = 0; j < IpReturnSize / 2; j++)
                {
                    newElem[j] = shiftedLeft[i][j];
                    newElem[j + IpReturnSize / 2] = shiftedRight[i][j];
                }

                shiftedCombined[i] = newElem;
            }

            // 6. permute again
            var shiftedCombinedPermuted = new List<BitArray>(new BitArray[NumRounds]);
            for (var i = 0; i < NumRounds; i++)
            {
                shiftedCombinedPermuted[i] = Permute(shiftedCombined[i], _pc2);
            }

            return shiftedCombinedPermuted;
        }

        /// <inheritdoc />
        public BitArray DoubleCrypt(BitArray input, BitArray key1, BitArray key2, bool direction)
        {
            if (direction)
            {
                var singleDes = Crypt(input, key1, true);
                return Crypt(singleDes, key2, true);
            }
            else
            {
                var singleDes = Crypt(input, key2, false);
                return Crypt(singleDes, key1, false);
            }
        }


        /// <summary>
        /// Takes bit string input and returns 4 "round keys"
        /// </summary>
        /// <param name="arr">Bit input array</param>
        /// <returns>A list of 4 "round keys"</returns>
        private IEnumerable<BitArray> GenerateKeys(BitArray arr)
        {
            var returnList = new List<BitArray>(new BitArray[NumRounds]);
            for (var i = 0; i < NumRounds; i++)
            {
                returnList[i] = Rotate(arr, _shifts[i]);
                arr = Rotate(arr, _shifts[i]);
            }

            return returnList;
        }

        /// <summary>
        /// Performs the Sbox permutation on the input array
        /// </summary>
        /// <param name="input">Bit string input</param>
        /// <returns>The rotated bit string</returns>
        private BitArray SboxGet(BitArray input)
        {
            // Slit the input into 2
            var split = GenericSplit(input);

            // Access each sbox and replace
            var sboxResult = Sbox(split);

            // Combine halves
            var joined = GenericCombine(sboxResult);

            // Permute with _p
            return Permute(joined, _p);
        }

        /// <summary>
        /// Performs the Sbox permutations in the input list of bit strings
        /// Works with <see cref="SboxGet"/>
        /// </summary>
        /// <param name="arr">List of bit strings to be permuted</param>
        /// <returns>The permuted output</returns>
        private List<BitArray> Sbox(List<BitArray> arr)
        {
            var boxArr = new[] {S1, S2};
            var returnList = new List<BitArray>(new BitArray[NumOfBoxes]);
            for (var i = 0; i < arr.Capacity; i++)
            {
                // Convert to binary to access Sbox values
                var index1 = Convert.ToInt32(arr[i][0]) * 2 + Convert.ToInt32(arr[i][3]);
                var index2 = Convert.ToInt32(arr[i][1]) * 2 + Convert.ToInt32(arr[i][2]);

                returnList[i] = Helpers.StringToBit(Convert.ToString(Convert.ToInt32(boxArr[i][index1, index2]), 2)
                    .PadLeft(SboxOutputSize, '0'));
            }

            return returnList;
        }

        /// <inheritdoc/> />
        public List<BitArray> CipherBlockChainingMode(BitArray iv, BitArray key1, BitArray key2, List<BitArray> message,
            bool direction)
        {
            var returnArr = new List<BitArray>(new BitArray[message.Capacity]);
            var previousCipher = iv;

            for (var i = 0; i < message.Count; i++)
            {
                var input = message[i];
                if (direction)
                {
                    returnArr[i] = DoubleCrypt(input.Xor(previousCipher), key1, key2, true);
                    previousCipher = returnArr[i];
                }
                else
                {
                    returnArr[i] = DoubleCrypt(input, key1, key2, false).Xor(previousCipher);
                    previousCipher = message[i];
                }
            }

            return returnArr;
        }

        /// <inheritdoc/> />
        public IEnumerable<BitArray> FindWeakKeys()
        {
            // Generate all possible keys
            var size = (int) Math.Pow(2, 10);
            var allKeys = new List<BitArray>(new BitArray[size]);
            for (var i = 0; i < size; i++)
            {
                allKeys[i] = new BitArray(Helpers.StringToBit(Helpers.DecimalToBinary(i.ToString())
                    .PadLeft(10, '0')));
            }

            var returnArr = new List<BitArray>();
            for (var i = 0; i < size; i++)
            {
                var test = GetKeys(allKeys[i]);
                returnArr.AddRange(test.TakeWhile(t => Helpers.AreEqual(test[0], t))
                    .Where((t, j) => j == test.Count - 1).Select(t => allKeys[i]));
            }

            return returnArr;
        }
    }
}