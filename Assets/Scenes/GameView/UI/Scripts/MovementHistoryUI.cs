using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.UI;

public class MovementHistoryUI : MonoBehaviour
{
    public GameObject PanelPrefab;
    public Transform ContentContainer;
    public List<GameObject> MovementHistoryPanels;

    public ScrollRect ScrollRect;

    private static string[,] Tiles = new string[8,8]
    {
        { "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8" },
        { "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7" },
        { "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6" },
        { "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5" },
        { "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4" },
        { "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3" },
        { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2" },
        { "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1" }
    };

    public void OnPieceMoved(Player player, GameObject piece, Vector2Int oldPos, Vector2Int newPos)
    {
        var newPanel = Instantiate(PanelPrefab);
        newPanel.transform.SetParent(ContentContainer);
        MovementHistoryPanels.Add(newPanel);

        var moveText = newPanel.transform.Find("MoveText").GetComponent<Text>();
        var pieceColor = newPanel.transform.Find("Color").GetComponent<Image>();
        var pieceText = pieceColor.transform.Find("PieceText").GetComponent<Text>();

        moveText.text = $"{Tiles[oldPos.x, oldPos.y]} > {Tiles[newPos.x, newPos.y]}";
        pieceColor.color = (player.PlayerType == PlayerType.White) ? (Color.white) : (Color.black);
        pieceText.text = AssignPiece(piece.GetComponent<Piece>().Type);
        pieceText.color = (pieceColor.color == Color.white) ? (Color.black) : (Color.white);

        ScrollTo(newPanel.GetComponent<RectTransform>());
    }

    private string AssignPiece(PieceType pieceType)
    {
        string piece = string.Empty;

        switch (pieceType)
        {
            case PieceType.Pawn:
                piece = "♙";
                break;

            case PieceType.King:
                piece = "♔";
                break;

            case PieceType.Queen:
                piece = "♕";
                break;

            case PieceType.Bishop:
                piece = "♗";
                break;

            case PieceType.Knight:
                piece = "♘";
                break;

            case PieceType.Rook:
                piece = "♖";
                break;
        }

        return piece;
    }

    private void ScrollTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform contentContainer = ContentContainer.GetComponent<RectTransform>();

        contentContainer.anchoredPosition =
            (Vector2)ScrollRect.transform.InverseTransformPoint(contentContainer.position)
            - (Vector2)ScrollRect.transform.InverseTransformPoint(target.position);
    }
}
