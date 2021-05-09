﻿using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        /*<Network variables> These are implemented here so that Networked games and local games would work on the same scene */
        [SerializeField] private NetworkGameManager networkGame;
        [SerializeField] private GameObject TeamSelect;
        protected bool canMove = true;
        /*</Network variables>*/

        public ChessBoard Board;
        public GameObject PawnPiece;
        public GameObject RookPiece;
        public GameObject QueenPiece;
        public GameObject KingPiece;
        public GameObject BishopPiece;
        public GameObject KnightPiece;
        public MovementSoundScript SoundManager;
        private Camera WhiteCamera;
        private Camera BlackCamera;

        public static GameManager Game;

        private const float YAxis = -0.9f;
        protected GameObject[,] Pieces;
        private List<GameObject> _movedPawns;
        private Player _white;
        private Player _black;
        public Player CurrentPlayer;
        private Player _otherPlayer;

        private void Awake()
        {
            SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<MovementSoundScript>();
            Game = this;
        }

        protected virtual void Start()
        {
            WhiteCamera = Camera.allCameras[1];
            BlackCamera = Camera.allCameras[2];

            WhiteCamera.enabled = true;
            BlackCamera.enabled = false;
            Pieces = new GameObject[8, 8];
            _movedPawns = new List<GameObject>();

            _white = new Player("white", true, true);
            _black = new Player("black", false, false);

            CurrentPlayer = _white;
            _otherPlayer = _black;

            InitialSetup();
        }
        private void InitialSetup()
        {
            Quaternion[] pieceAngles =
            {
                Quaternion.Euler(-90, 180, 0), // [0] - WHITE,
                Quaternion.Euler(-90, 0, 0) // [1] - BLACK
            };
            int[] xPoints =
            {
                -75, // ROOKS AND PAWNS
                -65, // KNIGHTS
                -55, // BISHOPS
                -45, // QUEEN
                -35 // KING

            };
            int[] zPoints =
            {
                -59, // PAWNS ROW WHITE
                -69, // BACK ROW WHITE
                -9, // PAWNS ROW BLACK
                1 // BACK ROW BLACK
            };

            SpawnFigures(xPoints, zPoints, pieceAngles);
        }

        private void SpawnFigures(int[] xPoints, int[] zPoints, Quaternion[] pieceAngles)
        {
            var white = pieceAngles[0];
            SpawnPaws(xPoints[0], zPoints[0], white, 1);
            SpawnRooks(xPoints[0], zPoints[1], white, 1);
            SpawnKnights(xPoints[1], zPoints[1], white, 1);
            SpawnBishops(xPoints[2], zPoints[1], white, 1);
            SpawnQueen(xPoints[3], zPoints[1], white, 1);
            SpawnKing(xPoints[4], zPoints[1], white, 1);

            // Spawn Black figures
            var black = pieceAngles[1];

            SpawnPaws(xPoints[0], zPoints[2], black, 0);
            SpawnRooks(xPoints[0], zPoints[3], black, 0);
            SpawnKnights(xPoints[1], zPoints[3], black, 0);
            SpawnBishops(xPoints[2], zPoints[3], black, 0);
            SpawnQueen(xPoints[3], zPoints[3], black, 0);
            SpawnKing(xPoints[4], zPoints[3], black, 0);
        }

        private void SpawnPaws(float x, float z, Quaternion quaternion, int color)
        {
            PawnPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            var c = "Black";
            if (color == 1)
            {
                c = "White";
            }

            for (var i = 0; i < 8; i++)
            {
                var pawnName = $"{c} Pawn {i + 1}";
                var pawn = Instantiate(PawnPiece, new Vector3(x, YAxis, z), quaternion);
                pawn.AddComponent<BoxCollider>();
                var piece = pawn.AddComponent<Pawn>();
                piece.Type = PieceType.Pawn;
                pawn.name = pawnName;
                x += 10;

                if (color == 1)
                {
                    AddPiece(pawn, _white, i, 6);
                }
                else
                {
                    AddPiece(pawn, _black, i, 1);
                }
            }
        }

        private void SpawnRooks(float x, float z, Quaternion quaternion, int color)
        {
            RookPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                var rook = Instantiate(RookPiece, new Vector3(x, YAxis, z), quaternion);
                rook.AddComponent<BoxCollider>();
                var piece = rook.AddComponent<Rook>();
                piece.Type = PieceType.Rook;
                x += 70;
                if (color == 1)
                {
                    AddPiece(rook, _white, i == 0 ? 0 : 7, 7);
                }
                else
                {
                    AddPiece(rook, _black, i == 0 ? 0 : 7, 0);
                }
            }
        }

        private void SpawnKnights(float x, float z, Quaternion quaternion, int color)
        {
            KnightPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                var knight = Instantiate(KnightPiece, new Vector3(x, YAxis, z), quaternion);
                knight.AddComponent<BoxCollider>();
                var piece = knight.AddComponent<Knight>();
                piece.Type = PieceType.Knight;
                x += 50;
                if (color == 1)
                {
                    AddPiece(knight, _white, i == 0 ? 1 : 6, 7);
                }
                else
                {
                    AddPiece(knight, _black, i == 0 ? 1 : 6, 0);
                }
            }
        }

        private void SpawnBishops(float x, float z, Quaternion quaternion, int color)
        {
            BishopPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            for (var i = 0; i < 2; i++)
            {
                var bishop = Instantiate(BishopPiece, new Vector3(x, YAxis, z), quaternion);
                bishop.AddComponent<BoxCollider>();
                var piece = bishop.AddComponent<Bishop>();
                piece.Type = PieceType.Bishop;
                x += 30;
                if (color == 1)
                {
                    AddPiece(bishop, _white, i == 0 ? 2 : 5, 7);
                }
                else
                {
                    AddPiece(bishop, _black, i == 0 ? 2 : 5, 0);
                }
            }
        }

        private void SpawnKing(float x, float z, Quaternion quaternion, int color)
        {
            KingPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            var king = Instantiate(KingPiece, new Vector3(x, YAxis, z), quaternion);
            king.AddComponent<BoxCollider>();
            var piece = king.AddComponent<King>();
            piece.Type = PieceType.King;
            if (color == 1)
            {
                AddPiece(king, _white, 4, 7);
            }
            else
            {
                AddPiece(king, _black, 4, 0);
            }
        }

        private void SpawnQueen(float x, float z, Quaternion quaternion, int color)
        {
            QueenPiece.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[color];
            var queen = Instantiate(QueenPiece, new Vector3(x, YAxis, z), quaternion);
            queen.AddComponent<BoxCollider>();
            var piece = queen.AddComponent<Queen>();
            piece.Type = PieceType.Queen;
            if (color == 1)
            {
                AddPiece(queen, _white, 3, 7);
            }
            else
            {
                AddPiece(queen, _black, 3, 0);
            }
        }

        private void AddPiece(GameObject piece, Player player, int column, int row)
        {
            player.Pieces.Add(piece);
            Pieces[column, row] = piece;
        }

        internal List<Vector2Int> LegalMoves(GameObject pieceGameObject)
        {
            var piece = pieceGameObject.GetComponent<Piece>();
            var gridPoint = GridForPiece(pieceGameObject);
            var locations = piece.MoveLocations(gridPoint);
            locations.RemoveAll(gp => gp.x < -80 || gp.y < -72 || gp.x > 2 || gp.y > 2);
            locations.RemoveAll(FriendlyPieceAt);
            return locations;
        }

        internal virtual void Move(GameObject piece, Vector2Int gridPoint)
        {
            if(Game.PieceAtGrid(gridPoint) != null)
            {
                CapturePieceAt(gridPoint);
            }

            var pieceComponent = piece.GetComponent<Piece>();
            if (pieceComponent.Type == PieceType.Pawn && !HasPawnMoved(piece))
            {
                PawnMoved(piece);
            }

            var gp = ChessBoard.PlaceFromGrid(gridPoint);
            var startGridPoint = PointForPiece(piece);
            Pieces[startGridPoint.y, startGridPoint.x] = null;
            Pieces[gp.y, gp.x] = piece;
            Board.MovePiece(piece, gridPoint);
        }

        protected void PawnMoved(GameObject pawn)
        {
            _movedPawns.Add(pawn);
        }

        internal bool HasPawnMoved(GameObject pawn)
        {
            return _movedPawns.Contains(pawn);
        }

        internal void CapturePieceAt(Vector2Int gridPoint)
        {
            var capturePiece = PieceAtGrid(gridPoint);
            if (capturePiece.GetComponent<Piece>().Type == PieceType.King)
            {
                Debug.Log(CurrentPlayer.Name + " winner!");
                Destroy(Board.GetComponent<TileSelector>());
            }
            Destroy(capturePiece);
        }

        internal void SelectPiece(GameObject piece)
        {
            Board.SelectPiece(piece);
        }

        internal void DeselectPiece(GameObject piece)
        {
            Board.DeselectPiece(piece);
        }

        internal bool IsCurrentPlayerPiece(GameObject piece)
        {
            return CurrentPlayer.Pieces.Contains(piece) && canMove;
        }

        internal GameObject PieceAtGrid(Vector2Int gridPoint)
        {
            if (gridPoint.x > -80 || gridPoint.y > -71 || gridPoint.x < 2 || gridPoint.y < 2)
            {
                for (var i = 0; i < 8; i++)
                {
                    for (var j = 0; j < 8; j++)
                    {
                        var point = ChessBoard.GridPoints[i, j];
                        if (Math.Abs(gridPoint.x - point.x) < 4 && Math.Abs(gridPoint.y - point.y) < 4)
                        {
                            return Pieces[j, i];
                        }
                    }
                }
            }
            return null;
        }

        protected Vector2Int GridForPiece(GameObject pieceGameObject)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (Pieces[i, j] == pieceGameObject)
                    {
                        return ChessBoard.GridPoints[j, i];
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        protected Vector2Int PointForPiece(GameObject pieceGameObject)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (Pieces[i, j] == pieceGameObject)
                    {
                        return new Vector2Int(j, i);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        private bool FriendlyPieceAt(Vector2Int gridPoint)
        {
            var piece = PieceAtGrid(gridPoint);

            if (piece is null)
            {
                return false;
            }

            return !_otherPlayer.Pieces.Contains(piece);
        }

        public virtual void NextPlayer()
        {
            var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            var temp = CurrentPlayer;
            CurrentPlayer = _otherPlayer;
            _otherPlayer = temp;
            if (BlackCamera.enabled == false)
            {
                SwitchToBlackCamera(mainCam);
            }
            else
            {
                SwitchToWhiteCamera(mainCam);
            }
        }

        private void SwitchToBlackCamera(GameObject mainCam)
        {
            WhiteCamera.enabled = false;
            BlackCamera.enabled = true;
            mainCam.transform.SetPositionAndRotation(new Vector3(-40.1f, 61, -34.1f), Quaternion.Euler(90, 180, 0));
        }

        private void SwitchToWhiteCamera(GameObject mainCam)
        {
            BlackCamera.enabled = false;
            WhiteCamera.enabled = true;
            mainCam.transform.SetPositionAndRotation(new Vector3(-39.9f, 61, -34.1f), Quaternion.Euler(90, 0, 0));
        }

        /*Methods used in networked games. Ideally they could be replaced by the methods used in local games,
          but because of the local game structure these are needed */
        public void SetCurrentPlayer(bool isWhite)
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (isWhite)
            {
                CurrentPlayer = _white;
                _otherPlayer = _black;
                canMove = true;
                SwitchToWhiteCamera(mainCamera);
            }
            else
            {
                canMove = false;
                CurrentPlayer = _black;
                _otherPlayer = _white;
                SwitchToBlackCamera(mainCamera);
            }
        }

        public void SetUpOnlineGame()
        {
            TeamSelect.SetActive(true);
            networkGame.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
