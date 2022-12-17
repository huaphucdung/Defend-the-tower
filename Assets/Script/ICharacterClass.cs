using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICharacterClass {
    
    void DoAttack(Animator anim, int combo);
    void DoSkill(Animator anim);
    void DoRoll(Animator anim);
    void DoAnimation(Animator anim, string name, float value);
} 

public abstract class CharacterClass : ScriptableObject, ICharacterClass {
    public int Health;
    public int Stamina;
    public int PhysicDame;
    public int MagicDame;
    public int Armor;
    public int NormalSpeed;
    public int RunSpeed;

    public int NumberCombo;    

    public string DirAnimation;
    
    public abstract void DoAttack(Animator anim, int combo);
    public abstract void DoSkill(Animator anim);
    public abstract void DoRoll(Animator anim);
    
    public void DoAnimation(Animator anim, string name, float value) {
        anim.SetFloat(name,value);
    }

    public void DoAnimation(Animator anim, string name) {
        anim.SetTrigger(name);
    }

    public int Start_Combo(int combo) {
        if(combo < NumberCombo) {
            return combo + 1;
        }
        return 0;
    }

    public int Finish_Anim(){
        return 0;
    }

}