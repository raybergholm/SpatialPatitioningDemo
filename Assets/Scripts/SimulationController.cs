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

        private TestScripts tests;
        public TestScripts Tests { get { return tests; } }

        private bool isPaused;
        public bool IsPaused { get { return isPaused; } }

        private float currentTimescale;

        private void Awake()
        {
            model = this.GetComponent<SimulationModel>();
            tests = this.GetComponent<TestScripts>();

            isPaused = false;

            currentTimescale = 1.0f;

            if (optionsMenu != null)
            {
                optionsMenu.Init(this);
            }

            if (pauseMenu != null)
            {
                pauseMenu.Init(this);
            }

            

            Debug.Log("SimulationController now active");
        }

        // Use this for initialization
        private void Start()
        {

        }

        private void OnDestroy()
        {

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
                ToggleOptionsMenu();
            }
        }

        public void StartSimulation()
        {

        }

        private void ResetSimulation()
        {

        }

        public void TogglePause(bool showPauseMenu = true)
        {
            if (!isPaused)
            {
                Pause(showPauseMenu);
            }
            else
            {
                Resume();
            }
        }

        public void ToggleOptionsMenu()
        {
            if (optionsMenu != null)
            {
                if (!optionsMenu.IsVisible)
                {
                    optionsMenu.Show();
                    Pause(false);
                }
                else
                {
                    optionsMenu.Hide();
                    Resume();
                }
            }
        }

        public void Pause(bool showPauseMenu = true)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
            if (pauseMenu != null && showPauseMenu)
            {
                pauseMenu.Show();
            }
        }

        public void Resume()
        {
            Time.timeScale = currentTimescale;
            isPaused = false;
            if (pauseMenu != null && pauseMenu.IsVisible)
            {
                pauseMenu.Hide();
            }
        }
    }
}