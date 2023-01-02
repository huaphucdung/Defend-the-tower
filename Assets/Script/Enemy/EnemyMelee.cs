using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Enemy Melee", menuName = "Enemy/Enemy Melee")]
public class EnemyMelee : Enemy
{
    public override void DoAttack(EnemyAI AI, Animator anim, LayerMask layer) {
        Transform trans = AI.gameObject.transform;
        Collider[] hitColliders = Physics.OverlapSphere(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward, 0.5f, layer);
        foreach(Collider col in hitColliders) {
            PhotonView view = col.GetComponent<PhotonView>();
            if(view != null && col.gameObject.tag == "Player") {
                //Only work when only my view hit damage
                if(view.IsMine)
                    view.RPC("TakeDame", RpcTarget.AllBuffered, Damage);
            }
            else{
                //Only work on master view
                if(PhotonNetwork.IsMasterClient) {
                    PhotonView tower = col.GetComponent<PhotonView>();
                    tower.RPC("TakeDame", RpcTarget.AllBuffered, Damage);
                }
                
            }
        }
    }
}
