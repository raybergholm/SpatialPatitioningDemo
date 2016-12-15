using UnityEngine;
using System.Collections;

public class BasePanelViewController : MonoBehaviour
{
    public bool IsVisible { get { return gameObject.activeSelf; } }

    public delegate void BasePanelDelegate();

    public static event BasePanelDelegate PanelShown;
    public static event BasePanelDelegate PanelHidden;

    public virtual void Init()
    {

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
