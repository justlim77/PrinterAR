using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public enum MenuItem
    {
        None,
        Welcome,
        Sigin,
        Register,
        About,
        Showcase,
        LifeScale
    }

    public Text Header;
    public GameObject Background;

    public RectTransform MainPanel;
    public GameObject WelcomePanel;
    public SigninPanel SigninPanel;
    public GameObject RegisterPanel;
    public GameObject AboutUsPanel;
    public GameObject ContactUsPanel;

    public SidePanel SidePanel;
    public RightPanel RightPanel;

    public UserBar UserBar;

    public float AboutPanelHorizontalPosition = 162.0f;

    public static MenuItem menuItem = MenuItem.Welcome;

    private CanvasScaler m_CanvasScaler;

    private Vector2 m_InitialPanelPosition = new Vector2();

	// Use this for initialization
	void Start ()
    {
        m_CanvasScaler = GetComponent<CanvasScaler>();
        m_CanvasScaler.dynamicPixelsPerUnit = 4;

        m_InitialPanelPosition = MainPanel.anchoredPosition;

        LoadMenuItem(MenuItem.Welcome);
	}

    void OnEnable()
    {
        SigninPanel.OnSignedIn += SigninPanel_OnSignedIn;
        UserBar.OnSignedOut += UserBar_OnSignedOut;
        UserBar.OnSignInPressed += UserBar_OnSignInPressed;
        UserBar.OnRegisterPressed += UserBar_OnRegisterPressed;
        SidePanel.OnAboutButtonClicked += SidePanel_OnAboutButtonClicked;
        SidePanel.OnProductButtonClicked += SidePanel_OnProductButtonClicked;
        SidePanel.OnShowcaseButtonClicked += SidePanel_OnShowcaseButtonClicked;
        SidePanel.OnLifeScaleButtonClicked += SidePanel_OnLifeScaleButtonClicked;
        //SidePanel.OnContactButtonClicked += SidePanel_OnContactButtonClicked;
    }

    void OnDisable()
    {
        SigninPanel.OnSignedIn -= SigninPanel_OnSignedIn;
        UserBar.OnSignedOut -= UserBar_OnSignedOut;
        UserBar.OnSignInPressed -= UserBar_OnSignInPressed;
        UserBar.OnRegisterPressed -= UserBar_OnRegisterPressed;
        SidePanel.OnAboutButtonClicked -= SidePanel_OnAboutButtonClicked;
        SidePanel.OnProductButtonClicked -= SidePanel_OnProductButtonClicked;
        SidePanel.OnShowcaseButtonClicked -= SidePanel_OnShowcaseButtonClicked;
        SidePanel.OnLifeScaleButtonClicked -= SidePanel_OnLifeScaleButtonClicked;
        //SidePanel.OnContactButtonClicked -= SidePanel_OnContactButtonClicked;
    }

    void Update()
    {
        if (UserInterface.menuItem != MenuItem.LifeScale)
        {
            Background.SetActive(true);
        }
        else
        {
            Background.SetActive(false);
        }
    }

    private void SidePanel_OnLifeScaleButtonClicked(object arg1, string arg2)
    {
        if (RightPanel != null)
        {
            RightPanel.Initialize();
            RightPanel.ToggleInfoPanel(false);
        }
        else
            Debug.LogWarning("RightPanel is null!");

        LoadMenuItem(MenuItem.LifeScale);
    }

    private void SidePanel_OnShowcaseButtonClicked(object arg1, string arg2)
    {
        LoadMenuItem(MenuItem.Showcase);
    }

    private void SidePanel_OnContactButtonClicked(object arg1, string arg2)
    {
        ContactUsPanel.SetActive(true);
        ContactUsPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
        SidePanel.transform.SetAsLastSibling();
    }

    private void SidePanel_OnAboutButtonClicked(object arg1, string arg2)
    {
        //AboutUsPanel.SetActive(true);
        //AboutUsPanel.transform.SetAsLastSibling();
        //UserBar.transform.SetAsLastSibling();
        //SidePanel.transform.SetAsLastSibling();

        LoadMenuItem(MenuItem.About);

        MainPanel.anchoredPosition = new Vector2(AboutPanelHorizontalPosition, m_InitialPanelPosition.y);
    }

    private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
    {
        //SigninPanel.gameObject.SetActive(false);
        //RegisterPanel.SetActive(false);
        //AboutUsPanel.SetActive(false);
        ////ContactUsPanel.SetActive(false);
        //WelcomePanel.SetActive(false);

        //UserBar.transform.SetAsLastSibling();
        //SidePanel.transform.SetAsLastSibling();

        LoadMenuItem(MenuItem.Showcase);

        RightPanel.SetAsMain();
        RightPanel.ToggleInfoPanel(true);
    }

    private void UserBar_OnRegisterPressed(object arg1, string arg2)
    {
        //RegisterPanel.SetActive(true);
        //RegisterPanel.transform.SetAsLastSibling();
        //UserBar.transform.SetAsLastSibling();
        LoadMenuItem(MenuItem.Register);
        ResetPanelPosition();
    }

    private void UserBar_OnSignInPressed(object arg1, string arg2)
    {
        //SigninPanel.gameObject.SetActive(true);
        //SigninPanel.transform.SetAsLastSibling();
        //UserBar.transform.SetAsLastSibling();
        LoadMenuItem(MenuItem.Sigin);       
        ResetPanelPosition();
    }

    private void UserBar_OnSignedOut(object sender, string arg)
    {
        //AboutUsPanel.SetActive(true);
        //SidePanel.gameObject.SetActive(false);
        //RightPanel.gameObject.SetActive(false);
        //RegisterPanel.SetActive(false);
        //AboutUsPanel.SetActive(false);
        //ContactUsPanel.SetActive(false);

        SigninPanel.Initialize();
        //SigninPanel.transform.SetAsLastSibling();
        //UserBar.transform.SetAsLastSibling();
        //SigninPanel.gameObject.SetActive(true);

        LoadMenuItem(MenuItem.Sigin);

        ResetPanelPosition();
    }


    private void SigninPanel_OnSignedIn(object sender, SignInEventArgs args)
    {
        UserBar.UpdateName(args.Username);
        Debug.Log(string.Format("{0} signed in at {1}:{2}", args.Username, args.Time.Hour, args.Time.Minute));

        //AboutUsPanel.SetActive(true);
        //AboutUsPanel.transform.SetAsLastSibling();
        //UserBar.transform.SetAsLastSibling();

        SidePanel.Initialize();
        //SidePanel.SetAsMain();

        LoadMenuItem(MenuItem.About);

        MainPanel.anchoredPosition = new Vector2(AboutPanelHorizontalPosition, m_InitialPanelPosition.y);
    }

    private void ResetPanelPosition()
    {
        MainPanel.anchoredPosition = m_InitialPanelPosition;
    }

    private void LoadMenuItem(MenuItem menuItem)
    {
        switch (menuItem)
        {
            case MenuItem.None:
                MainPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                SigninPanel.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);
                break;
            case MenuItem.Welcome:
                MainPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(true);
                SigninPanel.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Welcome";
                break;
            case MenuItem.Sigin:
                MainPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                SigninPanel.gameObject.SetActive(true);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Sign in";
                break;
            case MenuItem.Register:
                MainPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                SigninPanel.gameObject.SetActive(false);
                RegisterPanel.SetActive(true);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Register";
                break;
            case MenuItem.About:
                MainPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                SigninPanel.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(true);
                SidePanel.gameObject.SetActive(true);
                RightPanel.gameObject.SetActive(false);

                Header.text = "About Us";
                break;
            case MenuItem.Showcase:
                MainPanel.gameObject.SetActive(false);

                if (RightPanel != null)
                    RightPanel.ToggleInfoPanel(true);

                break;
            case MenuItem.LifeScale:
                MainPanel.gameObject.SetActive(false);
                break;
        }

        UserInterface.menuItem = menuItem;
    }
}
