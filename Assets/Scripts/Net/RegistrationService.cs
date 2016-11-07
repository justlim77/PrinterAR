/// 
/// =====CRM Integration=====
/// Replace temporary REGISTER_URL value with link to web service
/// Replace temporary RETISTER_SUCCESS value with return value from successful registration request
/// Currently set up to query [dbo.tblRegister] for [CUserID], [CPwd], [CEmail] *Currently does not exist*, and [Company]
/// =========================
/// 

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class RegistrationService
    {
        public static string REGISTER_URL = "http://unity-test-server.appspot.com/authentication/register";   // @WebServiceDeveloper Please set this to the web service registration URL
        public static string REGISTER_SUCCESS = "register-success";   // @WebServiceDeveloper Please set return value from successful register request

        public IEnumerator SendRegistrationData(RegistrationData registerData, System.Action<Response> responseHandler)
        {
            // Check for missing fields:
            if (registerData.CName == "")
            {

            }

            // Check if user exists, if so, get registration data:
            RegistrationData _data = new RegistrationData();
            _data = registerData;
            _data = DBManager.GetRegistrationData(_data.CUserID);
            Debug.Log(_data.CopierModel);
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
