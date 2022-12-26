using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField PlayerName;   
    public void Onclick_Connect() {
        if(PlayerName.text.Length > 0) {
            PhotonNetwork.NickName = PlayerName.text + "%" + Random.Range(0, 999);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }       
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
