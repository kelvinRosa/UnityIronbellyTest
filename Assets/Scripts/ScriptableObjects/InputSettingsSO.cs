using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Ironbelly/InputSettings")]
public class InputSettingsSO : ScriptableObject
{
    [Header("Input Settings")]
    public KeyCode spawnKey;
    public KeyCode despawnKey;
}
