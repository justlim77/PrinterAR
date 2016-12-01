#define WEBSERVICE

using UnityEngine;
using System.Collections;
using System;

namespace CopierAR
{
    public class RegistrationService
    {
        public IEnumerator SendRegistrationData(RegistrationData registerData, Action<Response> responseHandler)
        {
            Response response = new Response();

#if DIRECT
            // Fetch for existing database entry using CUserID
            RegistrationData _data = DBManager.GetRegistrationData(registerData.CName);

            // Cross-ref database to check for existing username (CUserID)
            if (_data.CName == registerData.CName)
            {
                Debug.Log("Username already exists");
                response.error = true;
                response.message = "Username already exists";
                responseHandler(response);
                yield break;
            }

            // Cross-ref database to check for existing email (Email)
            if (_data.Email == registerData.Email)
            {
                Debug.Log("Email already exists");
                response.error = true;
                response.message = "Email already exists";
                responseHandler(response);
                yield break;
            }

            Debug.Log("Username and Email valid for registration\nCreating user...");

            // Send registration DB transaction
            bool userCreated = DBManager.CreateUser(registerData);

            yield return new WaitForEndOfFrame();

            if (userCreated)
            {
                response.message = "Registration successful!";
                response.responseType = ResponseType.Success;
            }
            else
            {
                response.message = "Registration failed.";
                response.responseType = ResponseType.RegistrationFailed;
            }
            response.error = !userCreated;
            responseHandler(response);
            yield break;
#elif WEBSERVICE
            Debug.Log(string.Format("Sending registration request to {0}", Constants.REGISTRATION_URL));

            // Create form with username and password fields:
            WWWForm registerForm = new WWWForm();
            registerForm.AddField("CName", registerData.CName);
            registerForm.AddField("Company", registerData.Company);
            registerForm.AddField("CPwd", registerData.CPwd);
            registerForm.AddField("Email", registerData.Email); 

            // Sending request:
            WWW httpResponse = new WWW(Constants.REGISTRATION_URL, registerForm);

            // Waiting for response:
            yield return httpResponse;

            if (httpResponse.error != null)
            {
                // Encountering error response from server:
                Debug.Log("Registration encountered error: " + httpResponse.error);
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

                // If user created returns true:
                if (httpResponse.text == "True")
                {
                    response.message = "Registration successful!";
                    response.responseType = ResponseType.Success;
                }
                else
                {
                    response.message = "Registration failed.";
                    response.responseType = ResponseType.RegistrationFailed;
                }

                response.error = !bool.Parse(httpResponse.text);
            }

            // Calling response handler
            responseHandler(response);
#endif
        }
    }
}
