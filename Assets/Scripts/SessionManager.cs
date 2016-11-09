using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public static class SessionManager
    {
        static Session _Session = null;

        public static Session GetSession()
        {
            if (_Session == null)
            {
                _Session = new Session();
            }

            return _Session;
        }

        public static bool CommitSalesInfoData()
        {
            // TODO
            return true;
        }
    }

    public class Session
    {
        public RegistrationData RegistrationData;
        public LocationData LocationData;
        public SalesInfoData SalesInfoData;
    }
}

