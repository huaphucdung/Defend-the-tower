using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    private int Damage;
    private LayerMask layer;
    public void SetDamage(int number, LayerMask newLayer) {
        Damage = number;
        layer = newLayer;
    }

    //Only work with range attack Enemy and Player
    void OnParticleCollision(GameObject other) {
        //Check layermask
        if((layer.value & (1 << other.gameObject.layer)) > 0) {
            if(other.GetComponent<EnemyAI>() != null) {
                EnemyAI ai = other.GetComponent<EnemyAI>();
                ai.TakeDame(Damage);
            } 
            else {
                PlayerController PC = other.GetComponent<PlayerController>();
                if(PC != null)  PC.TakeDame(Damage);
                else{
                    TowerDefense tower = other.GetComponent<TowerDefense>();
                    tower.TakeDame(Damage);
                }
            }
            gameObject.SetActive(false);
        }
    }


    void OnEnable() {
        Invoke("AutoDisable", 2);
    }

    void AutoDisable() {
        gameObject.SetActive(false);
    }

    void OnDisable() {
        CancelInvoke();
    }
}
