using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{
    public partial class Quadtree
    {
        // Members
        private readonly QuadtreeNode root;
        public readonly int maxItems;
        public readonly int maxLevels;
        public readonly QuadtreeMode mode;

        public int NodeCount
        {
            get
            {
                return root.CountNodes();
            }
        }

        // Constructors
        /// <summary>
        /// Creates the Quadtree covering an area of size Bounds, with default values for max items per node and max tree depth.
        /// </summary>
        /// <param name="bounds">Axis-Aligned Bounding Box representation of the area covered by this Quadtree</param>
        public Quadtree(Bounds bounds) : this(bounds, ConfigSettings.DefaultQuadtreeMaxItemsPerNode, ConfigSettings.DefaultQuadtreeMaxTreeDepth, ConfigSettings.DefaultQuadtreeMode) { }

        /// <summary>
        /// Creates the Quadtree covering an area of size Bounds and user-defined values for max items per node and max tree depth.
        /// </summary>
        /// <param name="bounds">Axis-Aligned Bounding Box representation of the area covered by this Quadtre</param>
        /// <param name="maxItems">Maximum number of items per node. If this value is exceeded, the Quadtree attempts to create a new level and redistribute the items accordingly</param>
        /// <param name="maxLevels"></param>
        /// <param name="mode">Handles how items are delegated when child nodes are appended. Either nodes will only contain items that reside wholly inside the bounds, or overlaps are allowed (item references will be duplicated across all nodes they overlap)</param>
        public Quadtree(Bounds bounds, int maxItems, int maxLevels, QuadtreeMode mode)
        {
            root = new QuadtreeNode(this, bounds, 1);
        }

        // get the smallest node which wholly encloses the given target area. Out of bounds = returns null
        private QuadtreeNode GetEnclosingNode(Bounds target)
        {
            return root.IsOutOfBounds(target) ? null : root.GetNode(target);
        }

        public List<CollidableEntity> GetItems()
        {
            return new List<CollidableEntity>(); // TODO: method not finished
        }

        //public List<QuadtreeNode> GetOverlappingNodes(Bounds target)
        //{
        //    List<QuadtreeNode> nodes = new List<QuadtreeNode>();

        //    // recursive down to lowest level
        //    if (root.IsOverlapping(target))
        //    {
        //        nodes.Add(root);
        //    }

        //    if (!root.IsLeaf())
        //    {
        //        for (int i = 0; i < children.Length; i++)
        //        {
        //            nodes.AddRange(children[i].GetOverlappingNodes(target));
        //        }
        //    }
        //    return nodes;
        //}

        public void InsertItem(CollidableEntity item)
        {
            root.InsertItem(item);
        }

        public bool RemoveItem(CollidableEntity item)
        {
            return root.RemoveItem(item);
        }

        public List<CollidableEntity> GetItemsByArea(Bounds target)
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

        public void DebugDisplayNodes()
        {
            //root.TraverseDepthFirstPreOrder(root.DebugDisplayNodes);
        }

        public override string ToString()
        {
            return root.ToString();
        }
    }
}
