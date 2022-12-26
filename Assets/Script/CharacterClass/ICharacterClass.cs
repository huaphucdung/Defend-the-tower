using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICharacterClass {
    
    void DoAttack(PlayerController player, Animator anim, int combo);
    float DoSkill(PlayerController player, Animator anim);
    void DoRoll(PlayerController player, Animator anim);
    void DoAnimation(PlayerController player, Animator anim, string name, float value);
    void DoAnimation(PlayerController player, Animator anim, string name);
} 

public abstract class CharacterClass : ScriptableObject, ICharacterClass {
    public WeaponType type;
    public int Health;
    public int Stamina;
    public int PhysicDame;
    public int MagicDame;
    public int Armor;
    public int WalkSpeed;
    public int RunSpeed;

    public int NumberCombo;    

    public float TimeResetSkill;

    public GameObject Model;
    public Avatar Avatar;
    public string DirAnimation;
    
    public abstract void DoAttack(PlayerController player, Animator anim, int combo);
    public abstract float DoSkill(PlayerController player, Animator anim);
    public void DoRoll(PlayerController player, Animator anim) {
        DoAnimation(player, anim, "Roll");
    }
    
    public void DoAnimation(PlayerController player, Animator anim, string name, float value) {
        anim.SetFloat(name,value);
    }

    public void DoAnimation(PlayerController player, Animator anim, string name) {
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