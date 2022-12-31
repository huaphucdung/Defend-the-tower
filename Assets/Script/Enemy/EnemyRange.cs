using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Range", menuName = "Enemy/Enemy Range")]
public class EnemyRange : Enemy
{
    public GameObjectPool Particle;

    public override void DoAttack(EnemyAI AI, Animator anim, LayerMask layer) {
        Transform trans = AI.gameObject.transform;
        Particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.LookRotation(trans.forward), Damage, layer);
    }
}
