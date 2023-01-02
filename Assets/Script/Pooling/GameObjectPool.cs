using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GameObjectPool", menuName = "GameObjectPool")]
public class GameObjectPool : ScriptableObject 
{
    public GameObject Prefabs;

    private List<PooledObject> objectsInPool = new List<PooledObject>();
    private List<PooledObject> objectsInUse = new List<PooledObject>();

    //Spawn particle for range attack
    public GameObject SpawnObject(Vector3 position, Quaternion rotaion, int Damage, LayerMask layer) {
    
        PooledObject currentObject;

        //Check if has enought object will instantiate new
        if(objectsInPool.Count <= 0 ) {
            
            GameObject newGameObject = Instantiate(Prefabs);
            currentObject = newGameObject.AddComponent<PooledObject>();
            currentObject.pool = this;
        }
        else {
            currentObject = objectsInPool[0];
            objectsInPool.Remove(currentObject);
        }

        objectsInUse.Add(currentObject);
        currentObject.gameObject.SetActive(true);
        currentObject.gameObject.GetComponent<ParticleHit>().SetDamage(Damage, layer);

        currentObject.gameObject.transform.position = position;
        currentObject.gameObject.transform.rotation = rotaion;

        return currentObject.gameObject;
    }

    //Spawn particle for melee attack
    public GameObject SpawnObject(Vector3 position, Quaternion rotaion) {
    
        PooledObject currentObject;

        //Check if has enought object will instantiate new
        if(objectsInPool.Count <= 0 ) {
            
            GameObject newGameObject = Instantiate(Prefabs);
            currentObject = newGameObject.AddComponent<PooledObject>();
            currentObject.pool = this;
        }
        else {
            currentObject = objectsInPool[0];
            objectsInPool.Remove(currentObject);
        }

        objectsInUse.Add(currentObject);
        currentObject.gameObject.SetActive(true);
        
        currentObject.gameObject.transform.position = position;
        currentObject.gameObject.transform.rotation = rotaion;

        return currentObject.gameObject;
    }
    
    //Spawn particle for melee attack
    public GameObject SpawnEnemy(Vector3 position) {
    
        PooledObject currentObject;

        //Check if has enought object will instantiate new
        if(objectsInPool.Count <= 0 ) {
            
            GameObject newGameObject = PhotonNetwork.Instantiate(Prefabs.name, Vector3.zero, Quaternion.identity);
            currentObject = newGameObject.AddComponent<PooledObject>();
            currentObject.pool = this;
        }
        else {
            currentObject = objectsInPool[0];
            currentObject.gameObject.GetComponent<PhotonView>().RPC("ReBorn", RpcTarget.AllBuffered);
            objectsInPool.Remove(currentObject);
        }
        objectsInUse.Add(currentObject);
        NavMeshHit closestHit;
        if(NavMesh.SamplePosition(position, out closestHit, 500f, NavMesh.AllAreas))
            currentObject.gameObject.transform.position = closestHit.position;

        return currentObject.gameObject;
    }

    //Re-use Object 
    public void ReturnToPool(PooledObject objectPool) {
        if(objectsInPool.Contains(objectPool)) return;

        objectPool.gameObject.SetActive(false);
        objectsInUse.Remove(objectPool);
        objectsInPool.Add(objectPool);
    }

    //Destroy Object form scene
    public void RemoveObject(PooledObject objectPool) {
        objectsInPool.Remove(objectPool);
        objectsInUse.Remove(objectPool);
    }
}
