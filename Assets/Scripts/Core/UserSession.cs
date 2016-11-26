﻿using UnityEngine;
using System.Collections;
using CielaSpike;
using System;
using System.Threading;

namespace CopierAR
{
    public class UserSession : MonoBehaviour
    {
        public delegate void LogoutStartedEventHandler(object sender, System.EventArgs args);
        public static event LogoutStartedEventHandler OnLogoutStarted;

        public delegate void LogoutEndedEventHandler(object sender, System.EventArgs args);
        public static event LogoutEndedEventHandler OnLogoutEnded;

        public delegate void InactiveLogoutEventHandler(object sender, System.EventArgs args);
        public static event InactiveLogoutEventHandler OnInactiveLogout;

        private static int m_DISCONNECT_TIMEOUT = 300; // 5 min * 60 sec
        private static bool m_isLoggedIn = false;
        private static bool m_isLoggingOut = false;

        private float m_cachedTimeStamp = 0.0f;
        private float m_loginTime = 0.0f;

        // Cached login data and DateTime
        private LoginData m_loginData;
        private RegistrationData m_registrationData;
        private System.DateTime m_loginDateTime;

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

        public static bool IsLoggedIn()
        {
            return m_isLoggedIn;
        }

        public static bool IsLoggingOut()
        {
            return m_isLoggingOut;
        }

        private void RegistrationView_OnRegistered(object sender, RegistrationEventArgs args)
        {
            //m_isLoggedIn = true;

            //// Cache time stamp
            //m_cachedTimeStamp = 0;

            //// Cache login time
            //m_loginTime = Time.timeSinceLevelLoad;

            //SessionManager.UpdateSName(args.RegistrationData.CName);
            //SessionManager.UpdateLoginTime(args.Time);

            m_registrationData = args.RegistrationData;

            m_loginData = new LoginData();
            m_loginData.CName = m_registrationData.CName;

            m_loginDateTime = args.Time;
        }

        private void UserBar_OnSignedOut(object arg1, string arg2)
        {
            Logout();
        }

        private void ModelViewer_OnModelSelected(object sender, ModelSelectedEventArgs args)
        {

        }

        void OnDestroy()
        {
            // Un-subscribe to relevant events
            LoginView.OnLoggedIn -= LoginView_OnLoggedIn;
            LocationView.OnLocationSelected -= LocationView_OnLocationSelected;
            RegistrationView.OnRegistered -= RegistrationView_OnRegistered;
            ModelViewer.OnModelSelected -= ModelViewer_OnModelSelected;
            UserBar.OnSignedOut -= UserBar_OnSignedOut;
        }

        private void LocationView_OnLocationSelected(object sender, LocationSelectedEventArgs args)
        {
            SessionManager.UpdatePostalCod(args.locationData.code);

            // Migrated from OnLoggedIn
            m_isLoggedIn = true;

            // Cache time stamp
            m_cachedTimeStamp = 0;

            // Cache login time
            m_loginTime = Time.timeSinceLevelLoad;

            SessionManager.UpdateSName(m_loginData.CName);
            SessionManager.UpdateLoginTime(m_loginDateTime);

            // FOR ANDROID: App loses focus when keyboard is pulled up, causing camera lag, force app focus on login
#if UNITY_EDITOR
            // Nothing
#elif UNITY_ANDROID
            OnApplicationPause(false);
            OnApplicationFocus(true);
#endif
        }

        private void LoginView_OnLoggedIn(object sender, LoginEventArgs args)
        {
            m_loginData = args.loginData;
            m_loginDateTime = args.time;

            //SessionManager.UpdateSName(args.loginData.CName);
            //SessionManager.UpdateLoginTime(args.time);
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
            // Wait for logout process
            if (m_isLoggingOut)
                return;

            if (m_isLoggedIn)
            {
#if UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount > 0)
#elif UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
#endif
                {
                    m_cachedTimeStamp = 0;
                }

                m_cachedTimeStamp += Time.deltaTime;

                // TODO: Control rate
                if (m_cachedTimeStamp >= m_DISCONNECT_TIMEOUT)
                {
                    // Auto-logout on inactivty
                    Debug.Log("Logging out due to inactivity");
                    Logout();

                    // Fire OnInactiveLogout event
                    if (OnInactiveLogout != null)
                    {
                        OnInactiveLogout(this, new System.EventArgs());
                    }
                }
            }
        }

        public void Logout()
        {
            StartCoroutine(UserLogout());
        }

        IEnumerator UserLogout()
        {
            m_isLoggingOut = true;

            if (OnLogoutStarted != null)
                OnLogoutStarted(this, new System.EventArgs() { });

            //SessionManager.UpdateDemoDuration(GetDemoDuration());
            ModelsDuraFreq mdf = ModelViewer.GetModelFrequency();
            SessionManager.UpdatePhotoCopierModel(mdf.ModelString);
            SessionManager.UpdateDemoDuration(mdf.DemoDurationString);
            SessionManager.UpdateFrequency(mdf.FrequencyString);

            Task task;
            this.StartCoroutineAsync(ThreadedInsertInfo(), out task);
            yield return StartCoroutine(task.Wait());
            Debug.Log("[User Session Logout State] " + task.State);

            m_isLoggedIn = false;
            m_cachedTimeStamp = 0;

            yield return Ninja.JumpToUnity;
            ModelViewer.Instance.Initialize();
            yield return Ninja.JumpBack;

            if (OnLogoutEnded != null)
                OnLogoutEnded(this, new System.EventArgs() { });

            m_isLoggingOut = false;
        }

        IEnumerator ThreadedInsertInfo()
        {
            yield return SessionManager.InsertSalesInfoData();
        }

        public void LogoutOnQuit()
        {
            if (m_isLoggedIn)
            {
                Debug.Log("User logged in / Inserting sales info entry");
                Logout();
            }
        }

        //private bool m_isFocused = true;
        private void OnApplicationFocus(bool focus)
        {            
            // Focus
            //m_isFocused = focus;
        }

        //private bool m_isPaused = false;
        private void OnApplicationPause(bool pause)
        {
            // Unpause
            //m_isPaused = pause;
        }
    }
}
