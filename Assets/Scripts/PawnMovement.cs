﻿using System;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PawnMovement : MonoBehaviour
    {
        private Vector3 pos;
        private Quaternion rot;
        private Vector3 defaultPos;
        private Vector3 mOffset;        // mouse offset
        private float mZCoord;          // mouse Z axis coords
        private Vector3 curPos;

        void OnMouseDown()
        {
            pos = gameObject.transform.position;
            rot = gameObject.transform.rotation;
            defaultPos = pos;
            gameObject.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y + 1f, pos.z), rot);
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }

        private Vector3 GetMouseWorldPos()
        {
            var mousePoint = Input.mousePosition;

            mousePoint.z = mZCoord * 1.45f;
            mousePoint.x *= 0.7f;

            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        void OnMouseDrag()
        {
            curPos = transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y,
                GetMouseWorldPos().z + mOffset.z);
        }

        void OnMouseUp()
        {
            if (OutOfBorders())
            {
                gameObject.transform.SetPositionAndRotation(defaultPos, rot);
            }
            else MoveToNearest();

        }

        private bool OutOfBorders()
        {
            var rightBorder = 1f;
            var leftBorder = -80f;
            var topBorder = 6.65f;
            var bottomBorder = -75f;

            if (curPos.x >= rightBorder || curPos.x <= leftBorder)
            {
                return true;
            }

            if (curPos.z >= topBorder || curPos.z <= bottomBorder)
            {
                return true;
            }

            return false;
        }

        private void MoveToNearest()
        {
            pos = gameObject.transform.position;
            var cells = ChessBoard.BoardCoords;
            var isFull = ChessBoard.isFull;
            var origin = new Point((int) curPos.x, (int) curPos.z);
            var neighbors = cells.Select(p => new {Point = p, Distance = CalculateTwoDistances(origin, p)})
                .Where(pointAndDistance => pointAndDistance.Distance <= Math.Pow(7, 2))
                .OrderBy(pointAndDistance => pointAndDistance.Distance)
                .Select(pointAndDistance => pointAndDistance.Point)
                .ToArray();

            var defaultCellIndex = Array.FindIndex(cells, point => point.X == (int)defaultPos.x && point.Y == (int)defaultPos.z);
            var cellIndex = Array.FindIndex(cells, point => point.X == neighbors.First().X && point.Y == neighbors.First().Y);

            Debug.Log($"Chess piece at coordinates: {curPos.x};{curPos.z} was put on {isFull[cellIndex]} cell at {cells[cellIndex].X};{cells[cellIndex].Y}");

            if (isFull[cellIndex])
            {
                gameObject.transform.SetPositionAndRotation(defaultPos, rot);
            }
            else
            {
                gameObject.transform.SetPositionAndRotation(new Vector3(neighbors.First().X, -0.9f, neighbors.First().Y), rot);
                isFull[cellIndex] = true;
                isFull[defaultCellIndex] = false;
            }
        }

        private static float CalculateTwoDistances(Point originPoint, Point destinationPoint)
        {
            return (float) (Math.Pow(originPoint.X - destinationPoint.X, 2)
                            + Math.Pow(originPoint.Y - destinationPoint.Y, 2));
        }
    }
}
