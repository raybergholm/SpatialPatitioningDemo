using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{
    public class PausePanelViewController : BasePanelViewController
    {
        private void Awake()
        {
            Debug.Log("Awake called for Pause Panel");
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Log("Start called for Pause Panel");
        }

        private void OnDestroy()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        public override void Init(SimulationController parentController)
        {
            base.Init(parentController);
            Debug.Log("PausePanelViewController now active");
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}