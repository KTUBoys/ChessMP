using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts;

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

        public float YAxis = -0.9f;

        // Start is called before the first frame update
        void Start()
        {
            ChessBoard.Initialize();

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

        private void SpawnFigures([NotNull] IReadOnlyList<int> xPoints, [NotNull] IReadOnlyList<int> zPoints, [NotNull] IReadOnlyList<Quaternion> angles)
        {
            // Spawn White figures
            var white = angles[0];

            SpawnPaws(xPoints[0], zPoints[0], white, 1);
            SpawnRooks(xPoints[0], zPoints[1], white, 1);
            SpawnKnights(xPoints[1], zPoints[1], white, 1);
            SpawnBishops(xPoints[2], zPoints[1], white, 1);
            SpawnQueen(xPoints[3], zPoints[1], white, 1);
            SpawnKing(xPoints[4], zPoints[1], white, 1);

            // Spawn Black figures
            var black = angles[1];

            SpawnPaws(xPoints[0], zPoints[2], black, 0);
            SpawnRooks(xPoints[0], zPoints[3], black, 0);
            SpawnKnights(xPoints[1], zPoints[3], black, 0);
            SpawnBishops(xPoints[2], zPoints[3], black, 0);
            SpawnQueen(xPoints[3], zPoints[3], black, 0);
            SpawnKing(xPoints[4], zPoints[3], black, 0);
        }

        private void SpawnPaws(float x, float z, Quaternion quaternion, int color)
        {
            var isFull = ChessBoard.IsFull;
            var whoInCell = ChessBoard.WhoInCell;
            PawnPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            var index = 8;
            var c = "Black";
            if (color == 1)
            {
                c = "White";
                index = 48;
            }
            for (var i = 0; i < 8; i++)
            {
                var pawnName = $"{c} Pawn {i}";
                var pawn = Instantiate(PawnPiece, new Vector3(x, YAxis, z), quaternion);
                pawn.AddComponent<PawnMovement>();
                pawn.AddComponent<BoxCollider>();
                whoInCell[index] = pawnName;
                pawn.name = pawnName;
                isFull[index] = true;
                index++;
                x += 10;
            }
        }

        private void SpawnRooks(float x, float z, Quaternion quaternion, int color)
        {
            RookPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                Instantiate(RookPiece, new Vector3(x, YAxis, z), quaternion);
                x += 70;
            }
        }

        private void SpawnKnights(float x, float z, Quaternion quaternion, int color)
        {
            KnightPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                Instantiate(KnightPiece, new Vector3(x, YAxis, z), quaternion);
                x += 50;
            }
        }

        private void SpawnBishops(float x, float z, Quaternion quaternion, int color)
        {
            BishopPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                Instantiate(BishopPiece, new Vector3(x, YAxis, z), quaternion);
                x += 30;
            }
        }

        private void SpawnKing(float x, float z, Quaternion quaternion, int color)
        {
            KingPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            Instantiate(KingPiece,new Vector3(x, YAxis, z), quaternion);
        }

        private void SpawnQueen(float x, float z, Quaternion quaternion, int color)
        {
            QueenPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            Instantiate(QueenPiece, new Vector3(x, YAxis, z), quaternion);
        }
    }
}
