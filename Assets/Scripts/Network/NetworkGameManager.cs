using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Photon.Pun;
using Assets.Scripts.Pieces;

[RequireComponent(typeof(PhotonView))]
public class NetworkGameManager : GameManager
{
    private PhotonView PhotonView;
    private Player tempCurrentPlayer;
    private Player tempOtherPlayer;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        Game = this;
    }

    protected override void Start()
    {
        base.Start();
        CanMove = false;
    }

    internal override void Move(GameObject piece, Vector2Int gridPoint)
    {
        Vector2 selectedPiece = GridForPiece(piece);
        PhotonView.RPC(nameof(RPC_Move), RpcTarget.AllBuffered, new object[] { selectedPiece, new Vector2(gridPoint.x, gridPoint.y) }); //Vector2Int not supported by Photon
    }

    [PunRPC]
    private void RPC_Move(Vector2 selectedPiece, Vector2 gridPoint)
    {
        CanMove = !CanMove;

        var gridPointInteger = new Vector2Int(Mathf.RoundToInt(gridPoint.x), Mathf.RoundToInt(gridPoint.y));
        var piece = PieceAtGrid(new Vector2Int(Mathf.RoundToInt(selectedPiece.x), Mathf.RoundToInt(selectedPiece.y)));

        if (Game.PieceAtGrid(gridPointInteger) != null)
        {
            CapturePieceAt(gridPointInteger);
        }

        var pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.Type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            PawnMoved(piece);
        }

        var gp = ChessBoard.PlaceFromGrid(gridPointInteger);
        var startGridPoint = PointForPiece(piece);
        Pieces[startGridPoint.y, startGridPoint.x] = null;
        Pieces[gp.y, gp.x] = piece;
        Board.MovePiece(piece, gridPointInteger);
        MovementHistoryUI.OnPieceMoved(CurrentPlayer, piece, startGridPoint, gp);
    }

    //Hiding this when playing online since we dont need to change players
    public override void NextPlayer() { }

    protected override void CapturePieceAt(Vector2Int gridPoint)
    {
        var capturePiece = PieceAtGrid(gridPoint);

        // --- Captured pieces UI update
        if (!canMove)
            CPUI_White.OnPieceCapture(PlayerType.White, capturePiece.GetComponent<Piece>().Type);
        else
            CPUI_Black.OnPieceCapture(PlayerType.Black, capturePiece.GetComponent<Piece>().Type);
        // ---

        if (capturePiece.GetComponent<Piece>().Type == PieceType.King)
        {
            Debug.Log(CurrentPlayer.Name + " winner!");
            Destroy(Board.GetComponent<TileSelector>());
        }
        Destroy(capturePiece);
    }
}
