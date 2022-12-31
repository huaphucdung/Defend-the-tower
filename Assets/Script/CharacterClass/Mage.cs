using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mage", menuName = "Character Class/Mage")]
public class Mage : CharacterClass
{
    public GameObjectPool particleAttack;
    public GameObjectPool particleSkill;

    public override void DoAttack(PlayerController player, Animator anim, int combo) {
        Transform trans = player.gameObject.transform;
        int dame = Damage;
        if(combo == 0) {
            dame -= 10;
        }
        particleAttack.SpawnObject(trans.position + new Vector3(0f, 0.5f, 0f) + trans.forward / 2 , Quaternion.LookRotation(trans.forward), dame, player.TargetLayer);
    }
    
    public override float DoSkill(PlayerController player, Animator anim) {
        Transform trans = player.gameObject.transform;
        particleSkill.SpawnObject(trans.position + new Vector3(0f, 2f, 0f), Quaternion.AngleAxis(30f, trans.right) * Quaternion.LookRotation(trans.forward), Damage * 2, player.TargetLayer); 
        return TimeResetSkill;
    }
}
