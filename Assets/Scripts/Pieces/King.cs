using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class King : Piece
    {
        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            var locations = new List<Vector2Int>();
            var directions = new List<Vector2Int>(BishopDirections);
            directions.AddRange(RookDirections);

            foreach (var dir in directions)
            {
                var nextGridPoint = new Vector2Int(gridPoint.x + dir.x, gridPoint.y + dir.y);
                locations.Add(nextGridPoint);
            }

            return locations;
        }
    }
}
