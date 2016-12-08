using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpatialPartitioning.BoundingVolumes;

namespace SpatialPartitioning
{
    public class CircularEntity : CollidableEntity
    {
        private Circle boundingVolume;

        protected override void Awake()
        {
            boundingVolume = new Circle();
            boundingVolume.Origin = gameObject.transform.position;
            //boundingVolume.Radius = gameObject.transform.

            aabb = CircleToAABB(boundingVolume);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected Bounds CircleToAABB(Circle boundingVolume)
        {
            return new Bounds(boundingVolume.Origin, new Vector3(boundingVolume.Radius, boundingVolume.Radius));
        }
    }
}
