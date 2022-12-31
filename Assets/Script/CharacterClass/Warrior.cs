using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Warrior", menuName = "Character Class/Warrior")]
public class Warrior : CharacterClass
{
    public GameObjectPool particle;

    public override void DoAttack(PlayerController player, Animator anim, int combo) {
        Transform trans = player.gameObject.transform;
        int dame = Damage;;
        switch (combo) {
            case 1:
                particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f), Quaternion.LookRotation(trans.forward));
                dame -=12;
                break;
            case 2:
                particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f),  Quaternion.AngleAxis(220f, trans.forward) * Quaternion.LookRotation(trans.forward));
                break;
            default:
                particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.right * 0.3f, Quaternion.AngleAxis(250f, trans.forward) * Quaternion.LookRotation(trans.forward));
                dame -=12;
                break;
        }
        //Only work for Melee character
        Collider[] hitColliders = Physics.OverlapSphere(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , 1.5f, player.TargetLayer);
        foreach(Collider enemy in hitColliders) {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if(ai != null) {
                ai.TakeDame(dame);
            }
        }        
    }
    
    public override float DoSkill(PlayerController player, Animator anim) {
        player.SetState(PlayerState.Defense);
        return TimeResetSkill;
    }
}
