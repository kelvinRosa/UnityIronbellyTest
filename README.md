# Ironbelly test.

![alt running example](https://i.ibb.co/CvHZyr6/Captura-de-tela-2024-05-23-125114.png)
![alt running example game](https://i.ibb.co/fMV81T0/Captura-de-tela-2024-05-23-125133.png)

### Pooling System
Object Pooling is a common design pattern. I have experience using this pattern in almost all projects that have items that constantly need to be spawned and destroyed. It's a simple system, so I created a small  **ObjectPool** class that uses the Unity interface **IObjectPool**. The **ObjectPool** class has functions to create a pool of objects using a given key, get items from the pool, and release items back to the pool.

### Find Nearest Neighbour
This is something easy to do, but considering that every object spawned from the pool will be able to draw a line to the nearest object, it needs to be performant. I took some time to think about algorithms like octree and k-d tree, but I decided to implement a simple and performant solution without using these structures. While developing, I considered using DOTS and the Jobs system, but given the test description, I thought the best approach was to implement it exactly as stated, using a NearestNeighbour script attached to the prefabs. Using DOTS would make the implementation a bit more complex, so I decided to keep it simple for this test. However, using DOTS could greatly improve performance. One thing to mention is that Unity has a Vector3.Distance function to get the distance between two vectors, but for performance reasons, I decided to use Vector3.sqrMagnitude to avoid the expensive calculations.

### Draw Line Between Objects
Drawing lines for debugging is really simple using Unity's Debug.DrawLine function, but I decided to use a LineRenderer to be able to see the lines in both the build and play scenes.

### Random Movement
This was a really simple system where I can define the maximum range that the object can spawn and move. I get a random position within a defined area to move the object, and when the object reaches its destination, I get another position to move to.

### Possible Improvements
Using the DOTS and Jobs system here would achieve the maximum performance possible in C# and Unity. If you guys want to see the results, I can create a version using DOTS.