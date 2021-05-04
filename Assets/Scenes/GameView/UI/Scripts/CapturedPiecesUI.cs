using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.UI;

public class CapturedPiecesUI : MonoBehaviour
{
    public GameObject PanelPrefab;
    public Transform ContentContainer;
    public List<GameObject> CapturedPiecesPanels;

    public ScrollRect ScrollRect;

    public void OnPieceCapture(PlayerType color, PieceType pieceType)
    {
        var newPanel = Instantiate(PanelPrefab);
        newPanel.transform.SetParent(ContentContainer);
        CapturedPiecesPanels.Add(newPanel);

        var pieceText = newPanel.GetComponentInChildren<Text>();
        var pieceColor = pieceText.transform.parent.GetComponent<Image>();

        // Assigns opposite captured color
        pieceColor.color = (color.Equals(PlayerType.White)) ? (Color.black) : (Color.white);
        
        // Assigns piece and an opposite color for it
        pieceText.text = AssignPiece(pieceType);
        pieceText.color = (pieceColor.color.Equals(Color.white)) ? (Color.black) : (Color.white);

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
