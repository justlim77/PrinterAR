using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace CopierAR
{
    public class RegistrationEventArgs : EventArgs
    {
        public RegistrationData RegistrationData;
        public DateTime Time;
    }

    public class RegistrationView : MonoBehaviour
    {
        public delegate void RegistrationEventHandler(object sender, RegistrationEventArgs args);
        public static event RegistrationEventHandler OnRegistered;

        public InputField usernameField;
        public InputField passwordField;
        public InputField emailField;
        public InputField companyField;
        public Text usernameComment;
        public Text passwordComment;
        public Text emailComment;
        public Text companyComment;
        public Button registerButton;

        public RegistrationData registrationData { get; private set; }

        void Start()
        {
            registrationData = new RegistrationData();
            Initialize();
            registerButton.onClick.AddListener(Register);
            UserBar.OnRegisterPressed += UserBar_OnRegisterPressed;
        }

        void OnEnable()
        {
            usernameField.onEndEdit.AddListener((x) => registrationData.CName = x);
            usernameField.onEndEdit.AddListener(delegate { ValidateUsername(); });
            passwordField.onEndEdit.AddListener((x) => registrationData.CPwd = x);
            passwordField.onEndEdit.AddListener(delegate { ValidatePassword(); });
            emailField.onEndEdit.AddListener((x) => registrationData.Email = x);
            emailField.onEndEdit.AddListener(delegate { ValidateEmail(); });
            companyField.onEndEdit.AddListener((x) => registrationData.Company = x);
            companyField.onEndEdit.AddListener(delegate { ValidateCompany(); });
        }

        private void UserBar_OnRegisterPressed(object arg1, string arg2)
        {
            Initialize();
        }

        void OnDisable()
        {
            usernameField.onEndEdit.RemoveAllListeners();
            passwordField.onEndEdit.RemoveAllListeners();
            emailField.onEndEdit.RemoveAllListeners();
            companyField.onEndEdit.RemoveAllListeners();
        }

        public void ClearUserInputField()
        {
            usernameField.text = "";
            usernameComment.text = "";
        }

        public void ClearPassInputField()
        {
            passwordField.text = "";
            passwordComment.text = "";
        }

        public void ClearEmailField()
        {
            emailField.text = "";
            emailComment.text = "";
        }
        public void ClearCompanyField()
        {
            companyField.text = "";
            companyComment.text = "";
        }

        public bool Initialize()
        {
            ClearUserInputField();
            ClearPassInputField();
            ClearEmailField();
            ClearCompanyField();
            registrationData.Clear();
            return true;
        }

        public void Register()
        {
            //if (!ValidateName() || !ValidateUsername() || !ValidatePassword() || !ValidateEmail() || !ValidateCompany())
            //    return;

            // Check for required fields
            if (!isValid)
                return;

            if (OnRegistered != null)
            {
                OnRegistered(this, new RegistrationEventArgs
                {
                    RegistrationData = this.registrationData,
                    Time = DateTime.Now
                });
            }
        }

        public bool isValid
        {
            get
            {
                return (ValidateUsername() && ValidatePassword() && ValidateEmail() && ValidateCompany());
            }
        }

        private bool ValidateUsername()
        {
            // Check length
            if (usernameField.text.Length == 0)
            {
                ShowComment("Required field", ref usernameComment);
            }
            else if (usernameField.text.Length < 5)
            {
                ShowComment("Minimum 5 characters", ref usernameComment);
            }
            else
            {
                HideComment(ref usernameComment);
                return true;
            }
            return false;
        }

        private bool ValidatePassword()
        {
            // Check length
            if (passwordField.text.Length == 0)
            {
                ShowComment("Required field", ref passwordComment);
            }
            else if (passwordField.text.Length < 6)
            {
                ShowComment("Minimum 6 characters", ref passwordComment);
            }
            else
            {
                HideComment(ref passwordComment);
                return true;
            }
            return false;
        }
        private bool ValidateEmail()
        {
            // Check length
            if (emailField.text == null)
            {
                ShowComment("Required field", ref emailComment);
                return false;
            }

            int atSymbolPosition = emailField.text.IndexOf("@");

            // Check if the @ symbol is not found, at the start or end of the address.
            // Examples:
            // mail.com
            // @mail.com
            // johnsmith@
            // johnsmith@mail.com@
            if (atSymbolPosition < 1 || emailField.text.EndsWith("@"))
            {
                ShowComment("Invalid email format", ref emailComment);
                return false;
            }

            int periodSymbolPosition = emailField.text.IndexOf(".", atSymbolPosition);

            // Check if the period is not found, and that it's not beside the @ symbol, and it's not at the end.  
            // Examples:
            // johnsmith@mail
            // johnsmith@.mail.com
            // johnsmith@mail.
            // johnsmith@mail.co.
            if (periodSymbolPosition > (atSymbolPosition + 1) && !emailField.text.EndsWith("."))
            {
                HideComment(ref emailComment);
                return true;
            }

            ShowComment("Invalid email format", ref emailComment);
            return false;   
        }
        private bool ValidateCompany()
        {
            // Check length
            if (companyField.text.Length == 0)
            {
                ShowComment("Required field", ref companyComment);
            }
            else
            {
                HideComment(ref companyComment);
                return true;
            }
            return false;
        }

        private void ShowComment(string message, ref Text commentLabel)
        {
            commentLabel.text = string.Format("*{0}", message);
        }

        private void HideComment(ref Text commentLabel)
        {
            commentLabel.text = "";
        }
    }
}
