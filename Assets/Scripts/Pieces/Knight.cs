using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Knight : Piece
    {
        public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            var locations = new List<Vector2Int>();

            locations.Add(new Vector2Int(gridPoint.x - 10, gridPoint.y + 20));
            locations.Add(new Vector2Int(gridPoint.x + 10, gridPoint.y + 20));

            locations.Add(new Vector2Int(gridPoint.x + 20, gridPoint.y + 10));
            locations.Add(new Vector2Int(gridPoint.x - 20, gridPoint.y + 10));

            locations.Add(new Vector2Int(gridPoint.x + 20, gridPoint.y - 10));
            locations.Add(new Vector2Int(gridPoint.x - 20, gridPoint.y - 10));

            locations.Add(new Vector2Int(gridPoint.x + 10, gridPoint.y - 20));
            locations.Add(new Vector2Int(gridPoint.x - 10, gridPoint.y - 20));

            return locations;
        }
    }
}
