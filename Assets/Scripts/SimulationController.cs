using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

namespace SpatialPartitioning
{
    public class SimulationController : MonoBehaviour
    {
        enum DataStructureTypes
        {
            Quadtree,
            SpatialHash
        }

        [SerializeField]
        private OptionsPanel optionsMenu;
        [SerializeField]
        private PausePanel pauseMenu;

        private SimulationModel model;

        private TestScripts tests;

        private bool isPaused;
        public bool IsPaused { get { return isPaused; } }

        private float currentTimescale;

        private void Awake()
        {
            model = GetComponent<SimulationModel>();
            tests = GetComponent<TestScripts>();

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
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                ToggleOptionsMenu();
            }
        }

        public List<string> GetDataStructures()
        {
            return Enum.GetNames(typeof(DataStructureTypes)).ToList();
        }

        public List<string> GetTestList()
        {
            return tests != null ? tests.GetManifest() : null;
        }

        public void StartSimulation()
        {
            // steup steps:
            // fetch the datastruct to be used
            // fetch the test setup

            // actually start it 

            Debug.Log("Simulation Started.");
        }

        public void ResetSimulation()
        {
            Debug.Log("Simulation Reset.");
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