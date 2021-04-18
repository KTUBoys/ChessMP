using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class NetworkGameManager : GameManager
{
    private PhotonView PhotonView;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

/*    internal override void Move(GameObject piece, Vector2Int gridPoint)
    {
        PhotonView.RPC(nameof(MoveRPC), RpcTarget.AllBuffered, new object[] { gridPoint });
    }*/

    [PunRPC]
    private void MoveRPC()
    {

    }
}
