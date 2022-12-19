using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField PlayerName;   
    public void Onclick_Connect() {
        if(PlayerName.text.Length > 0) {
            PhotonNetwork.NickName = PlayerName.text + "%" + Random.Range(0, 999); 
            PhotonNetwork.ConnectUsingSettings();
        }       
    }

    public override void OnConnectedToMaster() {
        SceneManager.LoadScene("Lobby");
        PhotonNetwork.JoinLobby();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
    }
}
