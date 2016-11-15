﻿using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class UserSession : MonoBehaviour
    {
        private static int m_DISCONNECT_TIMEOUT = 300; // 5 min * 60 sec

        private float m_cachedTimeStamp = 0.0f;
        private bool m_isLoggedIn = false;
        private float m_loginTime = 0.0f;

        // Use this for initialization
        void Start()
        {
            // Subscribe to relevant events
            LoginView.OnLoggedIn += LoginView_OnLoggedIn;
            LocationView.OnLocationSelected += LocationView_OnLocationSelected;
            RegistrationView.OnRegistered += RegistrationView_OnRegistered;
            ModelViewer.OnModelSelected += ModelViewer_OnModelSelected;
            UserBar.OnSignedOut += UserBar_OnSignedOut;
        }

        private void RegistrationView_OnRegistered(object sender, RegistrationEventArgs args)
        {
            m_isLoggedIn = true;

            // Cache time stamp
            m_cachedTimeStamp = 0;

            // Cache login time
            m_loginTime = Time.timeSinceLevelLoad;

            SessionManager.UpdateSName(args.RegistrationData.CName);
            SessionManager.UpdateLoginTime(args.Time);
        }

        private void UserBar_OnSignedOut(object arg1, string arg2)
        {
            Logout();
        }

        private void ModelViewer_OnModelSelected(object sender, ModelSelectedEventArgs args)
        {
            SessionManager.UpdatePhotoCopierModel(args.ModelFrequency.ModelString);
            SessionManager.UpdateFrequency(args.ModelFrequency.FrequencyString);
        }

        void OnDestroy()
        {
            // Un-subscribe to relevant events
            LoginView.OnLoggedIn -= LoginView_OnLoggedIn;
            LocationView.OnLocationSelected -= LocationView_OnLocationSelected;
            ModelViewer.OnModelSelected -= ModelViewer_OnModelSelected;
            UserBar.OnSignedOut -= UserBar_OnSignedOut;
        }

        private void LocationView_OnLocationSelected(object sender, LocationSelectedEventArgs args)
        {
            SessionManager.UpdatePostalCod(args.locationData.code);
        }

        private void LoginView_OnLoggedIn(object sender, LoginEventArgs args)
        {
            m_isLoggedIn = true;

            // Cache time stamp
            m_cachedTimeStamp = 0;

            // Cache login time
            m_loginTime = Time.timeSinceLevelLoad;

            SessionManager.UpdateSName(args.loginData.CName);
            SessionManager.UpdateLoginTime(args.time);
        }

        private string GetDemoDuration()
        {
            int s = (int)Time.timeSinceLevelLoad - (int)m_loginTime;
            System.TimeSpan t = System.TimeSpan.FromSeconds(s);
            string duration = string.Format("{0:D1}m:{1:D2}s ",
                t.Minutes,
                t.Seconds);
            Debug.Log(duration);
            return duration;
        }

        void Update()
        {
            if (m_isLoggedIn)
            {
                if (Input.touchCount > 0)
                {
                    m_cachedTimeStamp = 0;
                }

                m_cachedTimeStamp += Time.deltaTime;

                if (m_cachedTimeStamp >= m_DISCONNECT_TIMEOUT)
                {
                    // Auto-logout on inactivty
                    Logout();
                }
            }
        }

        public void Logout()
        {
            //SessionManager.UpdateDemoDuration(GetDemoDuration());
            SessionManager.UpdateDemoDuration(ModelViewer.GetModelFrequency().DemoDurationString);
            SessionManager.InsertSalesInfoData();
            m_isLoggedIn = false;
            m_cachedTimeStamp = 0;
        }

        public void LogoutOnQuit()
        {
            if (m_isLoggedIn)
            {
                Debug.Log("User logged in / Inserting sales info entry");
                Logout();
            }
        }
    }
}

