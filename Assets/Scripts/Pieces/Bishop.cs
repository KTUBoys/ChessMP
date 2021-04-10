using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Bishop : Piece
    {
        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            var locations = new List<Vector2Int>();

            foreach (var dir in BishopDirections)
            {
                for (var i = 1; i < 8; i++)
                {
                    var nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                    locations.Add(nextGridPoint);
                    if (GameManager.Game.PieceAtGrid(nextGridPoint))
                    {
                        break;
                    }
                }
            }

            return locations;
        }
    }
}
