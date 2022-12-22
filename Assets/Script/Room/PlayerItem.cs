using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private GameObject ready;
    [SerializeField] private GameObject leftArrowBtn, rightArrowBtn;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private RenderTexture[] rendetTextureList;
    public Player Player {get; private set;}

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    public void SetPlayerInfo(Player player) 
    {
        Player = player;
        playerName.text = player.NickName.Split("%")[0];
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChange() {
        leftArrowBtn.SetActive(true);
        rightArrowBtn.SetActive(true);
    }

    public bool ChangeState() {
        if(!ready.activeSelf) {
            leftArrowBtn.SetActive(false);
            rightArrowBtn.SetActive(false);
            PlayerProperties["IsReady"] = true;
        } else {
            leftArrowBtn.SetActive(true);
            rightArrowBtn.SetActive(true);
            PlayerProperties["IsReady"] = false;
        }
        PhotonNetwork.SetPlayerCustomProperties(PlayerProperties);
        return (bool)PlayerProperties["IsReady"];
    }

    public void OnClick_LeftArrowBtn() {
        if((int)PlayerProperties["CharacterClass"] == 0) {
            PlayerProperties["CharacterClass"] = rendetTextureList.Length - 1; 
        }
        else {
            PlayerProperties["CharacterClass"] = (int)PlayerProperties["CharacterClass"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(PlayerProperties);
    }

    public void OnClick_RightArrowBtn() {
        if((int)PlayerProperties["CharacterClass"] == rendetTextureList.Length - 1) {
            PlayerProperties["CharacterClass"] = 0; 
        }
        else {
            PlayerProperties["CharacterClass"] = (int)PlayerProperties["CharacterClass"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(PlayerProperties);
    }

    void UpdatePlayerItem(Player player) {
        if(player.CustomProperties.ContainsKey("CharacterClass")) {
            rawImage.texture = rendetTextureList[(int)player.CustomProperties["CharacterClass"]];
            PlayerProperties["CharacterClass"] = (int)player.CustomProperties["CharacterClass"];
        }
        else {
            PlayerProperties["CharacterClass"] = 0;
            rawImage.texture = rendetTextureList[0];
        } 

        if(player.CustomProperties.ContainsKey("IsReady")) {
            ready.SetActive((bool)player.CustomProperties["IsReady"]);
            playerName.gameObject.SetActive(!(bool)player.CustomProperties["IsReady"]);
            PlayerProperties["IsReady"] = player.CustomProperties["IsReady"];
        }
        else {
            PlayerProperties["IsReady"] = false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable hash) {
        if(targetPlayer == Player) {
            UpdatePlayerItem(targetPlayer);
        } 
    }
}
