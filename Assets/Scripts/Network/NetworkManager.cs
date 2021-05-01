using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static Assets.Scripts.GameManager;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIManager UIManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
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

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameView");
        }
        StartCoroutine("SetUpOnlineScene");
    }

    private IEnumerator SetUpOnlineScene() //should be a callback when scene is loaded
    {
        yield return new WaitForSeconds(1f);
        Game.SetUpOnlineGame();
    }
}
