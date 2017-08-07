using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

namespace SpatialPartitioning
{
    public class OptionsPanel : BasePanel
    {
        [SerializeField]
        private Dropdown dataStructureDropdown;

        [SerializeField]
        private Dropdown testSelectionDropdown;

        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button resetButton;

        private void Awake()
        {

        }

        // Use this for initialization
        private void Start()
        {
            if (parentController == null)
            {
                Debug.LogError("Parent controller not linked!");
                return;
            }
            
            if (dataStructureDropdown != null)
            {
                List<string> dataStructures = parentController.GetDataStructures();
                if (dataStructures.Count > 0)
                {
                    dataStructureDropdown.AddOptions(dataStructures);
                }
            }

            if (testSelectionDropdown != null)
            {
                List<string> tests = parentController.GetTestList();
                if (tests.Count > 0)
                {
                    testSelectionDropdown.AddOptions(tests);
                }
            }

            if (startButton != null)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(parentController.StartSimulation);
            }

            if (resetButton != null)
            {
                resetButton.onClick.RemoveAllListeners();
                resetButton.onClick.AddListener(parentController.ResetSimulation);
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

        public int GetDataStructure()
        {
            return dataStructureDropdown.value;
        }
    }
}