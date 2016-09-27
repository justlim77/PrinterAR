#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class NativeShareTest : MonoBehaviour
    {
        public bool withChooser = true;

        public string subject;
        public string text;

        public void OnShareClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidShare.ShareText(subject, text);
        }

        public void OnSendEmailClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidShare.SendEmail(new [] { "x@gmail.com", "hello@gmail.com" }, "subj", "body", withChooser);
        }

        public void OnSendSmsClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidShare.SendSms("123123123", "Hello", withChooser);
        }

        public void OnTweetClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidShare.Tweet("hello %  dawdwdawf af afafxx");
        }
    }
}
#endif