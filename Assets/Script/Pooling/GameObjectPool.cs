using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameObjectPool", menuName = "GameObjectPool")]
public class GameObjectPool : ScriptableObject 
{
    public GameObject Prefabs;

    private List<PooledObject> objectsInPool = new List<PooledObject>();
    private List<PooledObject> objectsInUse = new List<PooledObject>();

    public GameObject SpawnObject(Vector3 position, Quaternion rotaion, int Damage) {
    
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
        currentObject.gameObject.GetComponent<ParticleHitEnemy>().SetDamage(Damage);

        currentObject.gameObject.transform.position = position;
        currentObject.gameObject.transform.rotation = rotaion;

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
