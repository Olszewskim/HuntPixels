using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PooledObject : MonoBehaviour {
    public ObjectPool Pool { get; set; }

    [NonSerialized] private ObjectPool poolInstanceForPrefab;

    protected virtual void Start() {
        SceneManager.sceneLoaded += (x, y) => ReturnToPool();
    }

    public T GetPooledInstance<T>() where T : PooledObject {
        if (!poolInstanceForPrefab) {
            poolInstanceForPrefab = ObjectPool.GetPool(this);
        }

        return (T) poolInstanceForPrefab.GetObject();
    }

    public void ReturnToPool() {
        if (Pool) {
            Pool.AddObject(this);
        } else {
            Destroy(gameObject);
        }
    }

    public virtual void ResetObject() {
        gameObject.SetActive(true);
    }
}
