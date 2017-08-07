using UnityEngine;

namespace SpatialPartitioning.BoundingVolumes
{
    class Item : ICollidableEntity
    {
        private Bounds bounds;
        

        public Item(Bounds bounds)
        {
            this.bounds = bounds;
        }

        public Bounds GetBoundingBox()
        {
            return bounds;
        }
    }
}
