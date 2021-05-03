using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.GameManager;

[RequireComponent(typeof(PhotonView))]
public class SelectPreferredColor : MonoBehaviour
{
    private enum ChessColor { None, Black, White}
    private PhotonView PhotonView;
    [SerializeField] private Image WhiteSelected;
    [SerializeField] private Image BlackSelected;
    [SerializeField] private Color YourSelectionButtonColor;
    [SerializeField] private Color OpponentSelectionButtonColor;
    [SerializeField] private Button WhiteButton;
    [SerializeField] private Button BlackButton;
    private ChessColor YourSelection = ChessColor.None;
    private ChessColor OpponentSelection = ChessColor.None;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void WhiteClicked()
    {
        PhotonView.RPC(nameof(RPC_WhiteClicked), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_WhiteClicked(PhotonMessageInfo info)
    {
        WhiteSelected.enabled = true;
        //You clicked
        if (info.Sender.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            WhiteSelected.color = YourSelectionButtonColor;
            BlackButton.interactable = false;
            WhiteButton.interactable = false;
            YourSelection = ChessColor.White;
        }
        //Opponent clicked
        else
        {
            OpponentSelection = ChessColor.White;
            WhiteSelected.color = OpponentSelectionButtonColor;
        }
        ProccessSelections();
    }

    public void BlackClicked()
    {
        PhotonView.RPC(nameof(RPC_BlackClicked), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_BlackClicked(PhotonMessageInfo info)
    {
        BlackSelected.enabled = true;
        //You clicked
        if (info.Sender.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            BlackSelected.color = YourSelectionButtonColor;
            WhiteButton.interactable = false;
            BlackButton.interactable = false;
            YourSelection = ChessColor.Black;
        }
        //Opponent clicked
        else
        {
            OpponentSelection = ChessColor.Black;
            BlackSelected.color = OpponentSelectionButtonColor;
        }
        ProccessSelections();
    }

    private void ProccessSelections()
    {
        if (YourSelection == ChessColor.None || OpponentSelection == ChessColor.None)
            return;

        if(YourSelection == ChessColor.White && OpponentSelection == ChessColor.Black)
        {
            Game.SetCurrentPlayer(true);
        } 
        else if(YourSelection == ChessColor.Black && OpponentSelection == ChessColor.White)
        {
            Game.SetCurrentPlayer(false);
        }
        //Assign random since selections match
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                bool isWhite = Random.Range(0, 2) == 0;
                Game.SetCurrentPlayer(isWhite);
                PhotonView.RPC(nameof(RPC_AssignRandomColor), RpcTarget.AllBuffered, new object[] { !isWhite });
            }
        }

        gameObject.SetActive(false);
    }

    [PunRPC]
    private void RPC_AssignRandomColor(bool isWhite)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Game.SetCurrentPlayer(isWhite);
        }
    }
}
