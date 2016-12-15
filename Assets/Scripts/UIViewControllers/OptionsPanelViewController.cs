using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{
    public class OptionsPanelViewController : BasePanelViewController
    {

        private void Awake()
        {

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
            
        }

        public override void Init()
        {

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