using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RegistrationEventArgs : EventArgs
{
    public string Username;
    public string Password;
    public string Email;
    public string Company;
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
    public Button registerButton;

    public RegistrationData registrationData = new RegistrationData();

    private string username = "";
    private string password = "";
    private string email = "";
    private string company = "";

    void Start()
    {
        Initialize();
        registerButton.onClick.AddListener(Register);
    }

    void OnEnable()
    {
        usernameField.onEndEdit.AddListener((x) => username = registrationData.username = x);
        passwordField.onEndEdit.AddListener((x) => password = registrationData.password = x);
        emailField.onEndEdit.AddListener((x) => email = registrationData.email = x);
        companyField.onEndEdit.AddListener((x) => company = registrationData.company = x);
    }

    void OnDisable()
    {
        usernameField.onEndEdit.RemoveAllListeners();
        passwordField.onEndEdit.RemoveAllListeners();
        emailField.onEndEdit.RemoveAllListeners();
        companyField.onEndEdit.RemoveAllListeners();
        //registerButton.onClick.RemoveAllListeners();
    }

    public void ClearUserInputField()
    {
        usernameField.text = "";
        username = "";
    }

    public void ClearPassInputField()
    {
        passwordField.text = "";
        password = "";
    }

    public bool Initialize()
    {
        ClearUserInputField();
        ClearPassInputField();
        emailField.text = email = "";
        companyField.text = company = "";
        registrationData.Clear();
        return true;
    }

    public void Register()
    {
        if (OnRegistered != null)
        {
            OnRegistered(this, new RegistrationEventArgs {
                Username = username,
                Password = password,
                Email = email,
                Company = company,
                Time = DateTime.Now
            });
        }
    }
}
