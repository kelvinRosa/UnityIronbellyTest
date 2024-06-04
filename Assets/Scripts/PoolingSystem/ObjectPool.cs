using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic object pooling class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Dictionary<string, Queue<T>> _pools = new Dictionary<string, Queue<T>>();
    private readonly Dictionary<string, PoolInfo> _poolInfo = new Dictionary<string, PoolInfo>();

    private class PoolInfo
    {
        public T Prefab { get; }
        public int MaxSize { get; }

        public PoolInfo(T prefab, int maxSize)
        {
            Prefab = prefab;
            MaxSize = maxSize;
        }
    }

    /// <summary>
    /// Create a new Pool using the defined pool key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="prefab"></param>
    /// <param name="initialSize"></param>
    /// <param name="maxSize"></param>
    public void CreatePool(string key, T prefab, int initialSize, int maxSize)
    {
        if (!_pools.ContainsKey(key))
        {
            Queue<T> pool = new Queue<T>();

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }

            _pools[key] = pool;
            _poolInfo[key] = new PoolInfo(prefab, maxSize);
        }
    }

    /// <summary>
    /// Get a pooled object from a created pool if it exists, otherwise create a new one.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetFromPool(string key)
    {
        if (_pools.ContainsKey(key))
        {
            if (_pools[key].Count > 0)
            {
                T obj = _pools[key].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                PoolInfo poolInfo = _poolInfo[key];
                if (_pools[key].Count < poolInfo.MaxSize)
                {
                    T obj = Object.Instantiate(poolInfo.Prefab);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
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
            obj.gameObject.SetActive(false);
            _pools[key].Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj.gameObject);
        }
    }
}