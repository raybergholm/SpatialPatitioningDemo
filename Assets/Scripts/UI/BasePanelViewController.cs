using UnityEngine;
using System.Collections;

public class BasePanelViewController : MonoBehaviour {

    public delegate void BasePanelDelegate();

    public static event BasePanelDelegate PanelShown;
    public static event BasePanelDelegate PanelHidden;

    public void Toggle()
    {
        if(!gameObject.activeSelf)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if(PanelShown != null)
        {
            PanelShown();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if(PanelHidden != null)
        {
            PanelHidden();
        }
    }
}
