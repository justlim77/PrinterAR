using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour
{
    public GameObject WelcomePanel;
    public SigninPanel SigninPanel;
    public GameObject RegisterPanel;
    public GameObject AboutUsPanel;
    public GameObject ContactUsPanel;

    public SidePanel SidePanel;
    public GameObject RightPanel;

    public UserBar UserBar;

	// Use this for initialization
	void Start ()
    {
        SidePanel.gameObject.SetActive(false);
        RightPanel.SetActive(false);
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
        SidePanel.OnProductButtonClicked += SidePanel_OnProductButtonClicked;
        SidePanel.OnAboutButtonClicked += SidePanel_OnAboutButtonClicked;

    }

    private void SidePanel_OnAboutButtonClicked(object arg1, string arg2)
    {
        AboutUsPanel.SetActive(true);
        AboutUsPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
    }

    private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
    {
        RightPanel.SetActive(true);
        SigninPanel.gameObject.SetActive(false);
        RegisterPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        ContactUsPanel.SetActive(false);
        WelcomePanel.SetActive(false);
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
        RightPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        AboutUsPanel.SetActive(false);
        ContactUsPanel.SetActive(false);

        SigninPanel.Initialize();
        SigninPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();
        SigninPanel.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        SigninPanel.OnSignedIn -= SigninPanel_OnSignedIn;
        UserBar.OnSignedOut -= UserBar_OnSignedOut;
        UserBar.OnSignInPressed -= UserBar_OnSignInPressed;
        UserBar.OnRegisterPressed -= UserBar_OnRegisterPressed;
        SidePanel.OnProductButtonClicked -= SidePanel_OnProductButtonClicked;
        SidePanel.OnAboutButtonClicked -= SidePanel_OnAboutButtonClicked;
    }

    private void SigninPanel_OnSignedIn(object sender, SignInEventArgs args)
    {
        UserBar.UpdateName(args.Username);
        Debug.Log(string.Format("{0} signed in at {1}:{2}", args.Username, args.Time.Hour, args.Time.Minute));

        AboutUsPanel.SetActive(true);
        AboutUsPanel.transform.SetAsLastSibling();
        UserBar.transform.SetAsLastSibling();

        SidePanel.Initialize();
        SidePanel.gameObject.SetActive(true);
        SidePanel.transform.SetAsLastSibling();
    }
}
