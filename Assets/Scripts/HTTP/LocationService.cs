#define WEBSERVICE

using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class LocationService
    {
        public IEnumerator SendLocationData(LocationData locationData, System.Action<Response> responseHandler)
        {
#if DIRECT
            Response response = new Response();

            // Check if postal code entry
            //bool postalCodeExists = DBManager.CheckPostalExists(locationData.code);
            bool postalCodeExists = DBManager.UpsertPostalCodeData(locationData.code);

            if (!postalCodeExists)
            {
                response.error = true;
                response.message = "Invalid postal code";
                response.responseType = ResponseType.InvalidPostalCode;
            }
            else
            {
                response.error = false;
                response.message = "Valid postal code";
                response.responseType = ResponseType.Success;
            }

            responseHandler(response);

            yield return 0;
#elif WEBSERVICE
            Response response = new Response();

            Debug.Log(string.Format("Sending location HTTP POST request to {0}", Constants.LOCATION_URL));

            // Create form with code field:
            WWWForm locationForm = new WWWForm();
            locationForm.AddField("code", locationData.code);

            // Sending request:
            WWW httpResponse = new WWW(Constants.LOCATION_URL, locationForm);

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

                // If postal code did not exist:
                if (httpResponse.text == "False")
                {
                    response.error = true;
                    response.message = "Inserted new postal code entry";
                    response.responseType = ResponseType.InvalidPostalCode;
                }
                else
                {
                    response.error = false;
                    response.message = "Valid postal code found";
                    response.responseType = ResponseType.Success;
                }

                responseHandler(response);
            }
#endif
        }
    }
}

