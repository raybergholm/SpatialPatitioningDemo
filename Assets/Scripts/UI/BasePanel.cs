using UnityEngine;
using System.Collections;

namespace SpatialPartitioning
{

    public abstract class BasePanel : MonoBehaviour
    {
        protected SimulationController parentController;

        public bool IsVisible { get { return gameObject.activeSelf; } }

        public delegate void BasePanelEventHandler();

        public static event BasePanelEventHandler PanelShown;
        public static event BasePanelEventHandler PanelHidden;

        public virtual void Init(SimulationController parentController)
        {
            this.parentController = parentController;
        }

        public virtual void ToggleVisibility()
        {
            if (!gameObject.activeSelf)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            if (PanelShown != null)
            {
                PanelShown();
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);

            if (PanelHidden != null)
            {
                PanelHidden();
            }
        }
    }
}
