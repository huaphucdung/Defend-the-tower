using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSetting", menuName = "Manager/Audio")]
public class AudioSetting : ScriptableObject {
    [Range(0,100)]
    public int audio = 100;
}
