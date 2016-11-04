using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public GameObject WelcomePanel;
    public SigninPanel SigninPanel;
    public GameObject RegisterPanel;
    public GameObject AboutUsPanel;
    public GameObject ContactUsPanel;

    public SidePanel SidePanel;
    public RightPanel RightPanel;

    public UserBar UserBar;

    private CanvasScaler m_CanvasScaler;

	// Use this for initialization
	void Start ()
    {
        m_CanvasScaler = GetComponent<CanvasScaler>();
        m_CanvasScaler.dynamicPixelsPerUnit = 4;

        SidePanel.gameObject.SetActive(false);
        RightPanel.gameObject.SetActive(false);
        SigninPanel.gameObject.SetActive(false);
        RegisterPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        ContactUsPanel.SetActive(false);

        WelcomePanel.SetActive(true);
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

    private void SidePanel_OnLifeScaleButtonClicked(object arg1, string arg2)
    {
        if (RightPanel != null)
        {
            RightPanel.Initialize();
            RightPanel.ToggleInfoPanel(false);
        }
        else
            Debug.LogWarning("RightPanel is null!");
    }

    private void SidePanel_OnShowcaseButtonClicked(object arg1, string arg2)
    {
        if (RightPanel != null)
            RightPanel.ToggleInfoPanel(true);
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
        AboutUsPanel.SetActive(true);
        AboutUsPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
        SidePanel.transform.SetAsLastSibling();
    }

    private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
    {
        SigninPanel.gameObject.SetActive(false);
        RegisterPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        //ContactUsPanel.SetActive(false);
        WelcomePanel.SetActive(false);

        UserBar.transform.SetAsLastSibling();
        SidePanel.transform.SetAsLastSibling();

        RightPanel.SetAsMain();
        RightPanel.ToggleInfoPanel(true);
    }

    private void UserBar_OnRegisterPressed(object arg1, string arg2)
    {
        RegisterPanel.SetActive(true);
        RegisterPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
    }

    private void UserBar_OnSignInPressed(object arg1, string arg2)
    {
        SigninPanel.gameObject.SetActive(true);
        SigninPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
    }

    private void UserBar_OnSignedOut(object sender, string arg)
    {
        AboutUsPanel.SetActive(true);
        SidePanel.gameObject.SetActive(false);
        RightPanel.gameObject.SetActive(false);
        RegisterPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        //ContactUsPanel.SetActive(false);

        SigninPanel.Initialize();
        SigninPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
        SigninPanel.gameObject.SetActive(true);
    }


    private void SigninPanel_OnSignedIn(object sender, SignInEventArgs args)
    {
        UserBar.UpdateName(args.Username);
        Debug.Log(string.Format("{0} signed in at {1}:{2}", args.Username, args.Time.Hour, args.Time.Minute));

        AboutUsPanel.SetActive(true);
        AboutUsPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();

        SidePanel.Initialize();
        SidePanel.SetAsMain();
    }
}
