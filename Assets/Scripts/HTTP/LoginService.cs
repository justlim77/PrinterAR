#define WEBSERVICE

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class LoginService
    {
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
            Debug.Log(string.Format("Sending login request to {0}", Constants.LOGIN_URL));

            // Create form with username and password fields:
            WWWForm loginForm = new WWWForm();
            loginForm.AddField("CName", loginData.CName);
            loginForm.AddField("CPwd", loginData.CPwd);

            // Sending request:
            WWW httpResponse = new WWW(Constants.LOGIN_URL, loginForm);

            // Waiting for response:
            yield return httpResponse;

            if (httpResponse.error != null)
            {
                // Encountering error response from server:
                Debug.Log("Error: " + httpResponse.error);
                response.error = true;
                response.message = httpResponse.error;
                response.responseType = ResponseType.FailedToConnect;

                responseHandler(response);
                yield break;    // End
            }
            else
            {
                // Successful response from server:
                Debug.Log("Response received: " + httpResponse.text);

                if (httpResponse.text == "False")
                {
                    Debug.Log("User does not exist");
                    response.error = true;
                    response.responseType = ResponseType.InvalidUser;
                    response.message = "User does not exist";

                    responseHandler(response);
                    yield break;    // End
                }
                // Continue if user exists
                else
                {
                    Debug.Log("User exists");
                    response.error = false;
                    response.responseType = ResponseType.Success;
                    response.message = "User exists";

                    loginForm = new WWWForm();
                    loginForm.AddField("CName", loginData.CName);

                    Debug.Log("Sending validateCreds HTTP POST to " + Constants.VALIDATE_CREDS_URL);
                    httpResponse = new WWW(Constants.VALIDATE_CREDS_URL, loginForm);
                    yield return httpResponse;

                    if (httpResponse.error != null)
                    {
                        // Encountering error response from server:
                        Debug.Log("Error: " + httpResponse.error);
                        response.error = true;
                        response.message = httpResponse.error;
                        response.responseType = ResponseType.FailedToConnect;

                        responseHandler(response);
                        yield break;    // End
                    }
                    else
                    {
                        // Response received
                        Debug.Log("Response received: " + httpResponse.text);

                        RegistrationData rd = JsonUtility.FromJson<RegistrationData>(httpResponse.text);
                        //Debug.Log(string.Format("DB {0}'s {1} vs user {2} PWD", rd.CName, rd.CPwd, loginData.CPwd));
                        if (rd.CPwd == loginData.CPwd)
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
                    }
                }
            }
#endif
        }
    }
}
