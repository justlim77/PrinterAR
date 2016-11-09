/// 
/// =====CRM Integration=====
/// Replace temporary LOGIN_URL value with link to web service
/// Replace temporary LOGIN_SUCCESS value with return value from successful login request
/// Currently set up to query [dbo.tblRegister] for [CUserID] and [CPwd]
/// =========================
/// 

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class LoginService
    {
        public static string LOGIN_URL = "http://unity-test-server.appspot.com/authentication/login";   // @WebServiceDeveloper Please set this to the web service URL
        public static string LOGIN_SUCCESS = "login-success";   // @WebServiceDeveloper Please set return value from successful login request

        public IEnumerator SendLoginData(LoginData loginData, System.Action<Response> responseHandler)
        {
            Response response = new Response();

            // Check if user exists
            bool userExists = DBManager.CheckUserExists(loginData.CName);

            if (!userExists)
            {
                response.error = true;
                response.message = "User does not exist";
                response.responseType = ResponseType.InvalidUser;

                // Calling response handler
                responseHandler(response);

                yield break;
            }
            
            // If user exists, check password
            RegistrationData _data = DBManager.GetRegistrationData(loginData.CName);
            
            if (_data.CPwd == loginData.CPwd)
            {
                response.error = false;
                response.message = "Login success";
                response.responseType = ResponseType.Success;
            }
            else
            {
                // Wrong password
                response.error = true;
                response.message = "Incorrect password";
                response.responseType = ResponseType.IncorrectPassword;
            }            

            // Calling response handler
            responseHandler(response);

            yield break;

            /*
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

                if (httpResponse.text == LOGIN_SUCCESS)
                {
                    Debug.Log("Login successful");
                    response.error = false;
                    response.message = "";
                }
            }           

            // Calling response handler
            responseHandler(response);
            */
        }
    }
}
