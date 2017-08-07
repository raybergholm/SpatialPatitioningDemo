using UnityEngine;

using SpatialPartitioning.BoundingVolumes;

namespace SpatialPartitioning
{
    public class SimulationModel : MonoBehaviour
    {
        private SimulationController controller;

        public Quadtree Quadtree { get; private set; }

        public SpatialHash SpatialHash { get; private set; }

        public Bounds Plane2D { get; private set; }

        private void Awake()
        {
            controller = this.GetComponent<SimulationController>();

            InitModel();
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void InitModel()
        {
            if (Plane2D == null)
            {
                Rect configPlane = new Rect(new Vector2(0, 0), new Vector2(ConfigSettings.DefaultSimulationBoundsHeight, ConfigSettings.DefaultSimulationBoundsWidth));
                Plane2D = new Bounds();

                Debug.Log("Simulation model initialised");
            }
        }

        public void AddItem()
        {
            // TODO: just statically checking quadtree for now

            Bounds bounds = new Bounds(GetRandomSpot(), new Vector3(1,1,1));
            Item item = new Item(bounds);

            Quadtree.InsertItem(item);
        }

        public void Validate()
        {
            // TODO: just statically checking quadtree for now
            Quadtree.Validate();
        }

        private Vector3 GetRandomSpot()
        {
            return new Vector3(Random.Range(Plane2D.min.x, Plane2D.max.x), Random.Range(Plane2D.min.y, Plane2D.max.y),  0.0f);
        }
    }
}