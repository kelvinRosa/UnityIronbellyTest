using UnityEngine;

/// <summary>
/// A component that registers itself with the NearestNeighborManager and updates its position.
/// </summary>
public class NearesNeighborComponent : MonoBehaviour
{
    /// <summary>
    /// The index of this object in the NearestNeighborManager.
    /// </summary>
    private int index = -1; // -1 means it's not registered
    
    void Start()
    {
        index = NearestNeighborManager.Instance.AddPosition(transform.position);
    }

    public void OnEnable()
    {
        // means the object was already registered and removed and we are ok to add it again.
        if(index != -1)
            index = NearestNeighborManager.Instance.AddPosition(transform.position);
    }

    void FixedUpdate()
    {
        NearestNeighborManager.Instance.UpdatePosition(index, transform.position);
    }

    void OnDisable()
    {
        NearestNeighborManager.Instance.RemovePosition(index);
    }
}