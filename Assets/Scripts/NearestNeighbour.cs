using UnityEngine;

/// <summary>
/// Class that finds the nearest neighbour in the scene.
/// </summary>
public class NearestNeighbour : MonoBehaviour
{
    private float updateInterval = 0.4f;
    private float lastUpdateTime = 0f;
    private NearestNeighbour nearest = null;
    [SerializeField]
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    private void FixedUpdate()
    {
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            FindNearestNeighbour();
            lastUpdateTime = Time.time;
        }
        DrawLine();
    }

    /// <summary>
    /// Draw a line renderer between the current object and the nearest neighbour.
    /// </summary>
    private void DrawLine()
    {
        if (nearest != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nearest.transform.position);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Find the nearest neighbour in the active objects list.
    /// </summary>
    private void FindNearestNeighbour()
    {
        float nearestDistance = float.MaxValue;
        float thresholdDistance = 10.0f; // Adjust threshold as needed
        Vector3 currentPosition = transform.position;

        foreach (var neighbour in PoolingManager.Instance.activePrefabs)
        {
            if (neighbour != this)
            {
                float distance = Vector3.SqrMagnitude(currentPosition - neighbour.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = neighbour;

                    // Early termination if distance is below threshold
                    if (distance < thresholdDistance)
                    {
                        break;
                    }
                }
            }
        }
    }
}