using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Ironbelly/MovementSettings")]
public class MovementSettingsSO : ScriptableObject
{
    [Header("Movement Settings")]
    public float cubesMovementSpeed;
    public Vector3 cubesMovementArea;
}
