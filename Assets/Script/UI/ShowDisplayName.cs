using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ShowDisplayName : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    [SerializeField] private TMP_Text NamePlayer;
    void Start()
    {
        NamePlayer.text= PV.Owner.NickName.Split("%")[0];   
    }
}
