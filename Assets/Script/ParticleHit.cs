using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ParticleHit : MonoBehaviour
{
    private int Damage;
    private LayerMask layer;
    public void SetDamage(int number, LayerMask newLayer) {
        Damage = number;
        layer = newLayer;
    }

    //Only work with range attack Enemy and Player
    void OnParticleCollision(GameObject other) {
        //Check layermask
        if((layer.value & (1 << other.gameObject.layer)) > 0) {
            if(other.GetComponent<EnemyAI>() != null) {
                PhotonView ai = other.GetComponent<PhotonView>();
                //Only work on master view
                if(PhotonNetwork.IsMasterClient) {
                    ai.RPC("TakeDame", RpcTarget.AllBuffered, Damage);
                }
            } 
            else {
                PhotonView view = other.GetComponent<PhotonView>();
                //Only work when only my view hit damage
                if(other.tag == "Player" && view.IsMine) {
                    view.RPC("TakeDame", RpcTarget.AllBuffered, Damage);
                }
                else {
                    if(PhotonNetwork.IsMasterClient) {
                        PhotonView tower = other.GetComponent<PhotonView>();
                        tower.RPC("TakeDame", RpcTarget.AllBuffered, Damage);
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }


    void OnEnable() {
        Invoke("AutoDisable", 2);
    }

    void AutoDisable() {
        gameObject.SetActive(false);
    }

    void OnDisable() {
        CancelInvoke();
    }
}
