using UnityEngine;

namespace Assets.Scenes.GameView.Scripts
{
    public class PieceSpawner : MonoBehaviour
    {
        public GameObject PawnPiece;

        public GameObject RookPiece;

        public GameObject QueenPiece;

        public GameObject KingPiece;

        public GameObject BishopPiece;

        public GameObject KnightPiece;

        public float YAxis = -0.85f;
        // Start is called before the first frame update
        void Start()
        {
            Quaternion[] pieceAngles = {Quaternion.Euler(-90, 180, 0),  // [0] - WHITE,
                Quaternion.Euler(-90, -180, 0)};                        // [1] - BLACK
            int[] xPoints = 
            {
                -75,    // ROOKS AND PAWNS
                -65,    // KNIGHTS
                -55,    // BISHOPS
                -45,    // QUEEN
                -35     // KING

            };
            int[] zPoints =
            {
                -59,    // PAWNS ROW WHITE
                -69,    // BACK ROW WHITE
                -9,     // PAWNS ROW BLACK
                1       // BACK ROW BLACK
            };

            SpawnFigures(xPoints, zPoints, pieceAngles);


        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void SpawnFigures(int[] xPoints, int[] zPoints, Quaternion[] angles)
        {
            // Spawn White figures
            var white = angles[0];

            SpawnPaws(xPoints[0], zPoints[0], white);
            SpawnRooks(xPoints[0], zPoints[1], white);
            SpawnKnights(xPoints[1], zPoints[1], white);
            SpawnBishops(xPoints[2], zPoints[1], white);
            SpawnQueen(xPoints[3], zPoints[1], white);
            SpawnKing(xPoints[4], zPoints[1], white);

            // Spawn Black figures
            var black = angles[1];

            SpawnPaws(xPoints[0], zPoints[2], black);
            SpawnRooks(xPoints[0], zPoints[3], black);
            SpawnKnights(xPoints[1], zPoints[3], black);
            SpawnBishops(xPoints[2], zPoints[3], black);
            SpawnQueen(xPoints[3], zPoints[3], black);
            SpawnKing(xPoints[4], zPoints[3], black);
        }

        private void SpawnPaws(float x, float z, Quaternion quaternion)
        {
            for (var i = 0; i < 8; i++)
            {
                Instantiate(PawnPiece, new Vector3(x, YAxis, z), quaternion);
                x += 10;    // Add 10 to x-axis
            }
        }

        private void SpawnRooks(float x, float z, Quaternion quaternion)
        {
            for (var i = 0; i < 2; i++)
            {
                Instantiate(RookPiece, new Vector3(x, YAxis, z), quaternion);
                x += 70;
            }
        }

        private void SpawnKnights(float x, float z, Quaternion quaternion)
        {
            for (var i = 0; i < 2; i++)
            {
                Instantiate(KnightPiece, new Vector3(x, YAxis, z), quaternion);
                x += 50;
            }
        }

        private void SpawnBishops(float x, float z, Quaternion quaternion)
        {
            for (var i = 0; i < 2; i++)
            {
                Instantiate(BishopPiece, new Vector3(x, YAxis, z), quaternion);
                x += 20;
            }
        }

        private void SpawnKing(float x, float z, Quaternion quaternion)
        {
            Instantiate(KingPiece,new Vector3(x, YAxis, z), quaternion);
        }

        private void SpawnQueen(float x, float z, Quaternion quaternion)
        {
            Instantiate(QueenPiece, new Vector3(x, YAxis, z), quaternion);
        }
    }
}
