using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData")]
public class PlayerData : ScriptableObject {
    public int _maxHealth;
    public int _maxMana;
    public float _speedMove;
    [Range(1, 100)]
    public int _sensitiveMouse;
}
