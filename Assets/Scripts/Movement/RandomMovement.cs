using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private MovementSettingsSO movementSettings;
    private Vector3 targetPosition;

    void Start()
    {
        RandomizeTargetPosition();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSettings.cubesMovementSpeed); // Adjust speed as necessary

        if (transform.position == targetPosition)
        {
            RandomizeTargetPosition();
        }
    }

    void RandomizeTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(-movementSettings.cubesMovementArea.x, movementSettings.cubesMovementArea.x), Random.Range(-movementSettings.cubesMovementArea.y, movementSettings.cubesMovementArea.y), Random.Range(-movementSettings.cubesMovementArea.z, movementSettings.cubesMovementArea.z));
    }
}