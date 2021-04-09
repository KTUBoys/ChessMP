using System;
using System.Drawing;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Scripts
{
    /// <summary>
    /// Pawn movement class inherited by GeneralMovement
    /// </summary>
    public class PawnMovement : GeneralMovement
    {
        private MovementSoundScript soundManager;
        private Quaternion _rot;
        private Vector3 _defaultPos;
        private Vector3 _mOffset;        // mouse offset
        private float _mZCoord;          // mouse Z axis coords
        private Vector3 _curPos;
        private Color _color;

        private void Awake()
        {
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<MovementSoundScript>();
        }

        [UsedImplicitly]    // for resharper
        void OnMouseDown()
        {
            MouseDown(out _rot, out _defaultPos, out _mZCoord, out _mOffset);
            GetPath();
            soundManager.TakeAPiece();
        }

        [UsedImplicitly]    // For resharper
        void OnMouseDrag()
        {
            MouseDrag(_mOffset, out _curPos, _mZCoord);
        }
        
        [UsedImplicitly]    // For resharper
        void OnMouseUp()
        {
            Cursor.visible = true;
            Move();
            foreach (var obj in GameObject.FindGameObjectsWithTag("path"))
            {
                Destroy(obj);
            }
            soundManager.MoveAPiece();
        }

        private void Move()
        {
            if (OutOfBorders(_curPos) || !LegalMove(_curPos, _defaultPos))
            {
                SetPosition(_defaultPos, _rot);
            }
            else MoveToNearest(_curPos, _defaultPos, _rot);
        }

        private void GetPath()
        {
            switch (gameObject.name.Split(' ')[0].ToLower())
            {
                case "white" when Math.Abs(_defaultPos.z - -59) < 1:
                    SpawnMovementPath(10, 2);
                    break;
                case "white" when Math.Abs(_defaultPos.z - -59) > 1:
                    SpawnMovementPath(10, 1);
                    break;
                case "black" when Math.Abs(_defaultPos.z - -9) < 1:
                    SpawnMovementPath(-10, 2);
                    break;
                default:
                    SpawnMovementPath(-10, 1);
                    break;
            }
        }

        private void SpawnMovementPath(float add, int times)
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Destroy(plane);
            for (var i = 1; i <= times; i++)
            {
                var path = Instantiate(plane, new Vector3(_defaultPos.x, -0.915f , _defaultPos.z + add * i),
                    Quaternion.identity);
                path.GetComponent<MeshRenderer>().material.color = Color.green;

                path.name = "path";
                path.tag = "path";
            }
        }
        public bool LegalMove(Vector3 curPos, Vector3 defaultPos)
        {
            // Take chessboard current stats
            var cells = ChessBoard.BoardCoords;
            var isFull = ChessBoard.IsFull;

            var neighbor = GetNeighbor(cells, new Point((int)curPos.x, (int)curPos.z));

            if (isFull[Array.FindIndex(cells, cell => cell.X == neighbor[0].X && cell.Y == neighbor[0].Y)])
            {
                return false;
            }

            if (gameObject.name.Split(' ')[0].ToLower() == "white" && Math.Abs(defaultPos.z - (-59)) < 0.5)
            {
                if (Math.Abs(curPos.z - defaultPos.z) < 0.5)
                {
                    return true;
                }
            }

            if (Math.Abs(curPos.x - defaultPos.x) < 1)
            {
                return true;
            }
            return false;
        }
    }
}
