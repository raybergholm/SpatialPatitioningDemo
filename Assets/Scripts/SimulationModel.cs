using UnityEngine;

namespace SpatialPartitioning
{
    public class SimulationModel : MonoBehaviour
    {
        SimulationViewController viewController;

        private void Awake()
        {
            viewController = this.GetComponent<SimulationViewController>();
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}