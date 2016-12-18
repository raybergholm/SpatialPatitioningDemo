using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SpatialPartitioning
{
    public class OptionsPanelViewController : BasePanelViewController
    {
        [SerializeField]
        private Button randomScatterTestButton;
        [SerializeField]
        private Button gridTestButton;

        private void Awake()
        {

        }

        // Use this for initialization
        private void Start()
        {
            if (parentController == null || parentController.Tests == null)
            {
                Debug.LogError("Test scripts were not linked!");
                return;
            }

            if(randomScatterTestButton != null)
            {
                randomScatterTestButton.onClick.RemoveAllListeners();
                randomScatterTestButton.onClick.AddListener(parentController.Tests.RandomScatterTest);
            }

            if (gridTestButton != null)
            {
                gridTestButton.onClick.RemoveAllListeners();
                gridTestButton.onClick.AddListener(parentController.Tests.GridTest);
            }

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
            Debug.Log("OptionsPanelViewController now active");
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