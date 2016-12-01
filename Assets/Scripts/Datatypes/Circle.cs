using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialPartitioning
{

    public class Circle
    {
        public Vector3 pos { get; set; }
        public float radius { get; set; }

        public Circle(Vector3 pos, float radius)
        {
            this.pos = pos;
            this.radius = radius;
        }

        public bool IsColliding(Circle other)
        {
            // negatives get cancelled, squaredDistance becomes a scalar value
            float distanceX = pos.x - other.pos.x,
                distanceY = pos.y - other.pos.y,
                squaredDistance = distanceX * distanceX + distanceY * distanceY,
                squaredTotalRadius = radius * radius + other.radius * other.radius;

            // if the distance between points is less than the combined radii then there has to be an overlap.
            return squaredDistance < squaredTotalRadius; // use squared distances to avoid having to calculate square root
        }
    }

}
