using System.Drawing;

namespace Assets.Scripts
{
    public static class ChessBoard
    {
        public static Point[] BoardCoords { get; } =
        {
            new Point(-75, 1), new Point(-65, 1), new Point(-55, 1), new Point(-45, 1), new Point(-35, 1), new Point(-25, 1), new Point(-15, 1), new Point(-5, 1),
            new Point(-75, -9), new Point(-65, -9), new Point(-55, -9), new Point(-45, -9), new Point(-35, -9), new Point(-25, -9), new Point(-15, -9), new Point(-5, -9),
            new Point(-75, -19), new Point(-65, -19), new Point(-55, -19), new Point(-45, -19), new Point(-35, -19), new Point(-25, -19), new Point(-15, -19), new Point(-5, -19),
            new Point(-75, -29), new Point(-65, -29), new Point(-55, -29), new Point(-45, -29), new Point(-35, -29), new Point(-25, -29), new Point(-15, -29), new Point(-5, -29),
            new Point(-75, -39), new Point(-65, -39), new Point(-55, -39), new Point(-45, -39), new Point(-35, -39), new Point(-25, -39), new Point(-15, -39), new Point(-5, -39),
            new Point(-75, -49), new Point(-65, -49), new Point(-55, -49), new Point(-45, -49), new Point(-35, -49), new Point(-25, -49), new Point(-15, -49), new Point(-5, -49),
            new Point(-75, -59), new Point(-65, -59), new Point(-55, -59), new Point(-45, -59), new Point(-35, -59), new Point(-25, -59), new Point(-15, -59), new Point(-5, -59),
            new Point(-75, -69), new Point(-65, -69), new Point(-55, -69), new Point(-45, -69), new Point(-35, -69), new Point(-25, -69), new Point(-15, -69), new Point(-5, -69),
        };

        public static bool[] IsFull = new bool[64];
        public static string[] WhoInCell = new string[64];

        public static void Initialize()
        {
            for (var i = 0; i < IsFull.Length; i++)
            {
                IsFull[i] = false;
            }
        }
    }
}