using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text RoomName;
    [SerializeField] private TMP_Text NumberPlayer;

    private RoomListingMenu roomMenu;

    public RoomInfo RoomInfo {get; private set;}

    void Start() {
        roomMenu = FindObjectOfType<RoomListingMenu>();
    }

    public void SetRoomInfo(RoomInfo roomInfo) 
    {
        RoomInfo = roomInfo;
        RoomName.text = roomInfo.Name;
        NumberPlayer.text = roomInfo.PlayerCount.ToString()+"/"+roomInfo.MaxPlayers.ToString();
    }

    public void OnClick_JoinRoom(RoomItem room) {
        roomMenu.JoinRoom(room.RoomInfo.Name);
    }
}
