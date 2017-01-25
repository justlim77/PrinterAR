#define WEBSERVICE

using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

namespace CopierAR
{
    public class SalesInfoService
    {
        public IEnumerator SendSalesInfo(SalesInfoData salesData, Action<Response> responseHandler)
        {
            Response response = new Response();

#if DIRECT

#elif WEBSERVICE
            Debug.Log(string.Format("Sending sales info insert request to {0}", Constants.SALES_INFO_URL));

            // JsonUtility unable to serialize DateTime
            //string dateTimeString = salesData.LoginTime.ToString();
            //Debug.Log(dateTimeString);

            //string json = JsonUtility.ToJson(salesData);
            //Debug.Log(JsonUtility.ToJson(salesData,true));

            //byte[] body = Encoding.UTF8.GetBytes(json);

            //Dictionary<string, string> headers = new Dictionary<string, string>();
            //headers.Add("Content-Type", "application/json");

            //WWW www = new WWW(Constants.SALES_INFO_URL, body, headers);

            // Alternative
            // Create form with sales info data
            WWWForm form = new WWWForm();
            form.AddField("SName", salesData.SName);
            form.AddField("PostalCod", salesData.PostalCod.ToString());
            form.AddField("LoginTime", salesData.LoginTime.ToString());
            form.AddField("PhotoCopierModel", salesData.PhotoCopierModel);
            form.AddField("DemoDuration", salesData.DemoDuration.ToString());
            form.AddField("Frequency", salesData.Frequency.ToString());

            // Sending request:
            WWW www = new WWW(Constants.SALES_INFO_URL, form);

            yield return www;

            if (www.error != null)
            {
                Debug.Log(www.error);
                response.error = true;
                response.message = www.error;
                response.responseType = ResponseType.FailedToConnect;
            }
            else
            {
                // Inserting sales info row successful
                response.error = false;
                response.message = www.text;
                response.responseType = ResponseType.Success;
            }

            // Calling response handler:
            responseHandler(response);
#endif
        }
    }
}
