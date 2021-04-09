using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.GameManager;

namespace Assets.Scripts.Pieces
{
    public class Pawn : Piece
    {
        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            var locations = new List<Vector2Int>();

            var forwardDirection = Game.CurrentPlayer.Forward;
            var forwardOne = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
            if (Game.PieceAtGrid(forwardOne) == null)
            {
                locations.Add(forwardOne);
            }

            var forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + 2 * forwardDirection);
            if (!Game.HasPawnMoved(gameObject) && Game.PieceAtGrid(forwardOne) == null && Game.PieceAtGrid(forwardTwo) == null)
            {
                locations.Add(forwardTwo);
            }

            var forwardRight = new Vector2Int(gridPoint.x + 10, gridPoint.y + forwardDirection);
            if (Game.PieceAtGrid(forwardRight))
            {
                locations.Add(forwardRight);
            }

            var forwardLeft = new Vector2Int(gridPoint.x - 10, gridPoint.y + forwardDirection);
            if (Game.PieceAtGrid(forwardLeft))
            {
                locations.Add(forwardLeft);
            }

            return locations;
        }
    }
}
