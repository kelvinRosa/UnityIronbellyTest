using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class PoolingManager : MonoBehaviour
{
    [SerializeField]
    private PoolingSettingsSO poolingSettings;
    [SerializeField]
    private InputSettingsSO inputSettings;
    [SerializeField][Header("UI Elements")]
    private TMP_Text prefabCountText;
    [SerializeField]
    private TMP_InputField inputField;
    
    private ObjectPool<NearesNeighborComponent> pool;
    
    [HideInInspector]
    public List<NearesNeighborComponent> activePrefabs = new List<NearesNeighborComponent>();

    
    private void Start()
    {
        pool = new ObjectPool<NearesNeighborComponent>();
        pool.CreatePool("CubePool", poolingSettings.prefab.GetComponent<NearesNeighborComponent>(), poolingSettings.startPoolSize, 10000);
        UpdatePrefabCount();
        PreloadPool();
    }

    /// <summary>
    /// Preload the Pool with the initial pool size
    /// </summary>
    private void PreloadPool()
    {
        SpawnPrefabs(poolingSettings.startPoolSize);
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
            //prefab.AddToNearestNeighbourManager();
            activePrefabs.Add(prefab);
        }
        
        // m_points = new NativeArray<float3>(activePrefabs.Count, Allocator.Persistent);
        // // Create a container that accelerates querying for neighbours
        // m_container = new KnnContainer(m_points, false, Allocator.Persistent);
        
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
	        NearesNeighborComponent prefab = activePrefabs[activePrefabs.Count - 1];
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
        if (Input.GetKeyDown(inputSettings.spawnKey))
        {
            SpawnPrefabs();
        }

        if (Input.GetKeyDown(inputSettings.despawnKey))
        {
            DespawnPrefabs();
        }
    }
}