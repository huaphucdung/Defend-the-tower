using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IWeapon {
    
    void DoAttack1();
    void DoAttack2();
    void DoSkill1();
    void DoSkill2();
    void DoSkill3();
    void DoSkill4();

    void DoAnimation(Animator anim, string name, float value);
} 

public abstract class Weapon : ScriptableObject, IWeapon {
    public string dirAnimation;
    public Sprite sprite1, sprite2 ,sprite3, sprite4;

    public abstract void DoAttack1();
    public abstract void DoAttack2();
    public abstract void DoSkill1();
    public abstract void DoSkill2();
    public abstract void DoSkill3();
    public abstract void DoSkill4();
    public abstract void DoAnimation(Animator anim, string name, float value);
}