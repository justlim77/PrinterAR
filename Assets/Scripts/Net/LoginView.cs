using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoginEventArgs : EventArgs
{
    public LoginData LoginData;
    public DateTime Time;
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
        //loginButton.onClick.AddListener(Login);
    }

    void OnEnable()
    {
        UserInputField.onEndEdit.AddListener((x) => loginData.username = x);
        UserInputField.onValueChanged.AddListener(delegate { usernameComment.text = ""; });
        PassInputField.onEndEdit.AddListener((x) => loginData.password = x);
        PassInputField.onValueChanged.AddListener(delegate { passwordComment.text = ""; });
    }

    void OnDisable()
    {
        UserInputField.onEndEdit.RemoveAllListeners();
        UserInputField.onValueChanged.RemoveAllListeners();
        PassInputField.onEndEdit.RemoveAllListeners();
        PassInputField.onValueChanged.RemoveAllListeners();
        //loginButton.onClick.RemoveAllListeners();
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

    public void ShowError(Response response)
    {        
        switch (response.responseType)
        {
            case ResponseType.IncorrectPassword:
                passwordComment.text = response.message;
                break;
            case ResponseType.InvalidUserID:
                usernameComment.text = response.message;
                break;            
        }
    }

    public void HideError()
    {
        loginComment.text = "";
    }

    public void Login()
    {
        // Clear error messages
        HideError();

        if (OnLoggedIn != null)
        {
            OnLoggedIn(this, new LoginEventArgs
            {
                LoginData = this.loginData,
                Time = DateTime.Now
            });
        }
    }
}
