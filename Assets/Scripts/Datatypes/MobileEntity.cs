using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialPartitioning
{
    public class MobileEntity : MonoBehaviour, IMobileEntity
    {
        private Bounds AABB;
        private Vector3 vector;

        public MobileEntity(Bounds bounds)
        {
            AABB = bounds;
        }

        public Bounds GetBounds()
        {
            return AABB;
        }
    }
}
