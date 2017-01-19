public static class Constants
{
    public static string SAVED_NAME_PREF_KEY = "SAVED_NAME";

    //const string WEBSERVICE_URL = "http://magesgp-001-site1.gtempurl.com/"; // Expired on 2017/01/05
    //const string WEBSERVICE_URL = "http://magesdev-001-site1.itempurl.com/"; // Expiring on 2017/03/17
    const string WEBSERVICE_URL = "http://66.96.194.225:8000/Copier/"; // iOOSH domain


    public const string LOGIN_URL = WEBSERVICE_URL + "Login";
    public const string VALIDATE_CREDS_URL = WEBSERVICE_URL + "ValidateCreds";
    public const string LOCATION_URL = WEBSERVICE_URL + "Location";
    public const string REGISTRATION_URL = WEBSERVICE_URL + "Registration";
    public const string SALES_INFO_URL = WEBSERVICE_URL + "SalesInfo";
}
