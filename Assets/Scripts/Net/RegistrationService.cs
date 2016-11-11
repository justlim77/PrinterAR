/// 
/// =====CRM Integration=====
/// Replace temporary REGISTER_URL value with link to web service
/// Replace temporary RETISTER_SUCCESS value with return value from successful registration request
/// Currently set up to query [dbo.tblRegister] for [CName], [CUserID], [CPwd], [Email] *Currently does not exist*, and [Company]
/// =========================
/// 

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class RegistrationService
    {
        public static string REGISTER_URL = "http://unity-test-server.appspot.com/authentication/register";
        public static string REGISTER_SUCCESS = "register-success";

        public IEnumerator SendRegistrationData(RegistrationData registerData, System.Action<Response> responseHandler)
        {
            Response response = new Response();

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

            // Send registration DB transaction


            Debug.Log("Username and Email valid for registration");
            response.error = false;
            response.message = "Username and Email valid for registration";
            responseHandler(response);
            yield break;

            // Check if user exists, if so, get registration data:
            //RegistrationData _data = new RegistrationData();
            _data = registerData;
            _data = DBManager.GetRegistrationData(_data.CName);
            yield return 0;
            
            /*Response response = new Response();

            Debug.Log(string.Format("Sending registration request to {0}", REGISTER_URL));

            // Create form with username and password fields:
            WWWForm registerForm = new WWWForm();
            registerForm.AddField("CUserID", registerData.CUserID);    // @WebServiceDeveloper Username refers to "CName" or "CUserID"? Do note that there is no field for "First/Last" name in app
            registerForm.AddField("CPwd", registerData.CPwd);
            registerForm.AddField("CEmail", registerData.Email);    // @WebServiceDeveloper Field does not exist in Copier database
            registerForm.AddField("Company", registerData.Company);

            // Sending request:
            WWW httpResponse = new WWW(REGISTER_URL, registerForm);

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

                if (httpResponse.text == REGISTER_SUCCESS)
                {
                    Debug.Log("Registration successful");
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
