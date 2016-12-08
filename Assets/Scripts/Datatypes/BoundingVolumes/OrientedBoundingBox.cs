using UnityEngine;

namespace SpatialPartitioning.BoundingVolumes
{
    public struct OrientedBoundingBox
    {
        public Bounds AABB;
        public float AngleRadians;
    }
}
