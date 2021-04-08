using UnityEngine;

namespace Assets.Scripts
{
    public class TileSelector : MonoBehaviour
    {
        public GameObject TileHighlightPrefab;
        private GameObject _tileHighlight;

        void Start()
        {
            var gridPoint = ChessBoard.GridPoint(0, 0);
            var point = ChessBoard.PointFromGrid(gridPoint);
            _tileHighlight = Instantiate(TileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
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
                _tileHighlight.transform.position =
                    ChessBoard.PointFromGrid(gridPoint);
                if (Input.GetMouseButtonDown(0))
                {
                    var selectedPiece = GameManager.Game.PieceAtGrid(gridPoint);
                    ChessBoard.DefaultMaterial = selectedPiece?.GetComponent<MeshRenderer>().material;
                    if (GameManager.Game.IsCurrentPlayerPiece(selectedPiece))
                    {
                        GameManager.Game.SelectPiece(selectedPiece);
                        ExitState(selectedPiece);
                    }
                }
            }
            else
                _tileHighlight.SetActive(false);
        }

        internal void EnterState() => enabled = true;

        private void ExitState(GameObject movedPiece)
        {
            enabled = false;
            _tileHighlight.SetActive(false);
            GetComponent<MoveSelector>().EnterState(movedPiece);
        }
    }
}
