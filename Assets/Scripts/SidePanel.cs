using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SidePanel : MonoBehaviour
{
    public static event System.Action<object, string> OnAboutButtonClicked;
    public static event System.Action<object, string> OnProductButtonClicked;
    public static event System.Action<object, string> OnContactButtonClicked;

    public ButtonGroup AboutButton;    
    public ButtonGroup ProductsButton;
    public GameObject ShowcaseGroup;
    public ButtonGroup ContactButton;

    private RectTransform m_ContactRect;
    private Vector2 m_ContactInitialPosition;

    private PanelType m_PanelType = PanelType.None;

    void Awake()
    {
        m_ContactRect = ContactButton.GetComponent<RectTransform>();
        m_ContactInitialPosition = m_ContactRect.anchoredPosition;
        print(m_ContactInitialPosition);
    }

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}

    void OnEnable()
    {
        AboutButton.onClick.AddListener(ShowAboutUs);
        ProductsButton.onClick.AddListener(ShowProducts);
        ContactButton.onClick.AddListener(ShowContactUs);
    }
    void OnDisable()
    {
        AboutButton.onClick.RemoveAllListeners();
        ProductsButton.onClick.RemoveAllListeners();
        ContactButton.onClick.RemoveAllListeners();
    }

    public void Initialize()
    {
        //AboutButton.image.sprite = AboutButton.spriteState.pressedSprite;
        //ProductsButton.image.sprite = ProductsButton.spriteState.disabledSprite;
        //ContactButton.image.sprite = ContactButton.spriteState.disabledSprite;

        AboutButton.SelectButton();
        ProductsButton.DeselectButton();
        ContactButton.DeselectButton();

        m_PanelType = PanelType.About;

        ShowcaseGroup.SetActive(false);

        m_ContactRect.anchoredPosition = m_ContactInitialPosition;
    }

    public void ShowAboutUs()
    {
        //AboutButton.image.sprite = AboutButton.spriteState.pressedSprite;
        //ProductsButton.image.sprite = ProductsButton.spriteState.disabledSprite;
        //ContactButton.image.sprite = ContactButton.spriteState.disabledSprite;

        if (m_PanelType == PanelType.About)
            return;

        m_PanelType = PanelType.About;

        AboutButton.SelectButton();
        ProductsButton.DeselectButton();
        ContactButton.DeselectButton();

        ShowcaseGroup.SetActive(false);

        m_ContactRect.anchoredPosition = m_ContactInitialPosition;

        if (OnAboutButtonClicked != null)
            OnAboutButtonClicked(this, "About Us button clicked");
    }

    public void ShowProducts()
    {
        //AboutButton.image.sprite = AboutButton.spriteState.disabledSprite;
        //ProductsButton.image.sprite = ProductsButton.spriteState.pressedSprite;
        //ContactButton.image.sprite = ContactButton.spriteState.disabledSprite;

        if (m_PanelType == PanelType.Products)
            return;

        m_PanelType = PanelType.Products;

        AboutButton.DeselectButton();
        ProductsButton.SelectButton();
        ContactButton.DeselectButton();

        ShowcaseGroup.SetActive(true);

        m_ContactRect.anchoredPosition = new Vector2(m_ContactRect.anchoredPosition.x, m_ContactRect.anchoredPosition.y - 230);

        if (OnProductButtonClicked != null)
            OnProductButtonClicked(this, "Show product button clicked");
    }


    public void ShowContactUs()
    {
        //AboutButton.image.sprite = AboutButton.spriteState.disabledSprite;
        //ProductsButton.image.sprite = ProductsButton.spriteState.disabledSprite;
        //ContactButton.image.sprite = ContactButton.spriteState.pressedSprite;

        if (m_PanelType == PanelType.Contact)
            return;

        m_PanelType = PanelType.Contact;

        AboutButton.DeselectButton();
        ProductsButton.DeselectButton();
        ContactButton.SelectButton();

        ShowcaseGroup.SetActive(false);

        m_ContactRect.anchoredPosition = m_ContactInitialPosition;

        if (OnContactButtonClicked != null)
            OnContactButtonClicked(this, "Contact Us button clicked");
    }

}

public enum PanelType
{
    None,
    Welcome,
    Signin,
    Register,
    About,
    Products,
    Contact    
}
