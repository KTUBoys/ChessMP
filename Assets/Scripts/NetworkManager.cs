using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIManager UIManager;

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        UIManager.SetConnectionStatusText(PhotonNetwork.NetworkClientState.ToString());
    }
    public void OnPlayOnlineClick()
    {
        ConnectToServer();
    }
    private void ConnectToServer()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null);
    }
}
