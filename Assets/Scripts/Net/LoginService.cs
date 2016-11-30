#define WEBSERVICE

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class LoginService
    {
        const string WEBSERVICE_URL = "http://magesgp-001-site1.gtempurl.com/";
        const string LOGIN_URL = "Login";

        public IEnumerator SendLoginData(LoginData loginData, System.Action<Response> responseHandler)
        {
            Response response = new Response();

#if DIRECT
            // Check if user exists
            Debug.Log("Check for user " + loginData.CName);
            bool userExists = DBManager.CheckUserExists(loginData.CName);

            // If user exists, check password
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
            Debug.Log(string.Format("DB {0}'s {1} vs user {2} PWD", _data.CName, _data.CPwd, loginData.CPwd));
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
#elif WEBSERVICE            
            Debug.Log(string.Format("Sending login request to {0}", WEBSERVICE_URL + LOGIN_URL));

            // Create form with username and password fields:
            WWWForm loginForm = new WWWForm();
            loginForm.AddField("CName", loginData.CName);
            loginForm.AddField("CPwd", loginData.CPwd);

            // Sending request:
            WWW httpResponse = new WWW(WEBSERVICE_URL + LOGIN_URL, loginForm);

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

                if (httpResponse.text == "True")
                {
                    Debug.Log("Login successful");
                    response.error = false;
                    response.responseType = ResponseType.Success;
                    response.message = "Login successful";
                }
                else
                {
                    Debug.Log("User does not exist");
                    response.error = true;
                    response.responseType = ResponseType.InvalidUser;
                    response.message = "User does not exist";
                }
            }           

            // Calling response handler
            responseHandler(response);
#endif
        }
    }
}
