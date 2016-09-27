using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SignInEventArgs : EventArgs
{
    public string Username;
    public string Password;
    public DateTime Time;
}

public class SigninPanel : MonoBehaviour
{
    public delegate void SignInEventHandler(object sender, SignInEventArgs args);
    public static event SignInEventHandler OnSignedIn;

    public InputField UserInputField;
    public InputField PassInputField;
    public Toggle RememberMeToggle;
    public Button SigninButton;

    private string username = "";
    private string password = "";

    void Start()
    {
        Initialize();
    }

    void OnEnable()
    {
        UserInputField.onEndEdit.AddListener((x) => username = x);
        PassInputField.onEndEdit.AddListener((x) => password = x);
        SigninButton.onClick.AddListener(Signin);
    }

    void OnDisable()
    {
        UserInputField.onEndEdit.RemoveAllListeners();
        PassInputField.onEndEdit.RemoveAllListeners();
        SigninButton.onClick.RemoveAllListeners();
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
        return true;
    }

    public void Signin()
    {
        if (OnSignedIn != null)
        {
            OnSignedIn(this, new SignInEventArgs { Username = username, Password = password, Time = DateTime.Now });
        }
    }
}
