using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Archer", menuName = "Character Class/Archer")]
public class Archer : CharacterClass
{
    public GameObjectPool particle;

    public override void DoAttack(PlayerController player, Animator anim, int combo) {
        Transform trans = player.gameObject.transform;
        int dame = Damage;
        if(combo == 0) {
            dame -= 10;
        }
        particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.LookRotation(trans.forward), dame, player.TargetLayer);
    }
    
    public override float DoSkill(PlayerController player, Animator anim) {
        Transform trans = player.gameObject.transform;
        //Forward
        particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.LookRotation(trans.forward), Damage, player.TargetLayer);
        //Left
        particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.AngleAxis(15f, trans.up) * Quaternion.LookRotation(trans.forward), Damage, player.TargetLayer);
        //Right
        particle.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.AngleAxis(-15f, trans.up) * Quaternion.LookRotation(trans.forward) , Damage, player.TargetLayer);
    
        return TimeResetSkill;
    }

}
