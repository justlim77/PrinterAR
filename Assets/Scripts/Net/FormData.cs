namespace CopierAR
{
    public class LoginData
    {
        public string CUserID = "";
        public string CPwd = "";

        public void Clear()
        {
            CUserID = "";
            CPwd = "";
        }
    }

    public class RegistrationData
    {
        public int CID = 0;
        public string CName = "";
        public string Company = "";
        public string CUserID = "";
        public string CPwd = "";
        public string CopierModel = "";
        public string Frequency = "0";
        public decimal PostalCode = 0;
        public string Email = "";

        public void Clear()
        {
            CID = 0;
            CName = "";
            Company = "";
            CUserID = "";
            CPwd = "";
            CopierModel = "";
            Frequency = "0";
            PostalCode = 0;
            Email = "";
        }
    }

    public class LocationData
    {
        public string code = "";
        public string Postal_Name = "";
        public string Postal_Code = "";

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
        public string DemoDuration = "";

        public void Clear()
        {
            ID = 0;
            SName = "";
            PostalCod = 0;
            LoginTime = null;
            PhotoCopierModel = "";
            DemoDuration = "";
        }
    }
    public enum ResponseType
    {
        None,
        InvalidUserID,
        IncorrectPassword,
        UserAlreadyExists,
        InvalidPostalCode
    }

    public class Response
    {
        public bool error = false;
        public string message = "";
        public ResponseType responseType = ResponseType.None;

        public Response()
        {
            error = false;
            message = "";
            responseType = ResponseType.None;
        }

        public Response(bool error, string message, ResponseType responseType)
        {
            this.error = error;
            this.message = message;
            this.responseType = responseType;
        }
    }
}