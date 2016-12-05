using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpatialPartitioning.BoundingVolumes;

namespace SpatialPartitioning
{
    public class CollidableEntity : MonoBehaviour
    {
        protected Bounds aabb;
        protected Vector3 vector;

        public CollidableEntity(Bounds bounds)
        {
            aabb = bounds;
        }

        public Bounds GetAABB()
        {
            return aabb;
        }
        
        public BaseBoundingVolume GetBoundingVolume()
        {
            return new BaseBoundingVolume();
        }
    }
}
