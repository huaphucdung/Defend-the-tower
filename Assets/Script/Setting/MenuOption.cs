using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class MenuOption : MonoBehaviour
{   
    [SerializeField] private Toggle Toggle;
    [SerializeField] private TMP_Dropdown Dropdown;
    [SerializeField] private Slider AudioSlider;
    [SerializeField] private Slider MouseSensitiveSlider;
    

    void OnEnable() {
        MasterManager.LoadDataSetting();
        Toggle.isOn = MasterManager.ScreenSetting.fullScreen;
        Dropdown.value = MasterManager.ScreenSetting.resolution;

        AudioSlider.value = MasterManager.AudioSetting.audio;
        MouseSensitiveSlider.value = MasterManager.MouseSetting.sensitive;

        AudioListener.volume = (float) AudioSlider.value / 100;
    }
    
    public void ChangeFullScreen(Toggle _toggle) {
        Screen.fullScreen = _toggle.isOn;
    }

    public void ChangeResolution(TMP_Dropdown _dropdown) {
        var resolution = _dropdown.options[_dropdown.value].text.Split(" x ");
        Screen.SetResolution(int.Parse(resolution[0]),int.Parse(resolution[1]), Toggle.isOn);
    }

    public void ChangeAudioSlider(Slider _audioSlider) {
        AudioListener.volume = (float) _audioSlider.value / 100;
        
    }

    public void ChangeMouseSensitiveSlider(Slider _mouseSensitiveSlider) { 
    }

    public void SetDefault() {
        Toggle.isOn = true;
        Dropdown.value  = 0;
        AudioSlider.value = 100;
        MouseSensitiveSlider.value = 25;

        var resolution = Dropdown.options[0].text.Split(" x ");
        Screen.SetResolution(int.Parse(resolution[0]),int.Parse(resolution[1]), Toggle.isOn);
        AudioListener.volume = (float) AudioSlider.value / 100;
    }

    void OnDisable() {
        MasterManager.ScreenSetting.fullScreen = Toggle.isOn;
        MasterManager.ScreenSetting.resolution = Dropdown.value;

        MasterManager.AudioSetting.audio = (int) AudioSlider.value;
        MasterManager.MouseSetting.sensitive = (int) MouseSensitiveSlider.value;
        MasterManager.SaveDataSetting();

        foreach(PlayerController PC in FindObjectsOfType<PlayerController>()) {
            PC.SetMouseSensitive();
        }
    }

    void OnApplicationQuit() {
        MasterManager.ScreenSetting.fullScreen = Toggle.isOn;
        MasterManager.ScreenSetting.resolution = Dropdown.value;

        MasterManager.AudioSetting.audio = (int) AudioSlider.value;
        MasterManager.MouseSetting.sensitive = (int) MouseSensitiveSlider.value;
        MasterManager.SaveDataSetting();
    }
}
