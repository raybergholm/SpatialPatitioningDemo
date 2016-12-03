using SpatialPartitioning.Quadtree;
using SpatialPartitioning.SpatialHash;

public static partial class ConfigSettings
{
    public static int DefaultQuadtreeMaxItemsPerNode { get { return maxItemsPerNode; } }
    public static int DefaultQuadtreeMaxTreeDepth { get { return maxTreeDepth; } }
    public static QuadtreeMode DefaultQuadtreeMode { get { return mode; } }
    public static bool QuadtreeRebuildEveryTick { get { return rebuildEveryTick; } }
    
}