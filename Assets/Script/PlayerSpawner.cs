using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{   
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private CharacterClass[] characters;
    [SerializeField] private PhotonView playerPrefabs;

    private List<PhotonView> _list = new List<PhotonView>();

    void Awake() {
        int index = _list.FindIndex(x => x.gameObject.GetComponent<PlayerController>().Name == PhotonNetwork.LocalPlayer.NickName);
        if (index == -1) {
            int randomNumber = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomNumber];
            PhotonView playerToSpawn= playerPrefabs;
            playerToSpawn.gameObject.GetComponent<PlayerController>().SetCharacterClass(characters[(int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterClass"]]);
            playerToSpawn.gameObject.GetComponent<PlayerController>().SetName(PhotonNetwork.LocalPlayer.NickName);
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
            _list.Add(playerToSpawn);
        }
    }
}
