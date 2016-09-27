#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class OtherInfoTest : MonoBehaviour
    {
        public Text infoText;

        void Awake()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            var builder = new StringBuilder();
            // Device info
            builder.AppendLine("ANDROID_ID? : " + AndroidDeviceInfo.GetAndroidId());

            builder.AppendLine("----------- Build class------------");
            builder.AppendLine("DEVICE : " + AndroidDeviceInfo.DEVICE);
            builder.AppendLine("MODEL : " + AndroidDeviceInfo.MODEL);
            builder.AppendLine("PRODUCT : " + AndroidDeviceInfo.PRODUCT);
            builder.AppendLine("MANUFACTURER : " + AndroidDeviceInfo.MANUFACTURER);

            // Build.VERSION
            builder.AppendLine("-----------Build.VERSION class------------");
            builder.AppendLine("BASE_OS : " + AndroidDeviceInfo.BASE_OS);
            builder.AppendLine("CODENAME : " + AndroidDeviceInfo.CODENAME);
            builder.AppendLine("INCREMENTAL : " + AndroidDeviceInfo.INCREMENTAL);
            builder.AppendLine("PREVIEW_SDK_INT : " + AndroidDeviceInfo.PREVIEW_SDK_INT);
            builder.AppendLine("RELEASE : " + AndroidDeviceInfo.RELEASE);
            builder.AppendLine("SDK_INT : " + AndroidDeviceInfo.SDK_INT);
            builder.AppendLine("SECURITY_PATCH : " + AndroidDeviceInfo.SECURITY_PATCH);
            builder.AppendLine("---------------------------");

            builder.AppendLine("Twitter installed? : " + AndroidShare.IsTwitterInstalled());
            builder.AppendLine("Has mail app? : " + AndroidShare.UserHasEmailApp());
            builder.AppendLine("Has sms app? : " + AndroidShare.UserHasSmsApp());
            builder.AppendLine("Has maps app? : " + AndroidMaps.UserHasMapsApp());

            infoText.text = builder.ToString();
        }

    }
}
#endif