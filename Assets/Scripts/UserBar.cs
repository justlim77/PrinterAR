using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CopierAR
{
    public class UserBar : MonoBehaviour
    {
        public static event System.Action<object, string> OnSignedOut;
        public static event System.Action<object, string> OnRegisterPressed;
        public static event System.Action<object, string> OnSignInPressed;

        public Text UserLabel;
        public GameObject RegisterButton;
        public GameObject SigninButton;
        public GameObject SignoutButton;

        // Use this for initialization
        private void Start()
        {
            Initialize();
        }

        private bool Initialize()
        {
            UserLabel.text = "";
            RegisterButton.SetActive(true);
            SigninButton.SetActive(true);
            SignoutButton.SetActive(false);

            return true;
        }

        private void OnEnable()
        {
            LoginView.OnLoggedIn += SigninPanel_OnSignedIn;
            UserSession.OnLogoutEnded += UserSession_OnLogoutEnded;
        }

        private void UserSession_OnLogoutEnded(object sender, System.EventArgs args)
        {
            Initialize();
        }

        private void OnDisable()
        {
            LoginView.OnLoggedIn -= SigninPanel_OnSignedIn;
            UserSession.OnLogoutEnded -= UserSession_OnLogoutEnded;
        }

        private void SigninPanel_OnSignedIn(object sender, LoginEventArgs args)
        {
            UpdateName(args.loginData.CName);
            SignoutButton.SetActive(true);

            RegisterButton.SetActive(false);
            SigninButton.SetActive(false);
        }

        public void Signin()
        {
            if (OnSignInPressed != null)
                OnSignInPressed(this, "Sign in button pressed");
        }

        public void Register()
        {
            if (OnRegisterPressed != null)
                OnRegisterPressed(this, "Register button pressed");
        }

        public void Signout()
        {
            if (OnSignedOut != null)
                OnSignedOut(this, "Signed out");
        }

        public void SignoutWithoutEvent()
        {
            UserLabel.text = "";
            SignoutButton.SetActive(false);

            SigninButton.SetActive(true);
            RegisterButton.SetActive(true);
        }

        public void UpdateName(string name)
        {
            UserLabel.text = string.Format("Hi {0},", name);
            SignoutButton.SetActive(true);

            SigninButton.SetActive(false);
            RegisterButton.SetActive(false);
        }
    }
}

