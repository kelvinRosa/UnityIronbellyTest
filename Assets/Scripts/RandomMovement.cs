using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float xRange = 10f;
    public float yRange = 10f;
    public float zRange = 10f;
    public float speed = 1f;

    private Vector3 targetPosition;
    private float sqrMinDistance;

    void Start()
    {
        SetNewTargetPosition();
        sqrMinDistance = 0.01f; // Square of minimum distance to reduce repeated calculations
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        // Calculate direction and distance
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        // Move only if the distance is significant
        if (distance > sqrMinDistance)
        {
            // Normalize direction
            direction /= distance;

            // Move towards the target with speed
            transform.position += direction * (speed * Time.deltaTime);

            // Check if close to target
            if (distance < 0.1f)
            {
                SetNewTargetPosition();
            }
        }
        else
        {
            // If distance is too small, consider it reached and set a new target position
            SetNewTargetPosition();
        }
    }

    private void SetNewTargetPosition()
    {
        // Generate random position within the specified range
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, yRange);
        float z = Random.Range(-zRange, zRange);
        targetPosition = new Vector3(x, y, z);
    }
}