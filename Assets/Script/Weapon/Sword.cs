using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Weapon/Sword")]
public class Sword : Weapon {
    
    private int modeAttack = 0;

    public override void DoAttack1() {

    }
    
    public override void DoAttack2() {

    }
    
    public override void DoSkill1() {

    }

    public override void DoSkill2() {

    }

    public override void DoSkill3() {

    }

    public override void DoSkill4() {
        modeAttack = (modeAttack!=0 ? 0: 1);
    }

    public override void DoAnimation(Animator anim, string name, float value) {
        switch (name) {
            case "Speed":
                anim.SetFloat(name, value);
                anim.SetFloat("Mode",modeAttack);
            break;
        }
    }
}
