using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy {
    void DoAttack(EnemyAI AI, Animator anim, LayerMask layer);
    void DoAnimation(Animator anim, string name);
    void DoAnimation(Animator anim, string name, bool value);
}

public abstract class Enemy: ScriptableObject, IEnemy {
    public int Health;
    public int Damage;
    public int Speed;
    public int DelayTime;
    public float RangeAttack;

    public abstract void DoAttack(EnemyAI AI, Animator anim, LayerMask layer);

    public void DoAnimation(Animator anim, string name) {
        anim.SetTrigger(name);
    }
    
    public void DoAnimation(Animator anim, string name, bool value){
        anim.SetBool(name, value);
    }
}