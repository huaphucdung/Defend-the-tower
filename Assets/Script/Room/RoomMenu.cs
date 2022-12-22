using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text RoomName;
    [SerializeField] private TMP_Text NumberPlayers;

    [SerializeField] private GameObject PlayerItemPrefab;
    [SerializeField] private Transform PlayerList;

    private List<PlayerItem> _playerList = new List<PlayerItem>();
    
    void Awake() {
        RoomName.text = "Room: "+ PhotonNetwork.CurrentRoom.Name;
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        GetCurrentRoomPlayer();
    }

    void GetCurrentRoomPlayer() {
        foreach (KeyValuePair<int, Player> _player in PhotonNetwork.CurrentRoom.Players) {
            if(_player.Value != PhotonNetwork.LocalPlayer)
                AddNewPlayer(_player.Value);
        }
        //For add last list
        AddNewPlayer(PhotonNetwork.LocalPlayer);
    }

    void AddNewPlayer(Player _player) {
        PlayerItem newPlayer = Instantiate(PlayerItemPrefab, PlayerList).GetComponent<PlayerItem>();
        newPlayer.SetPlayerInfo(_player);
        if(_player == PhotonNetwork.LocalPlayer) {
            newPlayer.ApplyLocalChange();
        }
        _playerList.Add(newPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        AddNewPlayer(newPlayer);
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log(otherPlayer);
        Debug.Log(_playerList[1].Player);
        int index = _playerList.FindIndex(x => x.Player == otherPlayer);
        if(index != -1) {
            Destroy(_playerList[index].gameObject);
            _playerList.RemoveAt(index);
            NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        }
    }

    public void OnClick_LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() 
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
