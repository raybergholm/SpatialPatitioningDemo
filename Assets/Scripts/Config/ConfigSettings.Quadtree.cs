using UnityEngine;
using SpatialPartitioning;

public static partial class ConfigSettings
{
    // change this to allow more items in the parent node before assigning child nodes
    [SerializeField]
    private static int maxItemsPerNode = 10;

    // change this to control the Quadtree depth. Deeper = more granularity, but consider the expected size of objects compared to the cell sizes!
    [SerializeField]
    private static int maxTreeDepth = 5;

    // chnage this to control the mode
    [SerializeField]
    private static QuadtreeMode mode = QuadtreeMode.EnclosingOnly;

    [SerializeField]
    private static bool rebuildEveryTick = true;
}