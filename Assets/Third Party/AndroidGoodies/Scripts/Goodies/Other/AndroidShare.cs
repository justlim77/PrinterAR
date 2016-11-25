#if UNITY_ANDROID
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
    public static class AndroidShare
    {
        /// <summary>
        /// Shares the text using default Android intent.
        /// </summary>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="withChooser">If set to <c>true</c> with chooser.</param>
        /// <param name="chooserTitle">Chooser title.</param>
        public static void ShareText(string subject, string body, bool withChooser = true, string chooserTitle = "Share via...")
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return; }

            var intent = new AndroidIntent()
                .SetAction(AndroidIntent.ACTION_SEND)
                .SetType(AndroidIntent.MIMETypeTextPlain);
            intent.PutExtra(AndroidIntent.EXTRA_SUBJECT, subject);
            intent.PutExtra(AndroidIntent.EXTRA_TEXT, body);

            if (withChooser)
            {
                AndroidUtils.StartActivityWithChooser(intent.JavaObj, chooserTitle);
            }
            else
            {
                AndroidUtils.StartActivity(intent.JavaObj);
            }
        }

        /// <summary>
        /// Checks if user has any app that can handle SMS intent
        /// </summary>
        /// <returns><c>true</c>, if user has any SMS app installed, <c>false</c> otherwise.</returns>
        public static bool UserHasSmsApp()
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return false; }

            return CreateSmsIntent("123123123", "dummy").ResolveActivity();
        }

        private const string SmsUriFormat = "smsto:{0}";

        /// <summary>
        /// Sends the sms using Android intent.
        /// </summary>
        /// <param name="phoneNumber">Phone number.</param>
        /// <param name="message">Message.</param>
        /// <param name="withChooser">If set to <c>true</c> with chooser.</param>
        /// <param name="chooserTitle">Chooser title.</param>
        public static void SendSms(string phoneNumber, string message, bool withChooser = true, string chooserTitle = "Send SMS...")
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return; }

            var intent = CreateSmsIntent(phoneNumber, message);
            if (withChooser)
            {
                AndroidUtils.StartActivityWithChooser(intent.JavaObj, chooserTitle);
            }
            else
            {
                AndroidUtils.StartActivity(intent.JavaObj);
            }
        }

        private static AndroidIntent CreateSmsIntent(string phoneNumber, string message)
        {
            var uri = AndroidUtils.UriParse(string.Format(SmsUriFormat, phoneNumber));
            return new AndroidIntent(AndroidIntent.ACTION_SEND)
                .SetType(AndroidIntent.MIMETypeTextPlain)
                .SetData(uri)
                .PutExtra("sms_body", message);
        }

        /// <summary>
        /// Checks if the user has any email app installed.
        /// </summary>
        /// <returns><c>true</c>, if the user has any email app installed, <c>false</c> otherwise.</returns>
        public static bool UserHasEmailApp()
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return false; }

            return CreateEmailIntent(new [] { "dummy@gmail.com" }, "dummy", "dummy").ResolveActivity();
        }

        /// <summary>
        /// Sends the email using Android intent.
        /// </summary>
        /// <param name="recipients">Recipient email addresses.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Body of email.</param>
        /// <param name="withChooser">If set to <c>true</c> with chooser.</param>
        /// <param name="chooserTitle">Chooser title.</param>
        public static void SendEmail(string[] recipients, string subject, string body, bool withChooser = true, string chooserTitle = "Send mail...")
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return; }

            var intent = CreateEmailIntent(recipients, subject, body);
            if (withChooser)
            {
                AndroidUtils.StartActivityWithChooser(intent.JavaObj, chooserTitle);
            }
            else
            {
                AndroidUtils.StartActivity(intent.JavaObj);
            }
        }

        private static AndroidIntent CreateEmailIntent(string[] recipients, string subject, string body)
        {
            var uri = AndroidUtils.UriParse("mailto:");
            return new AndroidIntent()
                .SetAction(AndroidIntent.ACTION_SENDTO)
                .SetData(uri)
                .PutExtra(AndroidIntent.EXTRA_EMAIL, recipients)
                .PutExtra(AndroidIntent.EXTRA_SUBJECT, subject)
                .PutExtra(AndroidIntent.EXTRA_TEXT, body);
        }

        // TWITTER
        private const string TwitterPackage = "com.twitter.android";
        private const string TweetActivity = "com.twitter.android.composer.ComposerActivity";

        /// <summary>
        /// Determines if twitter is installed.
        /// </summary>
        /// <returns><c>true</c> if twitter is installed; otherwise, <c>false</c>.</returns>
        public static bool IsTwitterInstalled()
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return false; }

            return AndroidUtils.IsPackageInstalled(TwitterPackage);
        }

        /// <summary>
        /// Tweet the specified text. Will fallback to browser if official twitter app is not installed.
        /// </summary>
        /// <param name="tweet">Text to tweet.</param>
        public static void Tweet(string tweet)
        {
            if (AndroidUtils.IsNotAndroidCheck()) { return; }

            if (IsTwitterInstalled())
            {
                var intent = new AndroidIntent(AndroidIntent.ACTION_SEND)
                    .SetType(AndroidIntent.MIMETypeTextPlain)
                    .PutExtra(AndroidIntent.EXTRA_TEXT, tweet)
                    .SetClassName(TwitterPackage, TweetActivity);

                AndroidUtils.StartActivity(intent.JavaObj);
            }
            else
            {
                Application.OpenURL("https://twitter.com/intent/tweet?text=" + WWW.EscapeURL(tweet));
            }
        }
    }
}
#endif