using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Testconnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.NickName = "Player A";
        PhotonNetwork.GameVersion = "0.1.0";
        PhotonNetwork.ConnectUsingSettings();        
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connect");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected (DisconnectCause cause) {
        Debug.Log("Cannot connect because " +cause.ToString());
    }
}
