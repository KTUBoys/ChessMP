using UnityEngine;
using static Assets.Scripts.ChessBoard;
using static Assets.Scripts.GameManager;

namespace Assets.Scripts
{
    public class TileSelector : MonoBehaviour
    {
        public GameObject TileHighlightPrefab;
        private GameObject _tileHighlight;

        void Start()
        {
            var point = PointFromGrid(new Vector2Int(0, 0));
            _tileHighlight = Instantiate(TileHighlightPrefab, point, Quaternion.Euler(-90f, 0f, 0f), gameObject.transform);
            _tileHighlight.SetActive(false);
        }

        void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var point = hit.point;
                var gridPoint = GridFromPoint(point);
                _tileHighlight.SetActive(true);
                _tileHighlight.transform.position =
                    PointFromGrid(gridPoint);
                if (Input.GetMouseButtonDown(0))
                {
                    Game.SoundManager.TakeAPiece();
                    var selectedPiece = Game.PieceAtGrid(gridPoint);
                    DefaultMaterial = selectedPiece?.GetComponent<MeshRenderer>().material;
                    if (Game.IsCurrentPlayerPiece(selectedPiece))
                    {
                        Game.SelectPiece(selectedPiece);
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
