# Unity-Spatial-partitioning

General proof of concept for thesis work. A collection of data structures for spatial partioning written in C# for the Unity Engine.

Each structure is designed to store lists of GameObjects, bounding volumes are checked from the component Collider.Bounds. As the general use case for these structures are broad-phase collision detection, axis-aligned bounding boxes (AABB) are sufficient, more accurate bounding volumes can be used in narrow-phase collision detection instead.

>> Quadtree:
Supports 2D only (the corresponding 3D would be an Octree).

Instantiation example:
Quadtree foo = new Quadtree(bounds); // where bounds is an Bounds object which describes the area covered by the Quadtree.

>> Spatial Hash:
Supports both 2D and 3D (for a 2D use case, set one axis value to be the same for all objects, e.g. all z values = 0).

Instantiation examples:
Spatialhash foo = new SpatialHash(bucketSize); // where bucketSize is an int, the bucket dimensions will be cubic.

Spatialhash foo = new SpatialHash(bucketSize); // where bucketSize is a Vector3, and the x/y/z values define the bucket dimension length.

>> TODO list:
- Currently untested. Work is currently on hiatus while the thesis is being written, these files are here for demo purposes.
