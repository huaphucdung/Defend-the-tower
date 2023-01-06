using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomItem _roomPrefabs;
    
    private List<RoomItem> _roomList = new List<RoomItem>();
    
    //Connect to lobby from leave Room 
    public override void OnConnectedToMaster() {
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby(){
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach(RoomInfo info in roomList) {
            //Remove form rooms list
            if(info.RemovedFromList){
                int index = _roomList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1) {
                    Destroy(_roomList[index].gameObject);
                    _roomList.RemoveAt(index);
                }
            }
            //Add to rooms list
            else{
                //Check if has room same name but update number players will not add new and modify
                int index = _roomList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index == -1) {
                    RoomItem room = Instantiate(_roomPrefabs, _content);
                    if(room != null) {
                        room.SetRoomInfo(info);
                        _roomList.Add(room);
                    }
                }
                else {
                    _roomList[index].SetRoomInfo(info);
                }
            }
        }
    }

    public void JoinRoom(string _roomName) {
        if(!PhotonNetwork.InLobby && !PhotonNetwork.IsConnected) return;
        
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom() {;
        SceneManager.LoadScene("Room");
    }

    public void OnClick_Disconnect() {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected (DisconnectCause cause) {
        SceneManager.LoadScene("MainMenu");
    }
}
