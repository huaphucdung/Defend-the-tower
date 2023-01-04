using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffectSound : MonoBehaviour
{
    private AudioSource btnSoundEffect;   

    void Awake() {
        btnSoundEffect = GetComponent<AudioSource>();
    } 

    public void BtnSoundClick() {
        if(!btnSoundEffect.isPlaying)
                btnSoundEffect.Play();
    }
}
