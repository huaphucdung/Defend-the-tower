using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameStting")]
public class GameSetting : SingletonScriptableObject<GameSetting>
{
    /* [SerializeField] private string _nickName = "Player";

    public string NickName {
        get {
            int value = Random.Range(0, 999);
            return _nickName +" "+ value.ToString();
        }
    } */
}
