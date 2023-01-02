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
    void Update()
    {   
        currentNumberEnmey = FindObjectsOfType<EnemyAI>().Length;

        if(FindObjectsOfType<PlayerController>().Length == PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient) {
            if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Level") && currentNumberEnmey  <= 10) {
                enemy[Random.Range(0,enemy.Length)].SpawnEnemy(point[Random.Range(0,point.Length)].position);
                Debug.Log((int) PhotonNetwork.CurrentRoom.CustomProperties["Level"]);
            }
        }
    }


}
