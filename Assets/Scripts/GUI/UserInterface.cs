﻿#define WEBSERVICE

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

#if DIRECT
using CielaSpike;
using System.Threading;
#endif

namespace CopierAR
{
    public class UserInterface : MonoBehaviour
    {
        public enum MenuItem
        {
            None,
            Welcome,
            Sigin,
            Register,
            Location,
            About,
            Showcase,
            LifeScale
        }

        public Text Header;
        public GameObject Background;

        public RectTransform contentPanel;
        public GameObject WelcomePanel;

        [Header("Menu item to load on start")]
        public MenuItem StartMenuItem = MenuItem.Welcome;

        [Header("Loading GIF")]
        public CanvasGroup loadingCanvasRenderer;
        public Animator loadingWheelAnimator;

        [Header("User Session")]
        public UserSession userSession;

        [Header("Login")]
        public LoginView loginView;

        [Header("Registration")]
        public RegistrationView registrationView;

        [Header("Location")]
        public LocationView locationView;

        public GameObject AboutUsPanel;
        //public GameObject ContactUsPanel;

        [Header("Showcase")]
        public GameObject ShowcasePanel;

        public SidePanel SidePanel;
        public RightPanel RightPanel;

        public UserBar UserBar;

        public float DefaultHorizontalPosition = -32.0f;
        public float CenterHorizontalPosition = -248.0f;

        public static MenuItem menuItem
        {
            get; private set;
        }

        private CanvasScaler m_CanvasScaler;
        private Vector2 m_InitialPanelPosition = new Vector2();

        private LoginService m_loginService = new LoginService();
        private RegistrationService m_registrationService = new RegistrationService();
        private LocationService m_locationService = new LocationService();

        private Response m_loginResponse;
        private Response m_registrationResponse;
        private Response m_locationResponse;
        /// <summary>
        /// Processes login response from database
        /// </summary>
        /// <param name="response"></param>
        void LoginResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Login status: Error {0}, {1}", response.error, response.message));

            m_loginResponse = response;
            //if (response.error == false)
            //{
            //    loginView.Login(true);
            //}
            //else
            //{
            //    loginView.ShowError(response);
            //}
        }

        /// <summary>
        /// Processes registration response from database
        /// </summary>
        /// <param name="response"></param>
        void RegistrationResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Registration status: Error {0}, {1}", response.error, response.message));

