using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    // "Father" class for movement, has MonoBehaviour implementation
    public class GeneralMovement : MonoBehaviour
    {
        public bool OutOfBorders(Vector3 curPos)
        {
            // Hard-coded values for borders
            const float rightBorder = 1f;
            const float leftBorder = -80f;
            const float topBorder = 6.65f;
            const float bottomBorder = -75f;

            if (curPos.x >= rightBorder || curPos.x <= leftBorder) return true;

            return curPos.z >= topBorder || curPos.z <= bottomBorder;
        }

       

        public void MouseDown(out Quaternion rot, out Vector3 defaultPos, out float mZCoord, out Vector3 mOffset)
        {
            var pos = gameObject.transform.position;        // Get current position
            rot = gameObject.transform.rotation;        // Get current rotation
            defaultPos = pos;                           // Put current position to default, to get default position later for any illegal moves
            Cursor.visible = false;

            // Get current mouse position, make offset.
            gameObject.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y + 1f, pos.z), rot);
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos(mZCoord);
        }

        public void MouseDrag(Vector3 mOffset, out Vector3 curPos, float mZCoord)
        {
            curPos = transform.position = new Vector3(GetMouseWorldPos(mZCoord).x + mOffset.x, transform.position.y,
                GetMouseWorldPos(mZCoord).z + mOffset.z);
        }

        /// <summary>
        /// Pretty complicated method
        /// </summary>
        /// <param name="curPos"> Current position </param>
        /// <param name="defaultPos"> Default position </param>
        /// <param name="rot"> Default rotation </param>
        /// TODO rewrite for simplicity(?)
        public void MoveToNearest(Vector3 curPos, Vector3 defaultPos, Quaternion rot)
        {
            // Take chessboard current stats
            var cells = ChessBoard.BoardCoords;
            var isFull = ChessBoard.IsFull;
            var whoInCell = ChessBoard.WhoInCell;

            // Get the current coords (where it was taken from) and put it into Point for comparison in for NN
            var origin = new Point((int)curPos.x, (int)curPos.z);

            var neighbors = GetNeighbor(cells, origin);

            // Get indexes for default and nearest cell found to check if it's empty
            var defaultCellIndex = Array.FindIndex(cells, point => point.X == (int)defaultPos.x && point.Y == (int)defaultPos.z);
            var cellIndex = Array.FindIndex(cells, point => point.X == neighbors.First().X && point.Y == neighbors.First().Y);

            // Variable for easier access
            var whoInDefault = whoInCell[defaultCellIndex];
            var position = new Vector3(neighbors.First().X, -0.9f, neighbors.First().Y);

            // Check if destination point is empty
            if (isFull[cellIndex])
            {
                // Not empty, check if we are trying to make a move
                var whoInDestination = whoInCell[cellIndex];
                var whatColorO = whoInDefault.Split(' ')[0];
                var whatColorD = whoInDestination.Split(' ')[0];
                if (whatColorO == whatColorD)
                {
                    // Tried to take out our own piece. Move the piece back to its place
                    SetPosition(defaultPos, rot);
                }
                else
                {
                    // Good, the piece was actually our enemy. Let's take it down
                    SetPosition(position, rot);
                    isFull[cellIndex] = true;
                    whoInCell[cellIndex] = whoInDefault;
                    whoInCell[defaultCellIndex] = null;
                    isFull[defaultCellIndex] = false;
                    Destroy(GameObject.Find(whoInDestination));
                }
            }
            else
            {
                // Empty destination, move successful
                SetPosition(position, rot);
                isFull[cellIndex] = true;
                whoInCell[cellIndex] = whoInDefault;
                whoInCell[defaultCellIndex] = null;
                isFull[defaultCellIndex] = false;
            }
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            gameObject.transform.SetPositionAndRotation(position, rotation);
        }

        private static float CalculateTwoDistances(Point originPoint, Point destinationPoint)
        {
            return (float)(Math.Pow(originPoint.X - destinationPoint.X, 2)
                            + Math.Pow(originPoint.Y - destinationPoint.Y, 2));
        }

        private static Vector3 GetMouseWorldPos(float mZCoord)
        {
            var mousePoint = Input.mousePosition;

            mousePoint.z = mZCoord;     // Multiplying by number due to mouse not actually following the piece correctly

            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        public Point[] GetNeighbor(IEnumerable<Point> cells, Point origin)
        {
            // A neat LinQ expression for finding nearest neighbor
            return cells.Select(p => new { Point = p, Distance = CalculateTwoDistances(origin, p) })
                .Where(pointAndDistance => pointAndDistance.Distance <= Math.Pow(7, 2))
                .OrderBy(pointAndDistance => pointAndDistance.Distance)
                .Select(pointAndDistance => pointAndDistance.Point)
                .ToArray();
        }
    }
}
