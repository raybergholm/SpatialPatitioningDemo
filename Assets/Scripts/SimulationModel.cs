using UnityEngine;

namespace SpatialPartitioning
{
    public class SimulationModel : MonoBehaviour
    {
        SimulationController controller;

        private void Awake()
        {
            controller = this.GetComponent<SimulationController>();
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