using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text RoomName;
    [SerializeField] private TMP_Text NumberPlayers;
    
    void Awake() {
        RoomName.text = "Room: "+ PhotonNetwork.CurrentRoom.Name;
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString(); 
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
