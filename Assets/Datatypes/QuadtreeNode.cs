using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning.Quadtree
{
	
	public class QuadtreeNode
    {
        // Members
		protected readonly string id;
		protected readonly Bounds bounds;
        protected readonly int level;
		protected readonly QuadtreeNode parent;
		
		protected List<GameObject> items;
        protected QuadtreeNode[] children;   // recursive, each node may have child nodes
		
		// Constants
		private const int NODE_COUNT = 4;	// Quadtrees have 4 child nodes. Changing this would change the tree type
		
		// change this to allow more items in the parent node before assigning child nodes
        public static int MaxItems { get; set; }

        // change this to control the Quadtree depth. Deeper = more granularity, but consider the expected size of objects compared to the cell sizes!
        public static int MaxLevels { get; set; }
		
		
        // Constructors
        public QuadtreeNode(Bounds bounds, int level, string id = "", QuadtreeNode parent = null)
        {
			items = new List<GameObject>();
			this.bounds = bounds;
			this.level = level;
			this.parent = parent;
			
			children = new QuadtreeNode[QuadtreeNode.NODE_COUNT];
			
			this.id = this.parent != null ? this.parent.id + "_" + id : "root";
        }

        #region Items
        public void Insert(GameObject item)
        {
            if (!IsLeaf())
            {
                int quadrant = MatchQuadrant(item.GetComponent<Collider>().bounds);
                
                if (quadrant > -1)
                {
                    children[quadrant].Insert(item);
                    return;
                }
            }
			
			items.Add(item);

            if (items.Count > QuadtreeNode.MaxItems && level < QuadtreeNode.MaxLevels)
            {
                Split();
            }
        }

        public bool Remove(GameObject item)
        {
            bool returnValue = items.Remove(item);
			
			if(!IsLeaf() && AreChildrenEmpty())
			{
				RemoveChildren(); // all descendants empty, delete
			}
            return returnValue;
        }
		
		public void Clear()
		{
			items.Clear();
		}
		
		public bool Contains(GameObject item)
		{
			return items.Contains(item);
		}
		
		public bool AreChildrenEmpty()
		{
			return items.Count == GetCount(); // if current node item count == current node + all descendants' item counts, then descendants are empty
		}
		
		public int GetCount()
		{
			int count = items.Count;
			
			if(!IsLeaf())
			{
				for(int i = 0; i < children.Length; i++)
				{
					count += children[i].GetCount();
				}
			}
			
			return count;
		}

        // basically, give a target AABB and this method returns all objects in nodes which overlap the AABB.
        public List<GameObject> GetItemsByArea(Bounds target) // the list return by the initial call contains objects from all nodes which overlap the given target area
        {
            List<GameObject> collisionCandidates = new List<GameObject>();
            // use IsOverlapping and going deeper into children
            if (!IsLeaf())
            {
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].IsOverlapping(target))
                    {
                        collisionCandidates.AddRange(children[i].GetItemsByArea(target)); // in-order traversal: add the items which overlap the target bounds
                    }

                }
            }

            if (!IsOutOfBounds(target))
            {
                collisionCandidates.AddRange(items);
                return collisionCandidates;
            }
            else
            {
                return new List<GameObject>();
            }
        }

        public void ClearItems()
        {
            items.Clear();
        }
        #endregion

        public QuadtreeNode GetNode(Bounds target)
        {
            if(!IsLeaf())
            {
                for(int i = 0; i < children.Length; i++)
                {
                    if(children[i].IsEnclosing(target))
                    {
                        return children[i].GetNode(target);
                    }
                }
            }
            return this;
        }

        

        public bool IsLeaf()
        {
            return children[0] == null; // all child nodes are instantiated or removed together, so to test if this is a leaf, just checking if the first child is null is sufficient
        }

        public void RemoveChildren()
        {
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = null;
            }
        }

        public int MatchQuadrant(Bounds target)
        {
            int index = -1;
            if (IsLeaf())
            {
                bool north = target.center.y - target.extents.y < bounds.center.y;
                bool south = target.center.y + target.extents.y > bounds.center.y;
                bool west = target.center.x - target.extents.x < bounds.center.x;
                bool east = target.center.x + target.extents.x > bounds.center.x;

                if (north)
                {
                    // Object completely fits within the north quadrants 	
                    if (west)
                    {
                        index = (int)Quadrants.Northwest; // Northwest
                    }
                    else if (east)
                    {
                        index = (int)Quadrants.Northeast; // Northeast
                    }
                }
                else if (south)
                {
                    // Object completely fits within the south quadrants 	
                    if (west)
                    {
                        index = (int)Quadrants.Southwest; // Southwest
                    }
                    else if (east)
                    {
                        index = (int)Quadrants.Southeast; // Southeast
                    }
                }
                // objects which do not fit entirely inside a quadrant will fall through and return a -1
            }
            return index;
        }

        #region Bounds checking

        public bool IsEnclosing(Bounds target) // return true if the target is entirely within this.bounds
        {
            return !(target.center.x - target.extents.x < bounds.center.x - bounds.extents.x ||
                target.center.x + target.extents.x < bounds.center.x + bounds.extents.x ||
                target.center.y - target.extents.y < bounds.center.y - bounds.extents.y ||
                target.center.y + target.extents.y < bounds.center.y + bounds.extents.y);
        }
		
		public bool IsOverlapping(Bounds target) // return true if any part of the target is inside this.bounds
		{
			// TODO: check if this needs to be reformulated, this is not quite right?
            return (target.center.x - target.extents.x < bounds.center.x + bounds.extents.x &&
				target.center.y - target.extents.y < bounds.center.y + bounds.extents.y) ||
				(target.center.x + target.extents.x < bounds.center.x - bounds.extents.x &&
				target.center.y + target.extents.y > bounds.center.y - bounds.extents.y);
		}
		
		public bool IsOutOfBounds(Bounds target) // return true if the target area does not intersect with this.bounds at any point
		{	
            return target.center.x - target.extents.x > bounds.center.x + bounds.extents.x ||
				target.center.y - target.extents.y > bounds.center.y + bounds.extents.y ||
				target.center.x + target.extents.x < bounds.center.x - bounds.extents.x ||
				target.center.y + target.extents.y < bounds.center.y - bounds.extents.y;
		}
        #endregion

        public void Split()
        {
            if (IsLeaf() && level < MaxLevels)
            {
                Vector3 childSize = bounds.extents / 2;

                children[(int)Quadrants.Northeast] = new QuadtreeNode(new Bounds(new Vector3(bounds.center.x + childSize.x / 2, bounds.center.y - childSize.y / 2, 0), childSize), level + 1, id, this);
				children[(int)Quadrants.Northwest] = new QuadtreeNode(new Bounds(new Vector3(bounds.center.x - childSize.x / 2, bounds.center.y - childSize.y / 2, 0), childSize), level + 1, id, this);
				children[(int)Quadrants.Southwest] = new QuadtreeNode(new Bounds(new Vector3(bounds.center.x - childSize.x / 2, bounds.center.y + childSize.y / 2, 0), childSize), level + 1, id, this);
				children[(int)Quadrants.Southeast] = new QuadtreeNode(new Bounds(new Vector3(bounds.center.x + childSize.x / 2, bounds.center.y + childSize.y / 2, 0), childSize), level + 1, id, this);

                int quadrant;

                foreach(GameObject item in items)
                {
                    quadrant = MatchQuadrant(item.GetComponent<Collider>().bounds);
                    if (quadrant > -1)
                    {
                        Remove(item);
                        children[quadrant].Insert(item);
                    }

                }
            }
        }
		
		public int CountNodes()
		{
			int count = 1;
			if (!IsLeaf())
            {
                foreach (QuadtreeNode child in children)
                {
                    count += child.CountNodes();
                }
            }
			return count;
		}

        #region Tree traversal
        public void TraverseDepthFirstPreOrder(Func<int> delegateMethod)
		{
            delegateMethod();
            if (!IsLeaf())
            {
                foreach (QuadtreeNode child in children)
                {
                    child.TraverseDepthFirstPreOrder(delegateMethod);
                }
            }
		}
		
		public void TraverseDepthFirstInOrder(Func<int> delegateMethod)
		{
            if (!IsLeaf())
            {
                foreach (QuadtreeNode child in children)
                {
                    child.TraverseDepthFirstInOrder(delegateMethod);
                    delegateMethod();
                }
            }
		}
		
		public void TraverseDepthFirstPostOrder(Func<int> delegateMethod)
		{
            if (!IsLeaf())
            {
                foreach (QuadtreeNode child in children)
                {
                    child.TraverseDepthFirstPostOrder(delegateMethod);
                }
            }
            delegateMethod();
		}
        #endregion

        #region Debug
        public bool AssetItemsInBounds()
        {

            return true;
        }

		public int DebugDisplayNodes() // TODO: check if this work
		{	
            foreach(GameObject item in items)
            {
				int lvl = level;
				string offset = "";
				
				while(lvl > 0)
				{
					offset += ">";
                    lvl--;
				}
				
                Debug.Log(offset + " " + id, item);
            }
            return 0;
		}
        #endregion
    }
}