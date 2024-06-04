using UnityEngine;

[CreateAssetMenu(fileName = "PoolingSettings", menuName = "Ironbelly/PoolingSettings")]
public class PoolingSettingsSO : ScriptableObject
{
    [Header("Pool Settings")]
    public int startPoolSize;
    public GameObject prefab;
}
