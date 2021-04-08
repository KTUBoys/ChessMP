using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class ChessBoard : MonoBehaviour
    {
        internal static Material DefaultMaterial { get; set; }
        public Material SelectedMaterial;

        public static Vector2Int[,] GridPoints { get; } = new Vector2Int[8,8]
        {
            { new Vector2Int(-75, 1), new Vector2Int(-65, 1), new Vector2Int(-55, 1), new Vector2Int(-45, 1), new Vector2Int(-35, 1), new Vector2Int(-25, 1), new Vector2Int(-15, 1), new Vector2Int(-5, 1) },
            { new Vector2Int(-75, -9), new Vector2Int(-65, -9), new Vector2Int(-55, -9), new Vector2Int(-45, -9), new Vector2Int(-35, -9), new Vector2Int(-25, -9), new Vector2Int(-15, -9), new Vector2Int(-5, -9) },
            { new Vector2Int(-75, -19), new Vector2Int(-65, -19), new Vector2Int(-55, -19), new Vector2Int(-45, -19), new Vector2Int(-35, -19), new Vector2Int(-25, -19), new Vector2Int(-15, -19), new Vector2Int(-5, -19) },
            { new Vector2Int(-75, -29), new Vector2Int(-65, -29), new Vector2Int(-55, -29), new Vector2Int(-45, -29), new Vector2Int(-35, -29), new Vector2Int(-25, -29), new Vector2Int(-15, -29), new Vector2Int(-5, -29) },
            { new Vector2Int(-75, -39), new Vector2Int(-65, -39), new Vector2Int(-55, -39), new Vector2Int(-45, -39), new Vector2Int(-35, -39), new Vector2Int(-25, -39), new Vector2Int(-15, -39), new Vector2Int(-5, -39) },
            { new Vector2Int(-75, -49), new Vector2Int(-65, -49), new Vector2Int(-55, -49), new Vector2Int(-45, -49), new Vector2Int(-35, -49), new Vector2Int(-25, -49), new Vector2Int(-15, -49), new Vector2Int(-5, -49) },
            { new Vector2Int(-75, -59), new Vector2Int(-65, -59), new Vector2Int(-55, -59), new Vector2Int(-45, -59), new Vector2Int(-35, -59), new Vector2Int(-25, -59), new Vector2Int(-15, -59), new Vector2Int(-5, -59) },
            { new Vector2Int(-75, -69), new Vector2Int(-65, -69), new Vector2Int(-55, -69), new Vector2Int(-45, -69), new Vector2Int(-35, -69), new Vector2Int(-25, -69), new Vector2Int(-15, -69), new Vector2Int(-5, -69) }
        };

        public static Vector3 PointFromGrid(Vector2Int gridPoint)
        {
            float x = gridPoint.x;
            float z = gridPoint.y;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var point = GridPoints[i,j];
                    if (Math.Abs(gridPoint.x - point.x) < 4 && Math.Abs(gridPoint.y - point.y) < 4)
                    {
                        x = point.x;
                        z = point.y;
                    }
                }
            }
            return new Vector3(x, -0.8f, z);
        }

        public static Vector2Int GridPoint(int col, int row)
        {
            return new Vector2Int(col ,row);
        }

        public static Vector2Int GridFromPoint(Vector3 point)
        {
            var col = (int) Math.Floor(point.x);
            var row = (int) Math.Floor(point.z);
            return new Vector2Int(col, row);
        }

        public static Vector2Int PlaceFromGrid(Vector2Int gridPoint)
        {
            var x = gridPoint.x;
            var y = gridPoint.y;

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var point = GridPoints[i, j];
                    if (Math.Abs(gridPoint.x - point.x) < 4 && Math.Abs(gridPoint.y - point.y) < 4)
                    {
                        x = j;
                        y = i;
                    }
                }
            }

            return new Vector2Int(x, y);
        }

        public void MovePiece(GameObject piece, Vector2Int gridPoint) =>
            piece.transform.position = PointFromGrid(gridPoint);

        public void SelectPiece(GameObject piece) => piece.GetComponentInChildren<MeshRenderer>().material = SelectedMaterial;

        public void DeselectPiece(GameObject piece) =>
            piece.GetComponentInChildren<MeshRenderer>().material = DefaultMaterial;
    }
}