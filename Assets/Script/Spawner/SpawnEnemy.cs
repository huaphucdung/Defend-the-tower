using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObjectPool[] enemy;
    [SerializeField] private Transform[] point;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    private int currentNumberEnmey;
    // Update is called once per frame
    void Start()
    {   
        InvokeRepeating("SpawnEnemyWare", 1f, 2f);
    }

    void SpawnEnemyWare() {
        if(FindObjectsOfType<PlayerController>().Length == 0)
            return;
        currentNumberEnmey = FindObjectsOfType<EnemyAI>().Length;
        if(!PhotonNetwork.IsConnected)
            return;
        if(FindObjectsOfType<PlayerController>().Length == PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient) {
            if(currentNumberEnmey  <= 15) {
                enemy[Random.Range(0,enemy.Length)].SpawnEnemy(point[Random.Range(0,point.Length)].position);
            }
        }
    }
}
