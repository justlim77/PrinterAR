#if UNITY_ANDROID
using UnityEngine;

namespace AndroidGoodiesExamples
{
    public static class AndroidGoodiesExampleUtils
    {
        public static bool IfNotAndroid()
        {
            bool isAndroid = Application.platform == RuntimePlatform.Android;
            if (!isAndroid)
            {
                Debug.LogWarning("You must run the demo on Android device or emulator to work");
            }
            return !isAndroid;
        }
    }
}
#endif