using System.Collections;
using System.Collections.Generic;

namespace SDES
{
    /// <summary>
    /// Service class that contains SDES methods.
    /// </summary>
    public interface ISdes
    {
        /// <summary>
        /// Main Encrpyion/Decryption method
        /// </summary>
        /// <param name="input">An 8-bit input message</param>
        /// <param name="key">A 10-bit input key</param>
        /// <param name="direction">A boolean to indicate decryption (false) or encryption (true)</param>
        /// <returns>The 8-bit result of performing SDES</returns>
        BitArray Crypt(BitArray input, BitArray key, bool direction);

        /// <summary>
        /// Gets the 4 10-bit key schedule from the single 10-bit input key
        /// </summary>
        /// <param name="key">A 10-bit input key</param>
        /// <returns>A key schedule composed of 4 10-bit keys</returns>
        List<BitArray> GetKeys(BitArray key);

        /// <summary>
        /// Performs SDES twice on the input
        /// </summary>
        /// <param name="input">An 8-bit input message</param>
        /// <param name="key1">The first 10-bit key</param>
        /// <param name="key2">The second 10-bit key</param>
        /// <param name="direction">A boolean to indicate decryption (false) or encryption (true)</param>
        /// <returns>The 8-bit result of performing SDES twice</returns>
        BitArray DoubleCrypt(BitArray input, BitArray key1, BitArray key2, bool direction);

        /// <summary>
        /// Implements Cipher Block Chaining Mode
        /// </summary>
        /// <param name="iv">Initial permutation vector</param>
        /// <param name="key1">First 10-bit key used in DS-DES</param>
        /// <param name="key2">Second 10-bit key used in DS-DES</param>
        /// <param name="message">List of 8-bit input messages</param>
        /// <param name="direction">A boolean to indicate decryption (false) or encryption (true)</param>
        /// <returns>The decrypted or encrypted output</returns>
        List<BitArray> CipherBlockChainingMode(BitArray iv, BitArray key1, BitArray key2, List<BitArray> message,
            bool direction);

        /// <summary>
        /// Systemically calculates all the weak keys through "brute force"
        /// </summary>
        /// <returns>A list of weak keys</returns>
        IEnumerable<BitArray> FindWeakKeys();
    }
}