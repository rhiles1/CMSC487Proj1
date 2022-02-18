namespace SDES
{
    /// <summary>
    /// File that contains constants for Sdes class
    /// </summary>
    public partial class Sdes
    {
        // Readonly arrays
        private readonly int[] _pc1 =
        {
            2, 4, 1, 6, 3,
            9, 0, 8, 7, 5
        };

        private readonly int[] _pc2 =
        {
            5, 2, 6, 3, 7, 4, 9, 8
        };

        private readonly int[] _shifts = {1, 2, 2, 2};

        private readonly int[] _ip =
        {
            1, 5, 2, 0, 3, 7, 4, 6
        };

        private readonly int[] _ip1 =
        {
            3, 0, 2, 4, 6, 1, 7, 5
        };

        private static readonly int[,] S1 =
        {
            {1, 0, 3, 2},
            {3, 2, 1, 0},
            {0, 2, 1, 3},
            {3, 1, 3, 2}
        };

        private static readonly int[,] S2 =
        {
            {0, 1, 2, 3},
            {2, 0, 1, 3},
            {3, 0, 1, 0},
            {2, 1, 0, 3}
        };

        private readonly int[] _e =
        {
            3, 0, 1, 2, 1, 2, 3, 0
        };

        private readonly int[] _p =
        {
            1, 3, 2, 0
        };
        
        // Constant Variables
        private const int IpReturnSize = 10;
        private const int NumRounds = 4;
        private const int SboxInputSize = 4;
        private const int SboxOutputSize = 2;
        private const int NumOfBoxes = 2;
    }
}