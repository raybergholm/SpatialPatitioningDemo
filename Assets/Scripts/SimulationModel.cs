using UnityEngine;

namespace SpatialPartitioning
{
    public class SimulationModel : MonoBehaviour
    {
        private SimulationController controller;

        [SerializeField]
        private Bounds plane2D;

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
            if (plane2D == null)
            {
                Rect configPlane = new Rect(new Vector2(0, 0), new Vector2(ConfigSettings.DefaultSimulationBoundsHeight, ConfigSettings.DefaultSimulationBoundsWidth));
                plane2D = new Bounds();
            }

        }
    }
}