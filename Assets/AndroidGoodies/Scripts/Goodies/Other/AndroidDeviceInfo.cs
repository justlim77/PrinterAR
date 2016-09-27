#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies
{
    public static class AndroidDeviceInfo
    {
        private const string BuildClass = "android.os.Build";
        private const string BuildVersionClass = "android.os.Build$VERSION";

        // https://developer.android.com/reference/android/provider/Settings.Secure.html#ANDROID_ID
        /// <summary>
        /// A 64-bit number (as a hex string) that is randomly generated when the user first sets up the device and should remain constant for the lifetime of the user's device. 
        /// The value may change if a factory reset is performed on the device.
        /// </summary>
        /// <returns>The unique identifier of the device.</returns>
        public static string GetAndroidId()
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return string.Empty;
            }

            try
            {
                using (var objResolver = AndroidUtils.Activity.Call<AndroidJavaObject>("getContentResolver"))
                {
                    using (var clsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                    {
                        string ANDROID_ID = clsSecure.GetStatic<string>("ANDROID_ID");
                        string androidId = clsSecure.CallStatic<string>("getString", objResolver, ANDROID_ID);
                        return androidId;
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region android.os.Build

        /// <summary>
        /// The name of the industrial design.
        /// </summary>
        public static string DEVICE
        {
            get { return GetClassStatic<string>(BuildClass, "DEVICE"); }
        }

        /// <summary>
        /// The end-user-visible name for the end product.
        /// </summary>
        public static string MODEL
        {
            get { return GetClassStatic<string>(BuildClass, "MODEL"); }
        }

        /// <summary>
        /// The name of the overall product.
        /// </summary>
        public static string PRODUCT
        {
            get { return GetClassStatic<string>(BuildClass, "PRODUCT"); }
        }

        /// <summary>
        /// The manufacturer of the product/hardware.
        /// </summary>
        public static string MANUFACTURER
        {
            get { return GetClassStatic<string>(BuildClass, "MANUFACTURER"); }
        }

        #endregion

        #region android.os.Build$VERSION

        /// <summary>
        /// The base OS build the product is based on.
        /// </summary>
        public static string BASE_OS
        {
            get { return GetClassStatic<string>(BuildVersionClass, "BASE_OS"); }
        }

        /// <summary>
        /// The current development codename, or the string "REL" if this is a release build.
        /// </summary>
        public static string CODENAME
        {
            get { return GetClassStatic<string>(BuildVersionClass, "CODENAME"); }
        }

        /// <summary>
        /// The internal value used by the underlying source control to represent this build.
        /// </summary>
        public static string INCREMENTAL
        {
            get { return GetClassStatic<string>(BuildVersionClass, "INCREMENTAL"); }
        }

        /// <summary>
        /// The developer preview revision of a prerelease SDK.
        /// </summary>
        public static int PREVIEW_SDK_INT
        {
            get { return GetClassStatic<int>(BuildVersionClass, "PREVIEW_SDK_INT"); }
        }

        /// <summary>
        /// The user-visible version string.
        /// </summary>
        public static string RELEASE
        {
            get { return GetClassStatic<string>(BuildVersionClass, "RELEASE"); }
        }

        /// <summary>
        /// The user-visible SDK version of the framework; its possible values are defined in Build.VERSION_CODES.
        /// </summary>
        public static int SDK_INT
        {
            get { return GetClassStatic<int>(BuildVersionClass, "SDK_INT"); }
        }

        /// <summary>
        /// The user-visible security patch level.
        /// </summary>
        public static string SECURITY_PATCH
        {
            get { return GetClassStatic<string>(BuildVersionClass, "SECURITY_PATCH"); }
        }

        #endregion

        private static T GetClassStatic<T>(string clazz, string fieldName)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return default(T);
            }

            try
            {
                using (var version = new AndroidJavaClass(clazz))
                {
                    return version.GetStatic<T>(fieldName);
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("Could not retrieve property " + fieldName + ". Check device API level");
                return default(T);
            }
        }
    }
}
#endif