using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOverMenu : MonoBehaviourPunCallbacks
{
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClick_LeaveRoom() {
        PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() 
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
