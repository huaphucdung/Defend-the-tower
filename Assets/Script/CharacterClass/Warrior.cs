using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Warrior", menuName = "Character Class/Warrior")]
public class Warrior : CharacterClass
{
    public override void DoAttack(Animator anim, int combo) {
        DoAnimation(anim, "Attack"+combo);
    }
    
    public override void DoSkill(Animator anim) {
        DoAnimation(anim, "Skill");
    }

    public override void DoRoll(Animator anim) {
        DoAnimation(anim, "Roll");
    }
}
