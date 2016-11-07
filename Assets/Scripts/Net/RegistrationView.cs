using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace CopierAR
{
    public class RegistrationEventArgs : EventArgs
    {
        public RegistrationData registrationData;
        public DateTime time;
    }

    public class RegistrationView : MonoBehaviour
    {
        public delegate void RegistrationEventHandler(object sender, RegistrationEventArgs args);
        public static event RegistrationEventHandler OnRegistered;

        public InputField nameField;
        public Text nameComment;
        public InputField usernameField;
        public Text usernameComment;
        public InputField passwordField;
        public Text passwordComment;
        public InputField emailField;
        public Text emailComment;
        public InputField companyField;
        public Text companyComment;

        public Button registerButton;

        public RegistrationData registrationData { get; private set; }

        void Start()
        {
            Initialize();
            registerButton.onClick.AddListener(Register);
        }

        void OnEnable()
        {
            nameField.onEndEdit.AddListener((x) => registrationData.CName = x);
            nameField.onEndEdit.AddListener(delegate { ValidateName(nameField); });
            usernameField.onEndEdit.AddListener((x) => registrationData.CUserID = x);
            usernameField.onEndEdit.AddListener(delegate { ValidateUsername(usernameField); });
            passwordField.onEndEdit.AddListener((x) => registrationData.CPwd = x);
            passwordField.onEndEdit.AddListener(delegate { ValidatePassword(passwordField); });
            emailField.onEndEdit.AddListener((x) => registrationData.Email = x);
            emailField.onEndEdit.AddListener(delegate { ValidateEmail(emailField); });
            companyField.onEndEdit.AddListener((x) => registrationData.Company = x);
            companyField.onEndEdit.AddListener(delegate { ValidateCompany(companyField); });
            // Validate input
            //nameField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateName(addedChar); };
        }

        void OnDisable()
        {
            nameField.onEndEdit.RemoveAllListeners();
            usernameField.onEndEdit.RemoveAllListeners();
            passwordField.onEndEdit.RemoveAllListeners();
            emailField.onEndEdit.RemoveAllListeners();
            companyField.onEndEdit.RemoveAllListeners();
            //registerButton.onClick.RemoveAllListeners();
        }

        public void ClearNameInputField()
        {
            nameField.text = "";
        }
        public void ClearUserInputField()
        {
            usernameField.text = "";
        }

        public void ClearPassInputField()
        {
            passwordField.text = "";
        }

        public void ClearEmailField()
        {
            emailField.text = "";
        }
        public void ClearCompanyField()
        {
            companyField.text = "";
        }

        public bool Initialize()
        {
            ClearNameInputField();
            ClearUserInputField();
            ClearPassInputField();
            ClearEmailField();
            ClearCompanyField();
            registrationData.Clear();
            return true;
        }

        public void Register()
        {
            // Check for required fields:


            if (OnRegistered != null)
            {
                OnRegistered(this, new RegistrationEventArgs
                {
                    registrationData = this.registrationData,
                    time = DateTime.Now
                });
            }
        }

        private void ValidateName(InputField input)
        {
            // Check length
            if (input.text.Length == 0)
            {
                ShowComment("Enter a name", ref nameComment);
            }
            else if (input.text.Length < 2)
            {
                ShowComment("Name too short", ref nameComment);
            }
            else
            {
                HideComment(ref nameComment);
            }
        }

        private void ValidateUsername(InputField input)
        {
            // Check length
            if (input.text.Length == 0)
            {
                ShowComment("Enter a username", ref usernameComment);
            }
            else if (input.text.Length < 5)
            {
                ShowComment("Username too short", ref usernameComment);
            }
            else
            {
                HideComment(ref usernameComment);
            }
        }

        private void ValidatePassword(InputField input)
        {
            // Check length
            if (input.text.Length == 0)
            {
                ShowComment("Enter a password", ref passwordComment);
            }
            else if (input.text.Length < 6)
            {
                ShowComment("Minimum 6 characters", ref passwordComment);
            }
            else
            {
                HideComment(ref passwordComment);
            }
        }
        private void ValidateEmail(InputField input)
        {
            // Check length
            if (input.text.Length == 0)
            {
                ShowComment("Enter an email", ref emailComment);
            }
            else if (input.)
            {
                ShowComment("", ref emailComment);
            }
            else
            {
                HideComment(ref emailComment);
            }
        }
        private void ValidateCompany(InputField input)
        {
            // Check length
            if (input.text.Length == 0)
            {
                ShowComment("Enter a username", ref companyComment);
            }
            else if (input.text.Length < 5)
            {
                ShowComment("Username too short", ref companyComment);
            }
            else
            {
                HideComment(ref companyComment);
            }
        }

        private void ShowComment(string message, ref Text commentLabel)
        {
            commentLabel.text = message;
        }

        private void HideComment(ref Text commentLabel)
        {
            commentLabel.text = "";
        }
    }
}
