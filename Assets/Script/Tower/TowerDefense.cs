using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowerDefense : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    
    private int health; 
    private bool IsEnd = false; 
    public int MaxHealthTower => maxHealth;
    public int HealthTower => health;
    void Awake() {
        health = maxHealth;
    }

    [PunRPC]
    public void TakeDame(int damage) {
        health -= damage;

        if(health <=0 && !IsEnd) {
            health = 0;
            IsEnd = true; 
            Invoke("EndGame", 5); 
        }
    }

    void Update() {
        int playerDead = 0;
        PlayerController[] PlayerList = FindObjectsOfType<PlayerController>();
        foreach(PlayerController players in PlayerList){
            if(players.CurrentState == PlayerState.Dead) playerDead++;
        }

        if(!IsEnd && playerDead == PlayerList.Length) {
            IsEnd = true; 
            Invoke("EndGame", 5);
        }
    }


    void EndGame() {
        PhotonNetwork.LoadLevel("Game Over");
    }
}
