using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{
	// KdTrees are for immobile point objects. Great for finding the nearest restaurant, terrible for tracking mobile bounding volumes
	public sealed class KdTree 
	{
		private int dimensions;
		
		public KdTree(int dimensions)
		{
            this.dimensions = dimensions;
			
			
		}
		
		public void Insert(object obj)
		{
			
		}
	}
	
}