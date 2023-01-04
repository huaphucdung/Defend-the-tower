using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseBackgournd;
    [SerializeField] private GameObject PauseOptionsMenu;

    private NewPlayerInput _input;
    void Awake() {
        _input = new NewPlayerInput();

        _input.UI.Pause.performed += ctx => ShowPauseMenu();
    }

    void ShowPauseMenu() {
        if(!PauseBackgournd.activeSelf && !PauseOptionsMenu.activeSelf) {
            PauseBackgournd.SetActive(true);
            PauseOptionsMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnEnable() {
        _input.Enable();
    }

    void OnDisable() {
        _input.Disable();
    }

}
