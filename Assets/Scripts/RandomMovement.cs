using UnityEngine;

/// <summary>
/// A simple class that moves the object to a random position within a range.
/// </summary>
public class RandomMovement : MonoBehaviour {
    public float xRange = 10f;
    public float yRange = 10f;
    public float zRange = 10f;
    public float speed = 1f;

    private Vector3 targetPosition;

    void Start() {
        SetNewTargetPosition();
    }

    void Update() {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            SetNewTargetPosition();
        }
    }

    private void SetNewTargetPosition() {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, yRange);
        float z = Random.Range(-zRange, zRange);
        targetPosition = new Vector3(x, y, z);
    }
}