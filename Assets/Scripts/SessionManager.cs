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
                    _session.RegistrationData = new RegistrationData();
                    _session.LocationData = new LocationData();
                    _session.SalesInfoData = new SalesInfoData();
                }

                return _session;
            }
        }

        public static bool InsertSalesInfoData()
        {
            // TODO
            bool result = DBManager.InsertSalesInfo(session.SalesInfoData);

            string message = result ? "successful" : "failed";
            Debug.Log(string.Format("Inserting sales info data {0}", message));

            return result;
        }

        public static void UpdateSName(string SName)
        {
            session.SalesInfoData.SName = SName;
        }
        public static void UpdatePostalCod(decimal PostalCod)
        {
            session.SalesInfoData.PostalCod = PostalCod;
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
    }

    public class Session
    {
        public RegistrationData RegistrationData;
        public LocationData LocationData;
        public SalesInfoData SalesInfoData;
    }
}

