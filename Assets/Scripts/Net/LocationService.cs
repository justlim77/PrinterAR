using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class LocationService
    {
        public IEnumerator SendLocationData(LocationData locationData, System.Action<Response> responseHandler)
        {
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
        }
    }
}

