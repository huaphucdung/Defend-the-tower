using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreenSetting", menuName = "Manager/Screen")]
public class ScreenSetting : ScriptableObject {
    public bool fullScreen =  true;
    public int resolution = 0;
}
