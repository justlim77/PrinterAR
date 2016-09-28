using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RightPanel : MonoBehaviour
{
    public GameObject InfoPanel;
    public PhotocopierController PhotocopierController;

    public bool Initialize()
    {
        if (PhotocopierController != null)
        {
            return PhotocopierController.Initialize();
        }
        else
        {
            Debug.LogWarning("PhotocopierController is null!");
            return false;
        }
    }

    public void ToggleInfoPanel(bool value = true)
    {
        if (InfoPanel != null)
        {
            InfoPanel.SetActive(value);
        }
        else
            Debug.LogWarning("InfoPanel is null!");

        PhotocopierController.CanvasGroup.alpha = value ? 0 : 1;
        PhotocopierController.CanvasGroup.interactable = value ? false : true;
    }

    public void SetAsMain()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }
}
