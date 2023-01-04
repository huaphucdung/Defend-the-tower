using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseSetting", menuName = "Manager/Mouse")]
public class MouseSetting : ScriptableObject {
    [Range(1,50)]
    public int sensitive = 25;
}