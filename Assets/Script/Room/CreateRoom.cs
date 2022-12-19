using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField room;


    public void OnClick_CreateRoom() {
        if(!PhotonNetwork.IsConnected) return;

        //Create room
        if(room.text.Length >=1) {
            PhotonNetwork.CreateRoom(room.text, new RoomOptions(){MaxPlayers = 4});            
        }
    }

    public override void OnJoinedRoom() {
        SceneManager.LoadScene("Room");
    }
}
