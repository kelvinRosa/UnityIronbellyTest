using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A generic object pooling class using unity IObjectPool interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : MonoBehaviour
{
    private Dictionary<string, IObjectPool<T>> _pools = new Dictionary<string, IObjectPool<T>>();

    /// <summary>
    /// Create a new Pool using the defined pool key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="prefab"></param>
    /// <param name="initialSize"></param>
    /// <param name="maxSize"></param>
    public void CreatePool(string key, T prefab, int initialSize, int maxSize)
    {
        if (!_pools.ContainsKey(key))
        {
            IObjectPool<T> pool = new UnityEngine.Pool.ObjectPool<T>(
                createFunc: () => Object.Instantiate(prefab),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                actionOnDestroy: (obj) => Object.Destroy(obj.gameObject),
                collectionCheck: false,
                defaultCapacity: initialSize,
                maxSize: maxSize
            );

            _pools[key] = pool;
        }
    }

    /// <summary>
    /// Get a pooled object from a created pool if it exists, otherwise return null.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetFromPool(string key)
    {
        if (_pools.ContainsKey(key))
        {
            return _pools[key].Get();
        }
        return null;
    }

    /// <summary>
    /// Return objects to the pool if it exists, otherwise destroy the object.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void ReturnToPool(string key, T obj)
    {
        if (_pools.ContainsKey(key))
        {
            _pools[key].Release(obj);
        }
        else
        {
            Object.Destroy(obj.gameObject);
        }
    }
}