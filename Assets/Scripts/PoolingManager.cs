using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;
    
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private int initialPoolSize = 10;
    [SerializeField]
    private TMP_Text prefabCountText;
    [SerializeField]
    private TMP_InputField inputField;
    public KeyCode spawnKey;
    public KeyCode despawnKey;
    
    private ObjectPool<NearestNeighbour> pool;
    [HideInInspector]
    public List<NearestNeighbour> activePrefabs = new List<NearestNeighbour>();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        pool = new ObjectPool<NearestNeighbour>();
        pool.CreatePool("CubePool", cubePrefab.GetComponent<NearestNeighbour>(), initialPoolSize, 100);
        UpdatePrefabCount();
        PreloadPool();
    }

    /// <summary>
    /// Preload the Pool with the initial pool size
    /// </summary>
    private void PreloadPool()
    {
        SpawnPrefabs(initialPoolSize);
    }

    /// <summary>
    /// Spawn prefabs based on the amount
    /// </summary>
    /// <param name="count">amount of prefabs to spawn</param>
    public void SpawnPrefabs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var prefab = pool.GetFromPool("CubePool");
            prefab.transform.parent = transform;
            prefab.transform.position = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );
            activePrefabs.Add(prefab);
        }
        UpdatePrefabCount();
    }
    
    /// <summary>
    /// Spawn prefabs based on the input field value
    /// </summary>
    public void SpawnPrefabs()
    {
        if (int.TryParse(inputField.text, out var count))
        {
            SpawnPrefabs(count);
        }
        else
        {
            Debug.LogError("Invalid input on input field");
        }
        
    }
    
    /// <summary>
    /// Despawn prefabs based on the amount
    /// </summary>
    /// <param name="count">amount of prefabs do despawn</param>
    public void DespawnPrefabs(int count)
    {
        for (int i = 0; i < count && activePrefabs.Count > 0; i++)
        {
            NearestNeighbour prefab = activePrefabs[activePrefabs.Count - 1];
            activePrefabs.RemoveAt(activePrefabs.Count - 1);
            pool.ReturnToPool("CubePool", prefab);
        }
        UpdatePrefabCount();
    }
    
    /// <summary>
    /// Despawn prefabs based on the input field value
    /// </summary>
    public void DespawnPrefabs()
    {
        if (int.TryParse(inputField.text, out var count))
        {
            DespawnPrefabs(count);
        }
        else
        {
            Debug.LogError("Invalid input on input field");
        }
    }

    /// <summary>
    /// Update the UI text with the active prefab count
    /// </summary>
    private void UpdatePrefabCount()
    {
        prefabCountText.text = activePrefabs.Count.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnPrefabs();
        }

        if (Input.GetKeyDown(despawnKey))
        {
            DespawnPrefabs();
        }
    }
}