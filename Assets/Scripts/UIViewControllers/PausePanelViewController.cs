using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{
    public class PausePanelViewController : BasePanelViewController
    {
        private void Awake()
        {
            AssignCallbacks();
        }

        // Use this for initialization
        private void Start()
        {

        }

        private void OnDestroy()
        {
            RemoveCallbacks();
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public override void Init()
        {
            AssignCallbacks();
            Debug.Log("pausePanel active");
        }

        private void AssignCallbacks()
        {
            SimulationController.ShowPauseMenu += OnShowPauseMenu;
            SimulationController.HidePauseMenu += OnHidePauseMenu;
        }

        private void RemoveCallbacks()
        {
            SimulationController.ShowPauseMenu -= OnShowPauseMenu;
            SimulationController.HidePauseMenu -= OnHidePauseMenu;
        }

        private void OnShowPauseMenu()
        {
            Debug.Log("show pause panel");
            Show();
        }

        private void OnHidePauseMenu()
        {
            Debug.Log("hide pause panel");
            Hide();
        }
    }
}