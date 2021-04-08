using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoveSelector : MonoBehaviour
    {
        public GameObject MoveLocationPrefab;
        public GameObject TileHighlightPrefab;
        public GameObject AttackLocationPrefab;

        private GameObject _tileHighlight;
        private GameObject _movingPiece;
        private List<Vector2Int> _moveLocations;
        private List<GameObject> _locationHighlights;

        void Start()
        {
            enabled = false;
            _tileHighlight = Instantiate(TileHighlightPrefab, ChessBoard.PointFromGrid(new Vector2Int(0, 0)),
                Quaternion.identity, gameObject.transform);
            _tileHighlight.SetActive(false);
        }

        void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var point = hit.point;
                var gridPoint = ChessBoard.GridFromPoint(point);

                _tileHighlight.SetActive(true);
                _tileHighlight.transform.position = ChessBoard.PointFromGrid(gridPoint);
                var gp = new Vector2Int((int) ChessBoard.PointFromGrid(gridPoint).x,
                    (int) ChessBoard.PointFromGrid(gridPoint).z);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log(_movingPiece);
                    if (!_moveLocations.Contains(gp))
                    {
                        CancelMove();
                        return;
                    }
                    
                    if (GameManager.Game.PieceAtGrid(gridPoint) == null)
                    {
                        GameManager.Game.Move(_movingPiece, gridPoint);
                    }
                    else
                    {
                        GameManager.Game.CapturePieceAt(gridPoint);
                        GameManager.Game.Move(_movingPiece, gridPoint);
                    }
                    ExitState();
                }
            }
            else
            {
                _tileHighlight.SetActive(false);
            }
        }

        private void CancelMove()
        {
            this.enabled = false;

            foreach (var highlight in _locationHighlights)
            {
                Destroy(highlight);
            }

            _tileHighlight.SetActive(false);
            GameManager.Game.DeselectPiece(_movingPiece);
            var selector = GetComponent<TileSelector>();
            selector.EnterState();
        }

        public void EnterState(GameObject piece)
        {
            _movingPiece = piece;
            this.enabled = true;
            _moveLocations = GameManager.Game.LegalMoves(_movingPiece);
            _locationHighlights = new List<GameObject>();

            if (_moveLocations.Count == 0)
            {
                Debug.Log("No move locations found");
                CancelMove();
            }

            foreach (var loc in _moveLocations)
            {
                GameObject highlight;
                if (GameManager.Game.PieceAtGrid(loc))
                {
                    Debug.Log($"Enemy at location {loc}");
                    highlight = Instantiate(AttackLocationPrefab, ChessBoard.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
                }
                else
                {
                    Debug.Log("No enemy");
                    highlight = Instantiate(MoveLocationPrefab, ChessBoard.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
                }
                _locationHighlights.Add(highlight);
            }
        }

        private void ExitState()
        {
            this.enabled = false;
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera.transform.Rotate(Vector3.back, 180f);
            var selector = GetComponent<TileSelector>();
            _tileHighlight.SetActive(false);
            GameManager.Game.DeselectPiece(_movingPiece);
            _movingPiece = null;
            GameManager.Game.NextPlayer();
            selector.EnterState();
            foreach (var highlight in _locationHighlights)
            {
                Destroy(highlight);
            }
        }
    }
}
