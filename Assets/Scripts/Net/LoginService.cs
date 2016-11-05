/// 
/// =====CRM Integration=====
/// Replace temporary LOGIN_URL value with link to web service
/// Replace temporary LOGIN_SUCCESS value with return value from successful login request
/// Currently set up to query [dbo.tblRegister] for [CUserID] and [CPwd]
/// =========================
/// 

using UnityEngine;
using System.Collections;

public class LoginService
{
    public static string LOGIN_URL = "http://unity-test-server.appspot.com/authentication/login";   // @WebServiceDeveloper Please set this to the web service URL
    public static string LOGIN_SUCCESS = "login-success";   // @WebServiceDeveloper Please set return value from successful login request

    public IEnumerator SendLoginData(LoginData loginData, System.Action<Response> responseHandler)
    {
        Response response = new Response();

        Debug.Log(string.Format("Sending login request to {0}", LOGIN_URL));

        // Create form with username and password fields:
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("CUserID", loginData.username);
        loginForm.AddField("CPwd", loginData.password);

        // Sending request:
        WWW httpResponse = new WWW(LOGIN_URL, loginForm);

        // Waiting for response:
        yield return httpResponse;

        if (httpResponse.error != null)
        {
            // Encountering error response from server:
            Debug.Log("Error: " + httpResponse.error);
            response.error = true;
            response.message = httpResponse.error;
        }
        else
        {
            // Successful response from server:
            Debug.Log("Response received: " + httpResponse.text);

            if(httpResponse.text == LOGIN_SUCCESS)
            {
                Debug.Log("Login successful");
                response.error = false;
                response.message = "";
            }
        }

        // Calling response handler
        responseHandler(response);
    }
}
