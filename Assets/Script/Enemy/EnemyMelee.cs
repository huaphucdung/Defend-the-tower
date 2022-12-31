using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Melee", menuName = "Enemy/Enemy Melee")]
public class EnemyMelee : Enemy
{
    public override void DoAttack(EnemyAI AI, Animator anim, LayerMask layer) {
        Transform trans = AI.gameObject.transform;
        Collider[] hitColliders = Physics.OverlapSphere(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward, 0.5f, layer);
        foreach(Collider col in hitColliders) {
            PlayerController player = col.GetComponent<PlayerController>();
            if(player != null) {
                player.TakeDame(Damage);
            }
            else{
                TowerDefense tower = col.GetComponent<TowerDefense>();
                tower.TakeDame(Damage);
            }
        }
    }
}