            m_registrationResponse = response;
            // TODO: Register user on database
            //if (response.error == false)
            //{
            //    registrationView.Register(true);
            //}
            //else
            //{
            //    registrationView.ShowError(response);
            //}
        }

        /// <summary>
        /// Processes location response from database
        /// </summary>
        /// <param name="response"></param>
        void LocationResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Location status: Error {0}, {1}", response.error, response.message));

            m_locationResponse = response;
            // TODO: Query location validation
            //if (response.error == false)
            //{
            //    locationView.SelectLocation(true);
            //}
            //else
            //{
            //    locationView.ShowError(response);
            //}
        }

        // Use this for initialization
        void Start()
        {
            //m_loginService = new LoginService();
            //m_registrationService = new RegistrationService();
            //DBManager.Initialize();

            m_CanvasScaler = GetComponent<CanvasScaler>();
            m_CanvasScaler.dynamicPixelsPerUnit = 4;

            m_InitialPanelPosition = contentPanel.anchoredPosition;

            HideLoadingView();

            LoadMenuItem(StartMenuItem);
        }

        void ShowLoadingView()
        {
            loadingCanvasRenderer.transform.SetAsLastSibling();

            loadingCanvasRenderer.alpha = 1;
            loadingCanvasRenderer.blocksRaycasts = true;
        }

        void HideLoadingView()
        {
            loadingCanvasRenderer.alpha = 0;
            loadingCanvasRenderer.blocksRaycasts = false;
        }

        IEnumerator ThreadedLogin()
        {
#if DIRECT
            ShowLoadingView();

            // Threading approach:
            Task task;
            this.StartCoroutineAsync(m_loginService.SendLoginData(loginView.LoginData,
                LoginResponseHandler), out task);

            yield return StartCoroutine(task.Wait());

            Debug.Log("[Threaded Login State] " + task.State);

            //yield return Ninja.JumpToUnity;

            HideLoadingView();

            if (m_loginResponse.error == false)
            {
                loginView.Login(true);
            }
            else
            {
                loginView.ShowError(m_loginResponse);
            }

            yield return Ninja.JumpBack;
#elif WEBSERVICE
            ShowLoadingView();

            // HTTP approach:
            yield return StartCoroutine(m_loginService.SendLoginData(loginView.LoginData,
                LoginResponseHandler));

            HideLoadingView();

            if (m_loginResponse.error == false)
            {
                loginView.Login(true);
            }
            else
            {
                loginView.ShowError(m_loginResponse);
            }
#endif
        }

        IEnumerator ThreadedRegistration()
        {
#if DIRECT
            ShowLoadingView();

            // Threading approach:
            Task task;
            this.StartCoroutineAsync(m_registrationService.SendRegistrationData(registrationView.registrationData,
                RegistrationResponseHandler), out task);
            yield return StartCoroutine(task.Wait());

            Debug.Log("[Threaded Registration State] " + task.State);

            yield return Ninja.JumpToUnity;

            HideLoadingView();

            if (m_registrationResponse.error == false)
            {
                registrationView.Register(true);
            }
            else
            {
                registrationView.ShowError(m_registrationResponse);
            }

            yield return Ninja.JumpBack;
#elif WEBSERVICE
            ShowLoadingView();

            yield return StartCoroutine(m_registrationService.SendRegistrationData(registrationView.registrationData,
                RegistrationResponseHandler));

            HideLoadingView();

            if (m_registrationResponse.error == false)
            {
                registrationView.Register(true);
            }
            else
            {
                registrationView.ShowError(m_registrationResponse);
            }
#endif
        }

        IEnumerator ThreadedLocation()
        {
#if DIRECT
            ShowLoadingView();

            // Threading approach:
            Task task;
            this.StartCoroutineAsync(m_locationService.SendLocationData(locationView.locationData,
                LocationResponseHandler), out task);
            yield return StartCoroutine(task.Wait());

            Debug.Log("[Threaded Location State] " + task.State);

            yield return Ninja.JumpToUnity;

            HideLoadingView();

            if (m_locationResponse.error == false)
            {
                locationView.SelectLocation(true);
            }
            else
            {
                locationView.ShowError(m_locationResponse);
            }

            yield return Ninja.JumpBack;
#elif WEBSERVICE
            ShowLoadingView();

            // Location HTTP POST approach:
            yield return StartCoroutine(m_locationService.SendLocationData(locationView.locationData,
                LocationResponseHandler));

            HideLoadingView();

            if (m_locationResponse.error == false)
            {
                locationView.SelectLocation(true);
            }
            else
            {
                locationView.ShowError(m_locationResponse);
            }
#endif
        }

        IEnumerator ThreadedInsert()
        {
#if DIRECT
            ShowLoadingView();

            // Threading approach:
            Task task;
            this.StartCoroutineAsync(m_locationService.SendLocationData(locationView.locationData,
                LocationResponseHandler), out task);
            yield return StartCoroutine(task.Wait());

            Debug.Log("[State]" + task.State);

            yield return Ninja.JumpToUnity;

            HideLoadingView();

            if (m_locationResponse.error == false)
            {
                locationView.SelectLocation(true);
            }
            else
            {
                locationView.ShowError(m_locationResponse);
            }

            yield return Ninja.JumpBack;
#elif WEBSERVICE
            yield return null;
#endif
        }

        void OnEnable()
        {
            LoginView.OnLoggedIn += LoginView_OnLoggedIn;
            LocationView.OnLocationSelected += LocationView_OnLocationSelected;
            RegistrationView.OnRegistered += RegistrationView_OnRegistered;
            UserBar.OnSignedOut += UserBar_OnSignedOut;
            UserBar.OnSignInPressed += UserBar_OnSignInPressed;
            UserBar.OnRegisterPressed += UserBar_OnRegisterPressed;
            SidePanel.OnAboutButtonClicked += SidePanel_OnAboutButtonClicked;
            SidePanel.OnProductButtonClicked += SidePanel_OnProductButtonClicked;
            SidePanel.OnShowcaseButtonClicked += SidePanel_OnShowcaseButtonClicked;
            SidePanel.OnLifeScaleButtonClicked += SidePanel_OnLifeScaleButtonClicked;
            UserSession.OnInactiveLogout += UserSession_OnInactiveLogout;
            UserSession.OnLogoutStarted += UserSession_OnLogoutStarted;
            UserSession.OnLogoutEnded += UserSession_OnLogoutEnded;

            loginView.loginButton.onClick.AddListener(() =>
            {
                if (loginView.isValid)
                {
                    //StartCoroutine(m_loginService.SendLoginData(loginView.LoginData, LoginResponseHandler));

                    // Threading approach:
                    StartCoroutine(ThreadedLogin());
                }
            });

            registrationView.registerButton.onClick.AddListener(() =>
            {
                if (registrationView.isValid)
                {
                    //StartCoroutine(m_registrationService.SendRegistrationData(registrationView.registrationData, RegistrationResponseHandler));

                    // Threading approach:
                    StartCoroutine(ThreadedRegistration());
                }
            });

            locationView.selectButton.onClick.AddListener(() =>
            {
                if (locationView.isValid)
                {
                    //StartCoroutine(m_locationService.SendLocationData(locationView.locationData, LocationResponseHandler));

                    // Threaded location approach:
                    StartCoroutine(ThreadedLocation());
                }
            });
        }

        private void UserSession_OnLogoutStarted(object sender, EventArgs args)
        {
            ShowLoadingView();
        }

        private void UserSession_OnLogoutEnded(object sender, EventArgs args)
        {
            HideLoadingView();

            loginView.Initialize();

            LoadMenuItem(MenuItem.Sigin);

            SetMainPanelHorizontal(CenterHorizontalPosition);
        }

        void OnDisable()
        {
            LoginView.OnLoggedIn -= LoginView_OnLoggedIn;
            LocationView.OnLocationSelected -= LocationView_OnLocationSelected;
            RegistrationView.OnRegistered -= RegistrationView_OnRegistered;
            UserBar.OnSignedOut -= UserBar_OnSignedOut;
            UserBar.OnSignInPressed -= UserBar_OnSignInPressed;
            UserBar.OnRegisterPressed -= UserBar_OnRegisterPressed;
            SidePanel.OnAboutButtonClicked -= SidePanel_OnAboutButtonClicked;
            SidePanel.OnProductButtonClicked -= SidePanel_OnProductButtonClicked;
            SidePanel.OnShowcaseButtonClicked -= SidePanel_OnShowcaseButtonClicked;
            SidePanel.OnLifeScaleButtonClicked -= SidePanel_OnLifeScaleButtonClicked;
            UserSession.OnInactiveLogout -= UserSession_OnInactiveLogout;
            UserSession.OnLogoutStarted -= UserSession_OnLogoutStarted;
            UserSession.OnLogoutEnded -= UserSession_OnLogoutEnded;

            loginView.loginButton.onClick.RemoveAllListeners();
            registrationView.registerButton.onClick.RemoveAllListeners();
            locationView.selectButton.onClick.RemoveAllListeners();
        }

        void Update()
        {
            if (UserInterface.menuItem != MenuItem.LifeScale)
            {
                Background.SetActive(true);
            }
            else
            {
                Background.SetActive(false);
            }

            // Debug login
            if (Input.GetKeyDown(KeyCode.Home))
            {
                LoadMenuItem(MenuItem.About);
            }
        }
        private void UserSession_OnInactiveLogout(object sender, System.EventArgs args)
        {
            UserBar_OnSignedOut(this, "Signed out");
            UserBar.SignoutWithoutEvent();
        }

        private void SidePanel_OnLifeScaleButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.LifeScale);
        }

        private void SidePanel_OnShowcaseButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Showcase);
        }

        private void SidePanel_OnAboutButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.About);

            SetMainPanelHorizontal(DefaultHorizontalPosition);
        }

        private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Showcase);

            RightPanel.Show();
        }

        private void UserBar_OnRegisterPressed(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Register);
            SetMainPanelHorizontal(CenterHorizontalPosition);
        }

        private void UserBar_OnSignInPressed(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Sigin);
            SetMainPanelHorizontal(CenterHorizontalPosition);
        }

        private void UserBar_OnSignedOut(object sender, string arg)
        {
            //if (!UserSession.IsLoggedIn())
            //{
            //    loginView.Initialize();

            //    LoadMenuItem(MenuItem.Sigin);

            //    SetMainPanelHorizontal();
            //}
        }

        private void LoginView_OnLoggedIn(object sender, LoginEventArgs args)
        {
            UserBar.UpdateName(args.loginData.CName);

            Debug.Log(string.Format("{0} signed in at {1}:{2}", args.loginData.CName, args.time.Hour, args.time.Minute));
            //DebugLog.Log(string.Format("{0} signed in at {1}:{2}", args.loginData.CName, args.time.Hour, args.time.Minute));

            LoadMenuItem(MenuItem.Location);
        }
        private void LocationView_OnLocationSelected(object sender, LocationSelectedEventArgs args)
        {
            Debug.Log(string.Format("Valid postal code {0} ({1}) entered", args.locationData.code, args.locationData.Postal_Name));
            //DebugLog.Log(string.Format("Valid postal code {0} ({1}) entered", args.locationData.code, args.locationData.Postal_Name));

            LoadMenuItem(MenuItem.About);
        }

        private void RegistrationView_OnRegistered(object sender, RegistrationEventArgs args)
        {
            UserBar.UpdateName(args.RegistrationData.CName);

            LoadMenuItem(MenuItem.Location);
        }

        private void SetMainPanelHorizontal(float x = 0)
        {
            contentPanel.anchoredPosition = new Vector2(x, m_InitialPanelPosition.y);
        }

        private void LoadMenuItem(MenuItem menuItem)
        {
            switch (menuItem)
            {
                case MenuItem.None:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(false);
                    loginView.gameObject.SetActive(false);
                    registrationView.gameObject.SetActive(false);
                    locationView.gameObject.SetActive(false);
                    AboutUsPanel.SetActive(false);
                    SidePanel.gameObject.SetActive(false);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);
                    break;
                case MenuItem.Welcome:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(true);
                    loginView.gameObject.SetActive(false);
                    registrationView.gameObject.SetActive(false);
                    locationView.gameObject.SetActive(false);
                    AboutUsPanel.SetActive(false);
                    SidePanel.gameObject.SetActive(false);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);

                    Header.text = "Welcome";
                    break;
                case MenuItem.Sigin:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(false);
                    loginView.gameObject.SetActive(true);
                    registrationView.gameObject.SetActive(false);
                    locationView.gameObject.SetActive(false);
                    AboutUsPanel.SetActive(false);
                    SidePanel.gameObject.SetActive(false);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);

                    Header.text = "Sign in";
                    break;
                case MenuItem.Register:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(false);
                    loginView.gameObject.SetActive(false);
                    registrationView.gameObject.SetActive(true);
                    locationView.gameObject.SetActive(false);
                    AboutUsPanel.SetActive(false);
                    SidePanel.gameObject.SetActive(false);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);

                    Header.text = "Register";
                    break;
                case MenuItem.Location:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(false);
                    loginView.gameObject.SetActive(false);
                    registrationView.gameObject.SetActive(false);
                    locationView.gameObject.SetActive(true);
                    AboutUsPanel.SetActive(false);
                    SidePanel.gameObject.SetActive(false);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);

                    Header.text = "Location";
                    break;
                case MenuItem.About:
                    contentPanel.gameObject.SetActive(true);

                    WelcomePanel.SetActive(false);
                    loginView.gameObject.SetActive(false);
                    registrationView.gameObject.SetActive(false);
                    locationView.gameObject.SetActive(false);
                    AboutUsPanel.SetActive(true);
                    SidePanel.gameObject.SetActive(true);
                    RightPanel.Hide();
                    ShowcasePanel.SetActive(false);

                    SidePanel.Initialize();
                    SetMainPanelHorizontal(DefaultHorizontalPosition);

                    Header.text = "About Us";
                    break;
                case MenuItem.Showcase:
                    contentPanel.gameObject.SetActive(false);

                    RightPanel.SetProductMode(ProductMode.Showcase);

                    ShowcasePanel.SetActive(true);

                    break;
                case MenuItem.LifeScale:
                    contentPanel.gameObject.SetActive(false);

                    RightPanel.SetProductMode(ProductMode.LifeScale);

                    ShowcasePanel.SetActive(false);
                    break;
            }

            UserInterface.menuItem = menuItem;
        }

        public static bool IsDemoing()
        {
            return (menuItem == MenuItem.LifeScale || menuItem == MenuItem.Showcase);
        }

        void OnApplicationQuit()
        {
            // Log out user session (if logged in)
            userSession.LogoutOnQuit();

            // Close database connection if not yet done so
            //DBManager.Uninitialize();
        }
    }    
}
