using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{

    public sealed class Quadtree
    {
		// Members
        private readonly QuadtreeNode root;
        public readonly int maxItems;
        public readonly int maxLevels;

        public int NodeCount
		{
			get 
			{ 
				return root.CountNodes();
			}
		}
		
		// Constants
        private const int DefaultMaxItems = 10;    // change this to allow more items in the parent node before assigning child nodes
        private const int DefaultMaxLevels = 5;    // change this to control the Quadtree depth. Deeper = more granularity, but consider the expected size of objects compared to the cell sizes!
		
        // Constructor
        public Quadtree(Bounds bounds, int maxItems = DefaultMaxItems, int maxLevels = DefaultMaxLevels)
        {
            root = new QuadtreeNode(bounds, 1);
			QuadtreeNode.MaxItems = this.maxItems = maxItems;
            QuadtreeNode.MaxLevels = this.maxLevels = maxLevels;
        }

        // get the smallest node which wholly encloses the given target area. Out of bounds = returns null
        public QuadtreeNode GetEnclosingNode(Bounds target)
        {
			return root.IsOutOfBounds(target) ? null : root.GetNode(target); // TODO: will this compile?
			
			/*
            if(root.IsOutOfBounds(target))
            {
                return null;
            }
            else
            {
                return root.GetNode(target);
            }
			*/
        }
		
		//public List<QuadtreeNode> GetOverlappingNodes(Bounds target)
		//{
		//	List<QuadtreeNode> nodes = new List<QuadtreeNode>();
			
		//	// recursive down to lowest level
		//	if(root.IsOverlapping(target))
		//	{
		//		nodes.Add(root);
		//	}
			
		//	if(!root.IsLeaf())
		//	{
		//		for(int i = 0; i < children.Length; i++)
		//		{
		//			nodes.AddRange(children[i].GetOverlappingNodes(target));
		//		}
		//	}
		//	return nodes;
		//}

        public void Insert(GameObject item)
        {
            root.Insert(item);
        }

        public bool Remove(GameObject item)
        {
            return root.Remove(item);
        }
		
        public List<GameObject> GetItemsByArea(Bounds target)
        {
			return root.GetItemsByArea(target);
        }
		
		public void TraverseBreadthFirst(Func<int> delegateMethod)
		{
			Queue<QuadtreeNode> q = new Queue<QuadtreeNode>();
			QuadtreeNode nodePointer = root;
			
			q.Enqueue(root);

            // TODO: queue up the node then do stuff to it
		}
		
        public override string ToString()
        {
            return root.ToString();
        }

        public void DebugDisplayNodes()
        {
            root.TraverseDepthFirstPreOrder(root.DebugDisplayNodes);
        }
    }
}
