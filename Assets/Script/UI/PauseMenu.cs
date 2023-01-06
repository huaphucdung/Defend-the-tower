using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject PauseBackgournd;
    [SerializeField] private GameObject PauseOptionsMenu;
    [SerializeField] private GameObject OptionSettingMenu;
    [SerializeField] private GameObject GameplayUI;

    private PhotonView view;
    private NewPlayerInput _input;
    void Start() {
        foreach(PhotonView pv in FindObjectsOfType<PhotonView>()) {
            if(pv.IsMine) {
                _input = pv.gameObject.GetComponent<PlayerController>().Input;
                break;
            }
        }
        _input.UI.Pause.performed += ctx => ShowPauseMenu();
    }

    void ShowPauseMenu() {
        //Show Pause menu
        if(!PauseBackgournd.activeSelf && !PauseOptionsMenu.activeSelf) {
            PauseBackgournd.SetActive(true);
            PauseOptionsMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _input.Player.Disable();
            GameplayUI.SetActive(false);
        }
        //Turn off Menu option
        else if(OptionSettingMenu.activeSelf) {
            OptionSettingMenu.SetActive(false);
            PauseOptionsMenu.SetActive(true);
        }
        else {
            DisablePauseMenu();
            
        }
    }

    //Turn off Pause menu
    public void DisablePauseMenu() {
        PauseBackgournd.SetActive(false);
        PauseOptionsMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _input.Player.Enable();
        GameplayUI.SetActive(true);
    }

    public void OnClick_LeaveRoom() {
        PhotonNetwork.LocalPlayer.CustomProperties["IsReady"] = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() 
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
