using SpatialPartitioning;

public static partial class ConfigSettings
{
    public static int DefaultQuadtreeMaxItemsPerNode { get { return maxItemsPerNode; } }
    public static int DefaultQuadtreeMaxTreeDepth { get { return maxTreeDepth; } }
    public static QuadtreeMode DefaultQuadtreeMode { get { return mode; } }

    public static bool QuadtreeRebuildEveryTick { get { return rebuildEveryTick; } }
    
    public static string ScenesRootPath { get { return scenesRootPath; } }
    public static string PrefabsRootPath { get { return prefabsRootPath; } }

    public static float DefaultSimulationBoundsHeight { get { return defaultSimulationBoundsHeight; } }
    public static float DefaultSimulationBoundsWidth { get { return defaultSimulationBoundsWidth; } }

    private static string scenesRootPath = "./Scenes/";
    private static string prefabsRootPath = "./Prefabs/";

    private static float defaultSimulationBoundsHeight = 200;
    private static float defaultSimulationBoundsWidth = 200;
}