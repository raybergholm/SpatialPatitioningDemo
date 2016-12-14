using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{
    public class OptionsPanelViewController : BasePanelViewController
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
            Debug.Log("optionsPanel active");
        }

        private void AssignCallbacks()
        {
            SimulationController.ShowOptionsMenu += OnShowOptionsMenu;
            SimulationController.HideOptionsMenu += OnHideOptionsMenu;
            
        }

        private void RemoveCallbacks()
        {
            SimulationController.ShowOptionsMenu -= OnShowOptionsMenu;
            SimulationController.HideOptionsMenu -= OnHideOptionsMenu;
        }

        private void OnShowOptionsMenu()
        {
            Debug.Log("show options panel");
            Show();
        }

        private void OnHideOptionsMenu()
        {
            Debug.Log("hide options panel");
            Hide();
        }
    }
}