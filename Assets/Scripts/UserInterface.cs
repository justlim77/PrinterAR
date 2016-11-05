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

    public RectTransform contentPanel;
    public GameObject WelcomePanel;

    [Header("Login")]
    public LoginView loginView;

    [Header("Registration")]
    public RegistrationView registrationView;
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

    private LoginService m_loginService = new LoginService();
    private RegistrationService m_registrationService = new RegistrationService();

    /// <summary>
    /// Processes login response from HTTP server
    /// </summary>
    /// <param name="response"></param>
    void LoginResponseHandler(Response response)
    {
        // TODO: Implement notification system
        Debug.Log(string.Format("Login status: {0}, {1}", response.error, response.message));
    }

    /// <summary>
    /// Processes registration response from HTTP server
    /// </summary>
    /// <param name="response"></param>
    void RegistrationResponseHandler(Response response)
    {
        // TODO: Implement notification system
        Debug.Log(string.Format("Registration status: {0}, {1}", response.error, response.message));
    }

    // Use this for initialization
    void Start ()
    {
        //m_loginService = new LoginService();
        //m_registrationService = new RegistrationService();

        m_CanvasScaler = GetComponent<CanvasScaler>();
        m_CanvasScaler.dynamicPixelsPerUnit = 4;

        m_InitialPanelPosition = contentPanel.anchoredPosition;

        LoadMenuItem(MenuItem.Welcome);
	}

    void OnEnable()
    {
        LoginView.OnLoggedIn += SigninPanel_OnSignedIn;
        UserBar.OnSignedOut += UserBar_OnSignedOut;
        UserBar.OnSignInPressed += UserBar_OnSignInPressed;
        UserBar.OnRegisterPressed += UserBar_OnRegisterPressed;
        SidePanel.OnAboutButtonClicked += SidePanel_OnAboutButtonClicked;
        SidePanel.OnProductButtonClicked += SidePanel_OnProductButtonClicked;
        SidePanel.OnShowcaseButtonClicked += SidePanel_OnShowcaseButtonClicked;
        SidePanel.OnLifeScaleButtonClicked += SidePanel_OnLifeScaleButtonClicked;

        loginView.loginButton.onClick.AddListener(() =>
        {
            StartCoroutine(m_loginService.SendLoginData(loginView.loginData, LoginResponseHandler));
        });

        registrationView.registerButton.onClick.AddListener(() =>
        {
            StartCoroutine(m_registrationService.SendRegistrationData(registrationView.registrationData, RegistrationResponseHandler));
        });
    }

    void OnDisable()
    {
        LoginView.OnLoggedIn -= SigninPanel_OnSignedIn;
        UserBar.OnSignedOut -= UserBar_OnSignedOut;
        UserBar.OnSignInPressed -= UserBar_OnSignInPressed;
        UserBar.OnRegisterPressed -= UserBar_OnRegisterPressed;
        SidePanel.OnAboutButtonClicked -= SidePanel_OnAboutButtonClicked;
        SidePanel.OnProductButtonClicked -= SidePanel_OnProductButtonClicked;
        SidePanel.OnShowcaseButtonClicked -= SidePanel_OnShowcaseButtonClicked;
        SidePanel.OnLifeScaleButtonClicked -= SidePanel_OnLifeScaleButtonClicked;

        loginView.loginButton.onClick.RemoveAllListeners();
        registrationView.registerButton.onClick.RemoveAllListeners();
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
        LoadMenuItem(MenuItem.About);

        contentPanel.anchoredPosition = new Vector2(AboutPanelHorizontalPosition, m_InitialPanelPosition.y);
    }

    private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
    {
        LoadMenuItem(MenuItem.Showcase);

        RightPanel.SetAsMain();
        RightPanel.ToggleInfoPanel(true);
    }

    private void UserBar_OnRegisterPressed(object arg1, string arg2)
    {
        LoadMenuItem(MenuItem.Register);
        SetMainPanelHorizontal();
    }

    private void UserBar_OnSignInPressed(object arg1, string arg2)
    {
        LoadMenuItem(MenuItem.Sigin);       
        SetMainPanelHorizontal();
    }

    private void UserBar_OnSignedOut(object sender, string arg)
    {
        loginView.Initialize();

        LoadMenuItem(MenuItem.Sigin);

        SetMainPanelHorizontal();
    }


    private void SigninPanel_OnSignedIn(object sender, LoginEventArgs args)
    {
        UserBar.UpdateName(args.Username);
        Debug.Log(string.Format("{0} signed in at {1}:{2}", args.Username, args.Time.Hour, args.Time.Minute));

        SidePanel.Initialize();

        LoadMenuItem(MenuItem.About);

        SetMainPanelHorizontal(AboutPanelHorizontalPosition);
    }

    private void SetMainPanelHorizontal(float x = 0)
    {
        contentPanel.anchoredPosition = new Vector2(x, m_InitialPanelPosition.y);
    }

    private void LoadMenuItem(MenuItem menuItem)
    {
        switch (menuItem)
        {
            case MenuItem.None:
                contentPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                loginView.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);
                break;
            case MenuItem.Welcome:
                contentPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(true);
                loginView.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Welcome";
                break;
            case MenuItem.Sigin:
                contentPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                loginView.gameObject.SetActive(true);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Sign in";
                break;
            case MenuItem.Register:
                contentPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                loginView.gameObject.SetActive(false);
                RegisterPanel.SetActive(true);
                AboutUsPanel.SetActive(false);
                SidePanel.gameObject.SetActive(false);
                RightPanel.gameObject.SetActive(false);

                Header.text = "Register";
                break;
            case MenuItem.About:
                contentPanel.gameObject.SetActive(true);

                WelcomePanel.SetActive(false);
                loginView.gameObject.SetActive(false);
                RegisterPanel.SetActive(false);
                AboutUsPanel.SetActive(true);
                SidePanel.gameObject.SetActive(true);
                RightPanel.gameObject.SetActive(false);

                Header.text = "About Us";
                break;
            case MenuItem.Showcase:
                contentPanel.gameObject.SetActive(false);

                if (RightPanel != null)
                    RightPanel.ToggleInfoPanel(true);

                break;
            case MenuItem.LifeScale:
                contentPanel.gameObject.SetActive(false);
                break;
        }

        UserInterface.menuItem = menuItem;
    }
}
