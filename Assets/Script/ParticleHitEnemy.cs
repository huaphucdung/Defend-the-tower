using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitEnemy : MonoBehaviour
{
    private int Damage;

    public void SetDamage(int number) {
        Damage = number;
    }

    void OnParticleCollision(GameObject other) {
        //Only Word with range character
        Debug.Log("Damage: " + Damage);

        gameObject.SetActive(false);
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
