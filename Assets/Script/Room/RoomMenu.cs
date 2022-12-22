using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text RoomName;
    [SerializeField] private TMP_Text NumberPlayers;
    
    [SerializeField] private GameObject PlayerItemPrefab;
    [SerializeField] private Transform PlayerList;

    [SerializeField] private Button StartBtn;
    [SerializeField] private Button ReadyBtn;

    private List<PlayerItem> _playerList = new List<PlayerItem>();
    
    void Awake() {
        RoomName.text = "Room: "+ PhotonNetwork.CurrentRoom.Name;
        NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        GetCurrentRoomPlayer();
        ShowStartBtnForMaster();
    }


    void ShowStartBtnForMaster() {
        if(PhotonNetwork.IsMasterClient)
            StartBtn.gameObject.SetActive(true);
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        ShowStartBtnForMaster();
    }

    void GetCurrentRoomPlayer() {
        foreach (KeyValuePair<int, Player> _player in PhotonNetwork.CurrentRoom.Players) {
            if(_player.Value != PhotonNetwork.LocalPlayer)
                AddNewPlayer(_player.Value);
        }
        //For add last in list
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
        int index = _playerList.FindIndex(x => x.Player == otherPlayer);
        if(index != -1) {
            Destroy(_playerList[index].gameObject);
            _playerList.RemoveAt(index);
            NumberPlayers.text= "Players: "+ PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        }
    }

    public void OnClick_Start() {
        int count = 0;
        foreach (KeyValuePair<int, Player> _player in PhotonNetwork.CurrentRoom.Players) {
            if(_player.Value.CustomProperties.ContainsKey("IsReady")) {
                if((bool)_player.Value.CustomProperties["IsReady"]) {
                    count++;
                }
            }
        }
        if(count == PhotonNetwork.CurrentRoom.PlayerCount) {
            Debug.Log("Can start game");
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable hash) {
        int count = 0;
        foreach (KeyValuePair<int, Player> _player in PhotonNetwork.CurrentRoom.Players) {
            if(_player.Value.CustomProperties.ContainsKey("IsReady")) {
                if((bool)_player.Value.CustomProperties["IsReady"]) {
                    count++;
                }
            }
        }
        if(count == PhotonNetwork.CurrentRoom.PlayerCount) {
            StartBtn.interactable = true;
        }
        else {
            StartBtn.interactable = false;
        }
    }

    public void OnClick_Ready() {
        int index = _playerList.FindIndex(x => x.Player == PhotonNetwork.LocalPlayer);
        if(index != -1) {
            if(_playerList[index].ChangeState()) {
                ReadyBtn.GetComponentInChildren<TMP_Text>().text = "Unready";
            } else {
                ReadyBtn.GetComponentInChildren<TMP_Text>().text = "Ready";
            }
        }
    }

    public void OnClick_LeaveRoom() {
        //Set properties is not ready when leave room
        PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() 
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
