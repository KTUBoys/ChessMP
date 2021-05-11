using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static Assets.Scripts.GameManager;
using UnityEngine.SceneManagement;
using Photon.Realtime;

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
        UIManager?.SetConnectionStatusText(PhotonNetwork.NetworkClientState.ToString());
    }
    public void OnPlayOnlineClick()
    {
        SceneManager.sceneLoaded += SceneLoaded;
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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
        Debug.LogError("Joining room failed! Creating a new one");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("Joined room in region: " + PhotonNetwork.CloudRegion);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameView_MultiPlayer");
        }
    }

    //Scene loaded
    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameView_MultiPlayer")
        {
            Game.SetUpOnlineGame();
        }
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
