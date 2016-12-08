using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning.Quadtree
{
    public partial class Quadtree
    {
        public class QuadtreeNode
        {
            // Members
            private readonly Quadtree owner;
            private readonly string nodeName;
            private readonly int nodeId;
            private readonly Bounds bounds;
            private readonly int level;
            private readonly QuadtreeNode parentNode;

            private List<CollidableEntity> items;
            public List<CollidableEntity> Items { get { return items; } }

            private QuadtreeNode[] children;   // recursive, each node may have child nodes
            public QuadtreeNode[] Children { get { return children; } }

            private static int nodeCount = 0;

            // Constants
            private const int CHILD_NODE_AMOUNT = 4;   // Quadtrees have 4 child nodes. Changing this would change the tree type

            // Constructors
            public QuadtreeNode(Quadtree owner, Bounds bounds, int level, string id = "", QuadtreeNode parentNode = null)
            {
                items = new List<CollidableEntity>();
                this.owner = owner;
                this.bounds = bounds;
                this.level = level;
                this.parentNode = parentNode;

                children = new QuadtreeNode[CHILD_NODE_AMOUNT];

                nodeName = this.parentNode != null ? (this.parentNode.nodeName + "_" + id) : "root";
                nodeId = ++nodeCount;
            }

            #region Items
            public void InsertItem(CollidableEntity item)
            {
                if (!IsLeaf())
                {
                    int quadrant = MatchQuadrant(item.AABB);

                    if (quadrant > -1)
                    {
                        children[quadrant].InsertItem(item);
                        return;
                    }
                }

                items.Add(item);

                if (items.Count > owner.maxItems && level < owner.maxLevels)
                {
                    Split();
                }
            }

            public bool RemoveItem(CollidableEntity item)
            {
                bool returnValue = items.Remove(item);

                if (!IsLeaf() && AreChildrenEmpty())
                {
                    RemoveChildren(); // all descendants empty, delete
                }
                return returnValue;
            }

            public void Clear()
            {
                items.Clear();
            }

            public bool Contains(CollidableEntity item)
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

                if (!IsLeaf())
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        count += children[i].GetCount();
                    }
                }

                return count;
            }

            // basically, give a target AABB and this method returns all objects in nodes which overlap the AABB.
            public List<CollidableEntity> GetItemsByArea(Bounds target) // the list return by the initial call contains objects from all nodes which overlap the given target area
            {
                List<CollidableEntity> collisionCandidates = new List<CollidableEntity>();
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
                    return new List<CollidableEntity>();
                }
            }

            public void ClearItems()
            {
                items.Clear();
            }
            #endregion

            public QuadtreeNode GetNode(Bounds target)
            {
                if (!IsLeaf())
                {
                    for (int i = 0; i < children.Length; i++)
                    {
                        if (children[i].IsEnclosing(target))
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
                            index = (int)Quadrants.NorthWest; // Northwest
                        }
                        else if (east)
                        {
                            index = (int)Quadrants.NorthEast; // Northeast
                        }
                    }
                    else if (south)
                    {
                        // Object completely fits within the south quadrants 	
                        if (west)
                        {
                            index = (int)Quadrants.SouthWest; // Southwest
                        }
                        else if (east)
                        {
                            index = (int)Quadrants.SouthEast; // Southeast
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
                if (IsLeaf() && level < owner.maxLevels)
                {
                    Vector3 childSize = bounds.extents / 2;

                    children[(int)Quadrants.NorthEast] = new QuadtreeNode(owner, new Bounds(new Vector3(bounds.center.x + childSize.x / 2, bounds.center.y - childSize.y / 2, 0), childSize), level + 1, nodeName, this);
                    children[(int)Quadrants.NorthWest] = new QuadtreeNode(owner, new Bounds(new Vector3(bounds.center.x - childSize.x / 2, bounds.center.y - childSize.y / 2, 0), childSize), level + 1, nodeName, this);
                    children[(int)Quadrants.SouthWest] = new QuadtreeNode(owner, new Bounds(new Vector3(bounds.center.x - childSize.x / 2, bounds.center.y + childSize.y / 2, 0), childSize), level + 1, nodeName, this);
                    children[(int)Quadrants.SouthEast] = new QuadtreeNode(owner, new Bounds(new Vector3(bounds.center.x + childSize.x / 2, bounds.center.y + childSize.y / 2, 0), childSize), level + 1, nodeName, this);

                    int quadrant;

                    foreach (CollidableEntity item in items)
                    {
                        quadrant = MatchQuadrant(item.AABB);
                        if (quadrant > -1)
                        {
                            RemoveItem(item);
                            children[quadrant].InsertItem(item);
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
            public bool AssertItemsInBounds()
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (IsOutOfBounds(items[i].AABB))
                    {
                        return false;
                    }
                }
                return true;
            }

            public override string ToString()
            {
                return String.Format("nodeId: {0}, nodeName: {1}, bounds: {2}, items in node: {3}", nodeName, nodeId.ToString(), bounds.ToString(), items.Count);
            }
            #endregion
        }
    }
}