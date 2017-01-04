using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace SpatialPartitioning
{
    public static class SpatialPartitioningUtils
    {
        public static Bounds RectToBounds(Rect input)
        {
            return new Bounds
            {
                center = new Vector3(),
                extents = new Vector3()
            };
        }


        public static Rect BoundsToRect(Bounds input)
        {
            return new Rect
            {
                position = new Vector2(),
                size = new Vector2()
            };
        }
    }
}
