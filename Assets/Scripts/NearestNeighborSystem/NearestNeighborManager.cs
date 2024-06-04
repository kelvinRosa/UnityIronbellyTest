using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;

/// <summary>
/// A manager that keeps track of the positions of all objects that register with it and finds the closest neighbor for each object.
/// </summary>
public class NearestNeighborManager : MonoBehaviour
{
    [SerializeField]
    private NearestNeighborSettingsSO nearestNeighborSettings;
    [SerializeField]
    private PoolingSettingsSO poolingSettings;

    /// <summary>
    /// The positions of all objects that have registered with the manager.
    /// </summary>
    private NativeArray<float3> positions;
    
    /// <summary>
    /// The index of the closest neighbor for each object.
    /// </summary>
    private NativeArray<int> closestNeighbors;

    /// <summary>
    /// The current number of objects that have registered with the manager.
    /// </summary>
    private int currentSize = 0;
    
    /// <summary>
    /// Timer to update the closest neighbors.
    /// </summary>
    private float timer = 0f;

    public static NearestNeighborManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        // Initialize the arrays with the start pool size
        positions = new NativeArray<float3>(poolingSettings.startPoolSize, Allocator.Persistent);
        closestNeighbors = new NativeArray<int>(poolingSettings.startPoolSize, Allocator.Persistent);
    }

    void OnDestroy()
    {
        // Free the memory
        if (positions.IsCreated) positions.Dispose();
        if (closestNeighbors.IsCreated) closestNeighbors.Dispose();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nearestNeighborSettings.updateInterval)
        {
            timer = 0f;
            UpdateClosestNeighbors();
        }
    }

    /// <summary>
    /// Updates the closest neighbors for all objects.
    /// </summary>
    void UpdateClosestNeighbors()
    {
        if (currentSize > 1)
        {
            NativeArray<int> newClosestNeighbors = new NativeArray<int>(currentSize, Allocator.TempJob);

            FindClosestNeighborJob job = new FindClosestNeighborJob
            {
                Positions = positions,
                ClosestNeighbors = newClosestNeighbors,
                CurrentSize = currentSize
            };

            JobHandle jobHandle = job.Schedule(currentSize, 64);
            jobHandle.Complete();
            // Copy the results from newClosestNeighbors to closestNeighbors
            NativeArray<int>.Copy(newClosestNeighbors, closestNeighbors);
            
            // Free Temporary memory
            newClosestNeighbors.Dispose();
        }
    }

    /// <summary>
    /// Adds a position to the manager and returns the index of the position.
    /// </summary>
    /// <param name="position"> The float3 position of the object</param>
    /// <returns></returns>
    public int AddPosition(float3 position)
    {
        if (currentSize >= positions.Length)
        {
            // Resize the arrays
            ResizeNativeArray(ref positions, currentSize+1);
            ResizeNativeArray(ref closestNeighbors, currentSize+1);
        }

        positions[currentSize] = position;
        return currentSize++;
    }

    /// <summary>
    /// Updates the position of an object in the manager.
    /// </summary>
    /// <param name="index"> The index that needs to be updated</param>
    /// <param name="position"> The new Position</param>
    public void UpdatePosition(int index, float3 position)
    {
        if (index < 0 || index >= currentSize) return;
        positions[index] = position;
    }

    /// <summary>
    /// Removes a position from the manager.
    /// </summary>
    /// <param name="index"> The index that needs to be removed </param>
    public void RemovePosition(int index)
    {
        if (index < 0 || index >= currentSize) return;
        positions[index] = positions[currentSize - 1];
        currentSize--;

        ResizeNativeArray(ref positions, currentSize);
        ResizeNativeArray(ref closestNeighbors, currentSize);
    }

    /// <summary>
    /// Resizes a NativeArray to a new size.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="newSize"></param>
    /// <typeparam name="T"></typeparam>
    void ResizeNativeArray<T>(ref NativeArray<T> array, int newSize) where T : struct
    {
        NativeArray<T> newArray = new NativeArray<T>(newSize, Allocator.Persistent);
        NativeArray<T>.Copy(array, newArray, math.min(array.Length, newSize));
        array.Dispose();
        array = newArray;
    }

    /// <summary>
    /// A job that finds the closest neighbor for each object.
    /// </summary>
    [BurstCompile]
    public struct FindClosestNeighborJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<int> ClosestNeighbors;
        public int CurrentSize;

        public void Execute(int index)
        {
            float3 currentPosition = Positions[index];
            float minDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < CurrentSize; i++)
            {
                if (i == index) continue;

                float distance = math.distance(currentPosition, Positions[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }
            }

            ClosestNeighbors[index] = closestIndex;
        }
    }

    /// <summary>
    /// Draw the line between objects during OnDrawGizmos.
    /// </summary>
    void OnDrawGizmos()
    {
        if (closestNeighbors.IsCreated && positions.IsCreated)
        {
            for (int i = 0; i < currentSize; i++)
            {
                int closestNeighborIndex = closestNeighbors[i];
                if (closestNeighborIndex != -1)
                {
                    if(positions.Length > i  && positions.Length > closestNeighborIndex)
                        Debug.DrawLine(positions[i], positions[closestNeighborIndex], Color.red);
                }
            }
        }
    }
}