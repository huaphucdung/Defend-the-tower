using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudio : MonoBehaviour
{   

    [SerializeField] private AudioListener listener;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clip;

    private static GameAudio instance;

    void Awake() {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
        MasterManager.LoadDataSetting();
        AudioListener.volume = (float) MasterManager.AudioSetting.audio / 100;
        source.clip = clip[0];
    }

    void Update() {
        if(SceneManager.GetActiveScene().name != "Gameplay") {
            source.clip = clip[0];
            listener.enabled = true;
        }
        else {
            source.clip = clip[1];
            listener.enabled = false;
        }
        if(!source.isPlaying)
            source.Play();
    }
}
