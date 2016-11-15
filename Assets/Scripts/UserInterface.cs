using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

        [Header("User Session")]
        public UserSession userSession;

        [Header("Login")]
        public LoginView loginView;

        [Header("Registration")]
        public RegistrationView registrationView;

        [Header("Location")]
        public LocationView locationView;

        public GameObject AboutUsPanel;
        public GameObject ContactUsPanel;

        [Header("Showcase")]
        public GameObject ShowcasePanel;

        public SidePanel SidePanel;
        public RightPanel RightPanel;

        public UserBar UserBar;

        public float AboutPanelHorizontalPosition = 162.0f;

        public static MenuItem menuItem
        {
            get; private set;
        }

        private CanvasScaler m_CanvasScaler;
        private Vector2 m_InitialPanelPosition = new Vector2();

        private LoginService m_loginService = new LoginService();
        private RegistrationService m_registrationService = new RegistrationService();
        private LocationService m_locationService = new LocationService();

        /// <summary>
        /// Processes login response from database
        /// </summary>
        /// <param name="response"></param>
        void LoginResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Login status: Error {0}, {1}", response.error, response.message));

            if (response.error == false)
            {
                loginView.Login(true);
            }
            else
            {
                loginView.ShowError(response);
            }
        }

        /// <summary>
        /// Processes registration response from database
        /// </summary>
        /// <param name="response"></param>
        void RegistrationResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Registration status: Error {0}, {1}", response.error, response.message));

            // TODO: Register user on database
            if (response.error == false)
            {
                registrationView.Register(true);
            }
            else
            {
                registrationView.ShowError(response);
            }
        }

        /// <summary>
        /// Processes location response from database
        /// </summary>
        /// <param name="response"></param>
        void LocationResponseHandler(Response response)
        {
            // TODO: Implement notification system
            Debug.Log(string.Format("Location status: Error {0}, {1}", response.error, response.message));

            // TODO: Query location validation
            if (response.error == false)
            {
                locationView.SelectLocation(true);
            }
            else
            {
                locationView.ShowError(response);
            }
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

            LoadMenuItem(MenuItem.Welcome);
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

            loginView.loginButton.onClick.AddListener(() =>
            {
                if(loginView.isValid)
                    StartCoroutine(m_loginService.SendLoginData(loginView.loginData, LoginResponseHandler));
            });

            registrationView.registerButton.onClick.AddListener(() =>
            {
                if(registrationView.isValid)
                    StartCoroutine(m_registrationService.SendRegistrationData(registrationView.registrationData, RegistrationResponseHandler));
            });

            locationView.selectButton.onClick.AddListener(() =>
            {
                if (locationView.isValid)
                    StartCoroutine(m_locationService.SendLocationData(locationView.locationData, LocationResponseHandler));
            });
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

        private void SidePanel_OnContactButtonClicked(object arg1, string arg2)
        {
            ContactUsPanel.SetActive(true);
            ContactUsPanel.transform.SetAsLastSibling();
            UserBar.transform.SetAsLastSibling();
            SidePanel.transform.SetAsLastSibling();
        }

        private void SidePanel_OnAboutButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.About);

            contentPanel.anchoredPosition = new Vector2(AboutPanelHorizontalPosition, m_InitialPanelPosition.y);
        }

        private void SidePanel_OnProductButtonClicked(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Showcase);

            RightPanel.Show();
        }

        private void UserBar_OnRegisterPressed(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Register);
            SetMainPanelHorizontal();
        }

        private void UserBar_OnSignInPressed(object arg1, string arg2)
        {
            LoadMenuItem(MenuItem.Sigin);
            SetMainPanelHorizontal();
        }

        private void UserBar_OnSignedOut(object sender, string arg)
        {
            loginView.Initialize();

            LoadMenuItem(MenuItem.Sigin);

            SetMainPanelHorizontal();
        }

        private void LoginView_OnLoggedIn(object sender, LoginEventArgs args)
        {
            UserBar.UpdateName(args.loginData.CName);

            Debug.Log(string.Format("{0} signed in at {1}:{2}", args.loginData.CName, args.time.Hour, args.time.Minute));
            DebugLog.Log(string.Format("{0} signed in at {1}:{2}", args.loginData.CName, args.time.Hour, args.time.Minute));

            LoadMenuItem(MenuItem.Location);
        }
        private void LocationView_OnLocationSelected(object sender, LocationSelectedEventArgs args)
        {
            Debug.Log(string.Format("Valid postal code {0} ({1}) entered", args.locationData.code, args.locationData.Postal_Name));
            DebugLog.Log(string.Format("Valid postal code {0} ({1}) entered", args.locationData.code, args.locationData.Postal_Name));

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
                    SetMainPanelHorizontal(AboutPanelHorizontalPosition);

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
            DBManager.Uninitialize();
        }
    }    
}
