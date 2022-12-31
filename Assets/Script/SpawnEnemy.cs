using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObjectPool[] enemy;
    [SerializeField] private Transform[] point;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient) {
            enemy[Random.Range(0,enemy.Length-1)].SpawnEnemy(point[Random.Range(0,point.Length-1)].position);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
}
