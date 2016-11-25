#if UNITY_ANDROID
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
    /// <summary>
    /// Android intent.
    /// </summary>
    class AndroidIntent
    {
        public const string MIMETypeTextPlain = "text/plain";
        public const string MIMETypeMessage = "message/rfc822";

        #region actions
        public const string ACTION_SEND = "android.intent.action.SEND";
        public const string ACTION_SENDTO = "android.intent.action.SENDTO";
        public const string ACTION_VIEW = "android.intent.action.VIEW";

        public const string EXTRA_TITLE = "android.intent.extra.TITLE";
        public const string EXTRA_SUBJECT = "android.intent.extra.SUBJECT";
        public const string EXTRA_TEXT = "android.intent.extra.TEXT";
        public const string EXTRA_EMAIL = "android.intent.extra.EMAIL";
        #endregion

        internal const string IntentClassSignature = "android.content.Intent";
        internal const string PutExtraMethodName = "putExtra";
        internal const string SetActionMethodName = "setAction";
        internal const string SetTypeMethodName = "setType";
        internal const string SetDataMethodName = "setData";
        internal const string SetClassNameMethodName = "setClassName";


        private static AndroidJavaClass _intentClass;
        private readonly AndroidJavaObject _intent;

        public AndroidJavaObject JavaObj
        {
            get
            {
                return _intent;
            }
        }

        public AndroidIntent()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                throw new System.InvalidOperationException("AndroidJavaObject can be created only on Android");
            }

            if (_intentClass == null)
            {
                _intentClass = new AndroidJavaClass(IntentClassSignature);
            }
            _intent = new AndroidJavaObject(IntentClassSignature);
        }

        public AndroidIntent(string action) : this()
        {
            SetAction(action);
        }

        #region put_extra
        public AndroidIntent PutExtra(string name, string value)
        {
            _intent.Call<AndroidJavaObject>(PutExtraMethodName, name, value);
            return this;
        }

        public AndroidIntent PutExtra(string name, string[] values)
        {
            _intent.Call<AndroidJavaObject>(PutExtraMethodName, name, values);
            return this;
        }
        #endregion

        public AndroidIntent SetAction(string action)
        {
            _intent.Call<AndroidJavaObject>(SetActionMethodName, action);
            return this;
        }

        public AndroidIntent SetType(string type)
        {
            _intent.Call<AndroidJavaObject>(SetTypeMethodName, type);
            return this;
        }

        public AndroidIntent SetData(AndroidJavaObject uri)
        {
            _intent.Call<AndroidJavaObject>(SetDataMethodName, uri);
            return this;
        }

        public AndroidIntent SetClassName(string packageName, string className)
        {
            _intent.Call<AndroidJavaObject>(SetClassNameMethodName, packageName, className);
            return this;
        }

        // intent.resolveActivity(getPackageManager()) != null
        public bool ResolveActivity()
        {
            using (AndroidJavaObject pm = AndroidUtils.Activity.Call<AndroidJavaObject>("getPackageManager"))
            {
                try 
                {
                    // This will throw exception if no app is installed that can handle the activity
                    _intent.Call<AndroidJavaObject>("resolveActivity", pm).GetClassSimpleName();
                    return true;
                }
                catch (System.Exception)
                {
                    // There are no activities to handle this intent
                    return false;
                }
            }
        }
    }
}
#endif