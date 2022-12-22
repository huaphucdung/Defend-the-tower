using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField room;


    public void OnClick_CreateRoom() {
        if(!PhotonNetwork.IsConnected && !PhotonNetwork.InLobby) return;

        //Create room
        if(room.text.Length >=1) {
            PhotonNetwork.CreateRoom(room.text, new RoomOptions(){MaxPlayers = 4, BroadcastPropsChangeToAll = true});            
        }
    }
    
    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Room");
    }
}
