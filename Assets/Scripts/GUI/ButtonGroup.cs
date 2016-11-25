using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonGroup : Button
{
    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    public void SelectButton()
    {
        image.sprite = ActiveSprite;
        image.SetNativeSize();
    }

    public void DeselectButton()
    {
        image.sprite = InactiveSprite;
        image.SetNativeSize();
    }

    public void ToggleState()
    {
        image.sprite = image.sprite == ActiveSprite ? InactiveSprite : ActiveSprite;
        image.SetNativeSize();
    }
}