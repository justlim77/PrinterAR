using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public static class SessionManager
    {
        static Session _session = null;

        private static Session session
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

        public static bool InsertSalesInfoData()
        {
            if (!UserSession.IsLoggedIn())
            {
                ClearSession();
                return false;
            }

            bool result = DBManager.InsertSalesInfo(session.SalesInfoData);

            string message = result ? "successful" : "failed";
            Debug.Log(string.Format("Inserting sales info data {0}", message));

            // Clear info
            ClearSession();

            return result;
        }

        public static void UpdateSName(string SName)
        {
            session.SalesInfoData.SName = SName;
        }
        public static void UpdatePostalCod(string PostalCod)
        {
            decimal postalCode = System.Convert.ToDecimal(PostalCod);
            session.SalesInfoData.PostalCod = postalCode;
        }
        public static void UpdateLoginTime(System.DateTime LoginTime)
        {
            session.SalesInfoData.LoginTime = LoginTime;
        }
        public static void UpdatePhotoCopierModel(string PhotoCopierModel)
        {
            session.SalesInfoData.PhotoCopierModel = PhotoCopierModel;
        }
        public static void UpdateDemoDuration(string DemoDuration)
        {
            session.SalesInfoData.DemoDuration = DemoDuration;
        }
        public static void UpdateFrequency(string Frequency)
        {
            session.SalesInfoData.Frequency = Frequency;
        }

        public static void ClearSession()
        {
            session.Clear();
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

