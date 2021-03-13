using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Pawn movement class inherited by GeneralMovement
    /// </summary>
    public class PawnMovement : GeneralMovement
    {
        private Vector3 pos;
        private Quaternion rot;
        private Vector3 defaultPos;
        private Vector3 mOffset;        // mouse offset
        private float mZCoord;          // mouse Z axis coords
        private Vector3 curPos;

        [UsedImplicitly]    // for resharper
        void OnMouseDown()
        {
            pos = gameObject.transform.position;        // Get current position
            rot = gameObject.transform.rotation;        // Get current rotation
            defaultPos = pos;                           // Put current position to default, to get default position later for any illegal moves


            // Get current mouse position, make offset.
            gameObject.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y + 1f, pos.z), rot);
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }

        [UsedImplicitly]    // For resharper
        void OnMouseDrag()
        {
            curPos = transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y,
                GetMouseWorldPos().z + mOffset.z);
        }
        
        [UsedImplicitly]    // For reshaper
        void OnMouseUp()
        {
            Move();
        }

        private Vector3 GetMouseWorldPos()
        {
            var mousePoint = Input.mousePosition;

            mousePoint.z = mZCoord * 1.45f;     // Multiplying by number due to mouse not actually following the piece correctly
            mousePoint.x *= 0.7f;               // Doing the same to X axis, so the piece doesn't move too fast

            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        private void Move()
        {
            // Check for out of borders
            if (OutOfBorders(curPos))
            {
                gameObject.transform.SetPositionAndRotation(defaultPos, rot);
            }   // Else move to the nearest applicable cell.
            else MoveToNearest(curPos, defaultPos, rot);
        }
    }
}
