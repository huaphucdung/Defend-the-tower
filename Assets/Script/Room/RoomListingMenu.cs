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
                RoomItem room = Instantiate(_roomPrefabs, _content);
                if(room != null) {
                    room.SetRoomInfo(info);
                    _roomList.Add(room);
                }
            }
        }
    }

    public void JoinRoom(string _roomName) {
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom() {
        SceneManager.LoadScene("Room");
    }
}
