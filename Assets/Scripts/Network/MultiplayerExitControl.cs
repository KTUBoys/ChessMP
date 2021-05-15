using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerExitControl : MonoBehaviour
{
    private PhotonView pw;

    private void Awake()
    {
        pw = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pw.RPC(nameof(RPC_DisconnectAll), RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_DisconnectAll()
    {
        PhotonNetwork.Disconnect();
    }
}
