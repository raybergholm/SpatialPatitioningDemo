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

            Debug.Log("SimulationController now active");
        }

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(ReportStillAlive());
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

        private IEnumerator ReportStillAlive()
        {
            while (true)
            {
                Debug.Log(string.Format("I'm alive, timescale: {0}", Time.timeScale));
                yield return new WaitForSeconds(3.0f);
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
            if (Mathf.Approximately(Time.timeScale, 0.0f))
            {
                Resume(showPauseMenu);
            }
            else
            {
                Pause(showPauseMenu);
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
                    Resume(false);
                }
            }
        }

        public void Pause(bool showPauseMenu = true)
        {
            Time.timeScale = 0.0f;
            if (pauseMenu != null && showPauseMenu)
            {
                pauseMenu.Show();
            }
        }

        public void Resume(bool showPauseMenu = true)
        {
            Time.timeScale = 1.0f;
            if (pauseMenu != null && showPauseMenu)
            {
                pauseMenu.Hide();
            }
        }
    }
}