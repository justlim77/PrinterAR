using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoginEventArgs : EventArgs
{
    public string Username;
    public string Password;
    public DateTime Time;
}

public class LoginView : MonoBehaviour
{
    public delegate void LoginEventHandler(object sender, LoginEventArgs args);
    public static event LoginEventHandler OnLoggedIn;

    public InputField UserInputField;
    public InputField PassInputField;
    public Toggle RememberMeToggle;
    public Button loginButton;

    public LoginData loginData = new LoginData();

    private string username = "";
    private string password = "";

    void Start()
    {
        Initialize();
        loginButton.onClick.AddListener(Login);
    }

    void OnEnable()
    {
        UserInputField.onEndEdit.AddListener((x) => username = loginData.username = x);
        PassInputField.onEndEdit.AddListener((x) => password = loginData.password = x);
    }

    void OnDisable()
    {
        UserInputField.onEndEdit.RemoveAllListeners();
        PassInputField.onEndEdit.RemoveAllListeners();
        //loginButton.onClick.RemoveAllListeners();
    }

    public void ClearUserInputField()
    {
        UserInputField.text = "";
    }

    public void ClearPassInputField()
    {
        PassInputField.text = "";
    }

    public bool Initialize()
    {
        ClearUserInputField();
        ClearPassInputField();
        loginData.Clear();
        return true;
    }

    public void Login()
    {
        if (OnLoggedIn != null)
        {
            OnLoggedIn(this, new LoginEventArgs { Username = username, Password = password, Time = DateTime.Now });
        }
    }
}
