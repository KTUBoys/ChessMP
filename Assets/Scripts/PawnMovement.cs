using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Pawn movement class inherited by GeneralMovement
    /// </summary>
    public class PawnMovement : GeneralMovement
    {
        private Quaternion _rot;
        private Vector3 _defaultPos;
        private Vector3 _mOffset;        // mouse offset
        private float _mZCoord;          // mouse Z axis coords
        private Vector3 _curPos;
        private Color _color;

        [UsedImplicitly]    // for resharper
        void OnMouseDown()
        {
            MouseDown(out _rot, out _defaultPos, out _mZCoord, out _mOffset);
            GetPath();
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
            var plane = ObjectFactory.CreatePrimitive(PrimitiveType.Plane);
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
    }
}
