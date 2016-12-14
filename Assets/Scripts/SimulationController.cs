using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{
    public class SimulationController : MonoBehaviour
    {
        [SerializeField]
        private OptionsPanelViewController optionsMenu;
        [SerializeField]
        private PausePanelViewController pauseMenu;

        private SimulationModel model;

        public delegate void PanelCallDelegate();

        public static event PanelCallDelegate ShowOptionsMenu;
        public static event PanelCallDelegate HideOptionsMenu;
        public static event PanelCallDelegate ShowPauseMenu;
        public static event PanelCallDelegate HidePauseMenu;

        private void Awake()
        {
            model = this.GetComponent<SimulationModel>();

            if (optionsMenu != null)
            {
                optionsMenu.Init();
            }

            if (pauseMenu != null)
            {
                pauseMenu.Init();
            }

            AssignCallbacks();
            Debug.Log("now active");
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePause();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                if (ShowOptionsMenu != null)
                {
                    ShowOptionsMenu();
                }

                Debug.Log("trying options menu");
            }
        }

        private void AssignCallbacks()
        {
            OptionsPanelViewController.PanelShown += OnOptionsPanelShown;
            OptionsPanelViewController.PanelHidden += OnOptionsPanelHidden;
        }

        private void RemoveCallbacks()
        {
            OptionsPanelViewController.PanelShown -= OnOptionsPanelShown;
            OptionsPanelViewController.PanelHidden -= OnOptionsPanelHidden;
        }

        private void OnOptionsPanelShown()
        {
            Pause();
        }

        private void OnOptionsPanelHidden()
        {
            Resume();
        }

        public void StartSimulation()
        {

        }

        private void ResetSimulation()
        {

        }

        public void TogglePause()
        {
            if (Mathf.Approximately(Time.timeScale, 0.0f))
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0.0f;
            if (ShowPauseMenu != null)
            {
                ShowPauseMenu();
            }

            Debug.Log("trying to pause");
        }

        public void Resume()
        {
            Time.timeScale = 1.0f;
            if (HidePauseMenu != null)
            {
                HidePauseMenu();
            }

            Debug.Log("trying to resume");
        }
    }
}