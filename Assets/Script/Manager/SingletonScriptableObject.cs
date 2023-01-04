using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance(string folder) {        
        if(_instance == null) {
            T[] result = Resources.LoadAll<T>(folder);
            if(result.Length == 0) {
                Debug.LogError("Don't find ScriptableObject "+ typeof(T)+" in Resource.");
                return null;
            }
            if(result.Length > 1) {
                Debug.LogError("Have more 1 ScriptableObject "+ typeof(T)+" in Resource.");
                return null;
            }
            _instance = result[0];
        }
        return  _instance;
    }
}
