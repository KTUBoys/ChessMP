using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public enum PieceType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    };

    public abstract class Piece : MonoBehaviour
    {
        public PieceType Type;

        protected Vector2Int[] RookDirections = {new Vector2Int(0,10), new Vector2Int(10, 0),
            new Vector2Int(0, -10), new Vector2Int(-10, 0)};
        protected Vector2Int[] BishopDirections = {new Vector2Int(10,10), new Vector2Int(10, -10),
            new Vector2Int(-10, -10), new Vector2Int(-10, 10)};

        public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
    }
}
