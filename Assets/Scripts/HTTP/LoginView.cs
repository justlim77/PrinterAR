using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace CopierAR
{
    public class LoginEventArgs : EventArgs
    {
        public LoginData loginData;
        public DateTime time;
    }

    public class LoginView : MonoBehaviour
    {
        public delegate void LoginEventHandler(object sender, LoginEventArgs args);
        public static event LoginEventHandler OnLoggedIn;

        public InputField UserInputField;
        public InputField PassInputField;
        public Toggle RememberMeToggle;
        public Text usernameComment;
        public Text passwordComment;
        public Text loginComment;
        public Button loginButton;

        private LoginData loginData = new LoginData();
        public LoginData LoginData
        {
            get
            {
                if (loginData == null)
                    loginData = new LoginData();
                return loginData;
            }
            set { loginData = value; }
        }

        void Start()
        {
            Initialize();
            loginButton.onClick.AddListener(delegate { Login(); });
            UserBar.OnSignInPressed += UserBar_OnSignInPressed;
        }

        void OnDestroy()
        {
            loginButton.onClick.RemoveAllListeners();
            UserBar.OnSignInPressed -= UserBar_OnSignInPressed;
        }

        void OnEnable()
        {
            UserInputField.onEndEdit.AddListener((x) => loginData.CName = x);
            UserInputField.onEndEdit.AddListener(x => PlayerPrefs.SetString(Constants.SAVED_NAME_PREF_KEY, x));
            UserInputField.onEndEdit.AddListener(delegate { ValidateUsername(); });
            UserInputField.onValueChanged.AddListener(delegate { usernameComment.text = ""; });

            PassInputField.onEndEdit.AddListener((x) => loginData.CPwd = x);
            PassInputField.onEndEdit.AddListener(delegate { ValidatePassword(); });
            PassInputField.onValueChanged.AddListener(delegate { passwordComment.text = ""; });

            RememberMeToggle.onValueChanged.AddListener((x) => ToggleRemember(x));
        }

        private void ToggleRemember(bool value)
        {
            PlayerPrefs.SetString(Constants.SAVED_NAME_PREF_KEY, value ? loginData.CName : "");
        }

        private bool CheckForSavedName()
        {
            string savedName = PlayerPrefs.GetString(Constants.SAVED_NAME_PREF_KEY);

            bool hasName = !string.IsNullOrEmpty(savedName);

            UserInputField.text = savedName;    // Set username to saved name if available
            loginData.CName = savedName;        // Set login data username
            usernameComment.text = "";          // Clear error text

            RememberMeToggle.isOn = hasName;

            //Debug.Log(loginData.CName);

            return hasName;
        }

        private void UserBar_OnSignInPressed(object arg1, string arg2)
        {
            Initialize();
        }

        void OnDisable()
        {
            UserInputField.onEndEdit.RemoveAllListeners();
            UserInputField.onValueChanged.RemoveAllListeners();

            PassInputField.onEndEdit.RemoveAllListeners();
            PassInputField.onValueChanged.RemoveAllListeners();

            RememberMeToggle.onValueChanged.RemoveAllListeners();
        }

        public void ClearUserInputField()
        {
            UserInputField.text = "";
            usernameComment.text = "";
        }

        public void ClearPassInputField()
        {
            PassInputField.text = "";
            passwordComment.text = "";
        }

        public bool Initialize()
        {
            //ClearUserInputField();
            ClearPassInputField();

            if (CheckForSavedName() == true)
                loginData.CName = "";
            else
                loginData.Clear();
            CheckForSavedName();

            HideError();
            return true;
        }

        private bool ValidateUsername()
        {
            bool valid = true;

            if (UserInputField.text == null)
            {
                Response response = new Response(true, "Required field", ResponseType.InvalidUser);
                ShowError(response);
                valid = false;
            }

            if (UserInputField.text.Length < 5)
            {
                Response response = new Response(true, "Minimum 5 characters", ResponseType.InvalidUser);
                ShowError(response);
                valid = false;
            }

            return valid;
        }
        private bool ValidatePassword()
        {
            bool valid = true;

            if (PassInputField.text == null)
            {
                Response response = new Response(true, "Required field", ResponseType.IncorrectPassword);
                ShowError(response);
                valid = false;
            }

            if (PassInputField.text.Length < 5)
            {
                Response response = new Response(true, "Minimum 5 characters", ResponseType.IncorrectPassword);
                ShowError(response);
                valid = false;
            }

            return valid;
        }

        public bool isValid
        {
            get
            {
                return (ValidateUsername() && ValidatePassword());
            }
        }

        public void ShowError(Response response)
        {
            switch (response.responseType)
            {
                case ResponseType.IncorrectPassword:
                    passwordComment.text = response.message;
                    break;
                case ResponseType.InvalidUser:
                    usernameComment.text = response.message;
                    break;
            }
        }

        public void HideError()
        {
            loginComment.text = "";
        }

        public void Login(bool success = false)
        {
            // Check for required fields
            if (!isValid || !success)
            {
                //Debug.Log(string.Format("{0},{1}", loginData.CName, loginData.CPwd));
                return;
            }

            // Clear error messages
            HideError();

            if (OnLoggedIn != null)
            {
                OnLoggedIn(this, new LoginEventArgs
                {
                    loginData = this.loginData,
                    time = DateTime.Now
                });
            }
        }
    }
}
