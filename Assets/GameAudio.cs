using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudio : MonoBehaviour
{   
    [SerializeField] private AudioListener listener;
    [SerializeField] private AudioSource source;
    void Start() {
        MasterManager.LoadDataSetting();
        AudioListener.volume = (float) MasterManager.AudioSetting.audio / 100;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if(SceneManager.GetActiveScene().name != "Gameplay") {
            listener.enabled  = true;
            if(!source.isPlaying)
                source.Play();
        }
        else {
            source.Stop();
            listener.enabled  = false;
        }
    }
}
