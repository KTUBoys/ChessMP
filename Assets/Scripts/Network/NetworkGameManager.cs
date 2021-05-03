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
        canMove = false;
    }

    internal override void Move(GameObject piece, Vector2Int gridPoint)
    {
        Vector2 selectedPiece = GridForPiece(piece);
        PhotonView.RPC(nameof(RPC_Move), RpcTarget.AllBuffered, new object[] { selectedPiece, new Vector2(gridPoint.x, gridPoint.y) }); //Vector2Int not supported by Photon
    }

    [PunRPC]
    private void RPC_Move(Vector2 selectedPiece, Vector2 gridPoint)
    {
        canMove = !canMove;

        Vector2Int gridPointInteger = new Vector2Int(Mathf.RoundToInt(gridPoint.x), Mathf.RoundToInt(gridPoint.y));
        GameObject piece = PieceAtGrid(new Vector2Int(Mathf.RoundToInt(selectedPiece.x), Mathf.RoundToInt(selectedPiece.y)));

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
        _pieces[startGridPoint.y, startGridPoint.x] = null;
        _pieces[gp.y, gp.x] = piece;
        Board.MovePiece(piece, gridPointInteger);
    }

    //Hiding this when playing online since we dont need to change players
    public override void NextPlayer() { }
}
