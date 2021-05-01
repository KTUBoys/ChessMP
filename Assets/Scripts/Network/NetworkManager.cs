using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static Assets.Scripts.GameManager;
using UnityEngine.SceneManagement;

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
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameView");
        }
    }

    //Scene loaded
    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameView")
        {
            Game.SetUpOnlineGame();
        }
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
