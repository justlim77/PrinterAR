using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public static class SessionManager
    {
        private static Session _session = null;
        public static Session Session
        {
            get
            {
                if (_session == null)
                {
                    _session = new Session();
                }

                return _session;
            }
        }

        #region Deprecated methods
        public static bool InsertSalesInfoData()
        {
            if (!UserSession.IsLoggedIn())
            {
                ClearSession();
                return false;
            }

            bool result = false;// DBManager.InsertSalesInfo(Session.SalesInfoData);

            string message = result ? "successful" : "failed";
            Debug.Log(string.Format("Inserting sales info data {0}", message));

            // Clear info
            ClearSession();

            return result;
        }
        #endregion

        public static void UpdateSName(string SName)
        {
            Session.SalesInfoData.SName = SName;
        }
        public static void UpdatePostalCod(string PostalCod)
        {
            decimal postalCode = System.Convert.ToDecimal(PostalCod);
            Session.SalesInfoData.PostalCod = postalCode;
        }
        public static void UpdateLoginTime(System.DateTime LoginTime)
        {
            Session.SalesInfoData.LoginTime = LoginTime;
        }
        public static void UpdatePhotoCopierModel(string PhotoCopierModel)
        {
            Session.SalesInfoData.PhotoCopierModel = PhotoCopierModel;
        }
        public static void UpdateDemoDuration(System.DateTime DemoDuration)
        {
            Session.SalesInfoData.DemoDuration = DemoDuration;
        }
        public static void UpdateFrequency(int Frequency)
        {
            Session.SalesInfoData.Frequency = Frequency;
        }

        public static void ClearSession()
        {
            Session.Clear();
        }
    }

    public class Session
    {
        public LoginData LoginData;
        public RegistrationData RegistrationData;
        public LocationData LocationData;
        public SalesInfoData SalesInfoData;

        public Session()
        {
            LoginData = new LoginData();
            RegistrationData = new RegistrationData();
            LocationData = new LocationData();
            SalesInfoData = new SalesInfoData();
        }

        public void Clear()
        {
            LoginData.Clear();
            RegistrationData.Clear();
            LocationData.Clear();
            SalesInfoData.Clear();
        }
    }
}

