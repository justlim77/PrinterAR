﻿namespace CopierAR
{
    public class LoginData
    {
        public string CName = "";
        public string CPwd = "";

        public void Clear()
        {
            CName = "";
            CPwd = "";
        }
    }

    public class RegistrationData
    {
        public int CID = 0;
        public string CName = "";
        public string Company = "";
        public string CPwd = "";
        public string Email = "";

        public void Clear()
        {
            CID = 0;
            CName = "";
            Company = "";
            CPwd = "";
            Email = "";
        }
    }

    public class LocationData
    {
        public string code = "";        // ex. 329685
        public string Postal_Name = ""; // ex. BALESTIER ROAD
        public string Postal_Code = ""; // same as [code]

        public void Clear()
        {
            code = "";
            Postal_Name = "";
            Postal_Code = "";
        }
    }

    public class SalesInfoData
    {
        public int ID = 0;
        public string SName = "";
        public decimal PostalCod = 0;
        public System.DateTime? LoginTime = null;
        public string PhotoCopierModel = "";
        public System.TimeSpan? DemoDuration = null;
        public int Frequency = 0;

        public void Clear()
        {
            ID = 0;
            SName = "";
            PostalCod = 0;
            LoginTime = null;
            PhotoCopierModel = "";
            DemoDuration = null;
            Frequency = 0;
        }
    }
    public enum ResponseType
    {
        Success,
        InvalidUser,
        IncorrectPassword,
        UserAlreadyExists,
        InvalidPostalCode,
        RegistrationFailed,
        FailedToConnect
    }

    public class Response
    {
        public bool error = false;
        public string message = "";
        public ResponseType responseType = ResponseType.Success;

        public Response()
        {
            error = false;
            message = "";
            responseType = ResponseType.Success;
        }

        public Response(bool error, string message, ResponseType responseType)
        {
            this.error = error;
            this.message = message;
            this.responseType = responseType;
        }
    }
}