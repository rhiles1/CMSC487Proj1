using System.Collections;
using System.Collections.Generic;

namespace SDES
{
    public partial class Sdes
    {
        /// <summary>
        /// Uses a permutation array to permute the input array 
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="perm">Permutation array</param>
        /// <returns>The permuted array</returns>
        private BitArray Permute(BitArray arr, IList<int> perm)
        {
            var returnArr = new BitArray(perm.Count);
            for (var i = 0; i < perm.Count; i++)
            {
                returnArr[i] = arr[perm[i]];
            }

            return returnArr;
        }

        /// <summary>
        /// Splits the input array and returns the left half
        /// </summary>
        /// <param name="arr">The input array</param>
        /// <returns>The left half of the input array</returns>
        private BitArray SplitLeftHalf(BitArray arr)
        {
            var returnArr = new BitArray(arr.Length / 2);
            for (var i = 0; i < arr.Length / 2; i++)
            {
                returnArr[i] = arr[i];
            }

            return returnArr;
        }

        /// <summary>
        /// Splits the input array and returns the right half
        /// </summary>
        /// <param name="arr">The input array</param>
        /// <returns>The right half of the input array</returns>
        private BitArray SplitRightHalf(BitArray arr)
        {
            var returnArr = new BitArray(arr.Length / 2);
            for (var i = 0; i < arr.Length / 2; i++)
            {
                returnArr[i] = arr[arr.Length / 2 + i];
            }

            return returnArr;
        }

        /// <summary>
        /// Takes bit string input and rotates
        /// </summary>
        /// <param name="arr">Bit string input</param>
        /// <param name="size">The size of the rotation</param>
        /// <returns>The rotated bit string</returns>
        private BitArray Rotate(BitArray arr, int size)
        {
            var returnArr = new BitArray(IpReturnSize / 2);
            for (var i = 0; i < IpReturnSize / 2; i++)
            {
                returnArr[i] = arr[(i + size) % (IpReturnSize / 2)];
            }

            return returnArr;
        }

        /// <summary>
        /// Splits a BitArray into halves and returns a list containing the halves
        /// Works with <see cref="GenericCombine"/>
        /// </summary>
        /// <param name="arr">Bit string input to be split</param>
        /// <returns>A list of the split input</returns>
        private List<BitArray> GenericSplit(BitArray arr)
        {
            var returnList = new List<BitArray>(new BitArray[NumOfBoxes]);
            for (var i = 0; i < NumOfBoxes; i++)
            {
                returnList[i] = new BitArray(SboxInputSize, true);
            }

            for (var i = 0; i < NumOfBoxes; i++)
            {
                for (var j = 0; j < SboxInputSize; j++)
                {
                    returnList[i][j] = arr[j + i * SboxInputSize];
                }
            }

            return returnList;
        }

        /// <summary>
        /// Combined two bit string halves
        /// Works with <see cref="GenericSplit"/>
        /// </summary>
        /// <param name="arr">List of bit strings to be combined</param>
        /// <returns>The combined input</returns>
        private BitArray GenericCombine(List<BitArray> arr)
        {
            var returnList = new BitArray(arr.Capacity * arr[0].Count);
            for (var i = 0; i < arr.Capacity; i++)
            {
                for (var j = 0; j < arr[0].Count; j++)
                {
                    returnList[i * arr[0].Count + j] = arr[i][j];
                }
            }

            return returnList;
        }

        /// <summary>
        /// Overload of <cref>GenericCombine(BitArray)</cref>
        /// </summary>
        /// <param name="left">Left bit string to be combined</param>
        /// <param name="right">Right bit string to be combined</param>
        /// <returns>The combined input</returns>
        private BitArray GenericCombine(BitArray left, BitArray right)
        {
            var returnArr = new BitArray(left.Count * 2);
            for (var i = 0; i < left.Count; i++)
            {
                returnArr[i] = left[i];
            }

            for (var i = 0; i < right.Count; i++)
            {
                returnArr[i + left.Count] = right[i];
            }

            return returnArr;
        }
    }
}