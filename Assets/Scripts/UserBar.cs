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
        void Start()
        {
            UserLabel.text = "";
            RegisterButton.SetActive(true);
            SigninButton.SetActive(true);
            SignoutButton.SetActive(false);
        }

        void OnEnable()
        {
            LoginView.OnLoggedIn += SigninPanel_OnSignedIn;
        }

        void OnDisable()
        {
            LoginView.OnLoggedIn -= SigninPanel_OnSignedIn;
        }

        private void SigninPanel_OnSignedIn(object sender, LoginEventArgs args)
        {
            UpdateName(args.loginData.CUserID);
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

        public void UpdateName(string name)
        {
            UserLabel.text = string.Format("Hi {0},", name);
        }

        public void Signout()
        {
            UserLabel.text = "";
            SignoutButton.SetActive(false);

            SigninButton.SetActive(true);
            RegisterButton.SetActive(true);

            if (OnSignedOut != null)
                OnSignedOut(this, "Signed out");
        }
    }
}

