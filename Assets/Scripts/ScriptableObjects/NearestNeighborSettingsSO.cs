using UnityEngine;

[CreateAssetMenu(fileName = "NearestNeighborSettings", menuName = "Ironbelly/NearestNeighborSettings")]
public class NearestNeighborSettingsSO : ScriptableObject
{
    [Header("Nearest NeighborSystem Settings")]
    public float updateInterval;
}
