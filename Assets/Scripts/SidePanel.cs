using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SidePanel : MonoBehaviour
{
    public static event System.Action<object, string> OnProductButtonClicked;
    public static event System.Action<object, string> OnAboutButtonClicked;

    public Button AboutButton;    
    public Button ProductsButton;
    public GameObject ShowcaseGroup;
    public Button ContactButton;

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}

    void OnEnable()
    {
        AboutButton.onClick.AddListener(ShowAboutUs);
        ProductsButton.onClick.AddListener(ShowProducts);
    }
    void OnDisable()
    {
        AboutButton.onClick.RemoveAllListeners();
        ProductsButton.onClick.RemoveAllListeners();
    }

    public void Initialize()
    {
        AboutButton.image.sprite = AboutButton.spriteState.pressedSprite;
        ProductsButton.image.sprite = ProductsButton.spriteState.disabledSprite;
        ContactButton.image.sprite = ContactButton.spriteState.disabledSprite;
    }

    public void ShowProducts()
    {
        if (OnProductButtonClicked != null)
            OnProductButtonClicked(this, "Show product button clicked");
    }

    public void ShowAboutUs()
    {
        if (OnAboutButtonClicked != null)
            OnAboutButtonClicked(this, "About Us button clicked");
    }

}
