#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies
{
    public static class AndroidUtils
    {
        private static AndroidJavaObject _activity;

        public static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _activity;
            }
        }

        public static AndroidJavaObject ActivityDecorView
        {
            get
            {
                return Activity.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
            }
        }

        public static void RunOnUiThread(Action action)
        {
            Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
        }

        public static AndroidJavaObject UriParse(string uriString)
        {
            using (var uriClass = new AndroidJavaClass("android.net.Uri"))
            {
                return uriClass.CallStatic<AndroidJavaObject>("parse", uriString);
            }
        }

        public static void StartActivity(AndroidJavaObject intent)
        {
            try
            {
                Activity.Call("startActivity", intent);
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogError("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
            }
        }

        public static void StartActivityWithChooser(AndroidJavaObject intent, string chooserTitle)
        {
            try
            {
                AndroidJavaObject jChooser = intent.CallStatic<AndroidJavaObject>("createChooser", intent, chooserTitle);
                Activity.Call("startActivity", jChooser);
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogError("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
            }
        }

        /// <summary>
        /// Check if the application with the provided package is installed on device
        /// </summary>
        /// <returns><c>true</c> if application with this package is installed; otherwise, <c>false</c>.</returns>
        public static bool IsPackageInstalled(string package)
        {
            using (AndroidJavaObject pm = Activity.Call<AndroidJavaObject>("getPackageManager"))
            {
                try
                {
                    const int GET_ACTIVITIES = 1;
                    pm.Call<AndroidJavaObject>("getPackageInfo", package, GET_ACTIVITIES);
                    return true;
                }
                catch (AndroidJavaException e)
                {
                    Debug.LogWarning(e);
                    return false;
                }
            }
        }

        public static bool IsNotAndroidCheck()
        {
            bool isAndroid = Application.platform == RuntimePlatform.Android;

            if (isAndroid)
            {
                GoodiesSceneHelper.Init();
            }

            return !isAndroid;
        }

        #region extension_methods

        public static string GetClassName(this AndroidJavaObject ajo)
        {
            return ajo.GetJavaClass().Call<string>("getName");
        }

        public static string GetClassSimpleName(this AndroidJavaObject ajo)
        {
            return ajo.GetJavaClass().Call<string>("getSimpleName");
        }

        public static AndroidJavaObject GetJavaClass(this AndroidJavaObject ajo)
        {
            return ajo.Call<AndroidJavaObject>("getClass");
        }

        public static string JavaToString(this AndroidJavaObject ajo)
        {
            return ajo.Call<string>("toString");
        }

        #endregion
    }
}
#endif