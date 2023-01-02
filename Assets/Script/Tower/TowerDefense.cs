using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowerDefense : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    
    private int health; 

    public int MaxHealthTower => maxHealth;
    public int HealthTower => health;
    void Awake() {
        health = maxHealth;
    }

    [PunRPC]
    public void TakeDame(int damage) {
        health -= damage;

        if(health <=0) {
            health = 0;
            EndGame();
        }
    }

    void EndGame() {
        Debug.Log("end game");
    }
}
