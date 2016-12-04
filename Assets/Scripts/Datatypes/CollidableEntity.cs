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
        private Bounds AABB;
        private Vector3 vector;

        public CollidableEntity(Bounds bounds)
        {
            AABB = bounds;
        }

        public Bounds GetAABB()
        {
            return AABB;
        }
        
        public BaseBoundingVolume GetBoundingVolume()
        {
            return new BaseBoundingVolume();
        }
    }
}
