using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{   
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private CharacterClass[] characters;
    [SerializeField] private PhotonView playerPrefabs;

    void Start() {
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];
        PhotonNetwork.Instantiate(playerPrefabs.name, spawnPoint.position, Quaternion.identity);
    }
}
