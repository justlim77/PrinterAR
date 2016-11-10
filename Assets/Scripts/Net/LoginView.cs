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

        public LoginData loginData { get; private set; }

        void Start()
        {
            loginData = new LoginData();
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
            UserInputField.onEndEdit.AddListener(delegate { ValidateUsername(); });
            UserInputField.onValueChanged.AddListener(delegate { usernameComment.text = ""; });

            PassInputField.onEndEdit.AddListener((x) => loginData.CPwd = x);
            PassInputField.onEndEdit.AddListener(delegate { ValidatePassword(); });
            PassInputField.onValueChanged.AddListener(delegate { passwordComment.text = ""; });
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
            ClearUserInputField();
            ClearPassInputField();
            loginData.Clear();
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
                return;

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
