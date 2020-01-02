using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private PooledObject prefab;
    private List<PooledObject> availableObjects = new List<PooledObject>();

    public PooledObject GetObject() {
        PooledObject obj;
        int lastAvailableIndex = availableObjects.Count - 1;
        if (lastAvailableIndex >= 0) {
            obj = availableObjects[lastAvailableIndex];
            availableObjects.RemoveAt(lastAvailableIndex);
        } else {
            obj = Instantiate(prefab);
            obj.transform.SetParent(transform, false);
            obj.Pool = this;
        }
        obj.ResetObject();
        return obj;
    }

    public void AddObject(PooledObject obj) {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        if (!availableObjects.Contains(obj)) {
            availableObjects.Add(obj);
        }
    }

    public static ObjectPool GetPool(PooledObject prefab) {
        GameObject obj;
        ObjectPool pool;
        if (Application.isEditor) {
            obj = GameObject.Find(prefab.name + " Pool");
            if (obj) {
                pool = obj.GetComponent<ObjectPool>();
                if (pool) {
                    return pool;
                }
            }
        }
        obj = new GameObject(prefab.name + " Pool");
        DontDestroyOnLoad(obj);
        pool = obj.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        return pool;
    }
}
