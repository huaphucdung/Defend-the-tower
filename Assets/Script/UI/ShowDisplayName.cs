using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class ShowDisplayName : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    [SerializeField] private TMP_Text NamePlayer;
    [SerializeField] private Slider HealthSlider;
    void Start()
    {
        NamePlayer.text= PV.Owner.NickName.Split("%")[0];
        if(PV.IsMine) {
           HealthSlider.gameObject.SetActive(false);
        }   
    }
}
