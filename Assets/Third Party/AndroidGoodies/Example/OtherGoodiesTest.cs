#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class OtherGoodiesTest : MonoBehaviour
    {
        public void OnShortToastClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidGoodiesMisc.ShowToast("hello short!");
        }

        public void OnLongToastClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidGoodiesMisc.ShowToast("hello long!", AndroidGoodiesMisc.ToastLength.Long);
        }

        public void OnOpenMapClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidMaps.OpenMapLocation(47.6f, -122.3f, 9);
        }

        public void OnOpenMapLabelClick()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidMaps.OpenMapLocationWithLabel(47.6f, -122.3f, "My Label");
        }

        public void OnOpenMapAddress()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidMaps.OpenMapLocation("1st & Pike, Seattle");
        }

        public void OnEnableImmersiveMode()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidGoodiesMisc.EnableImmersiveMode();
        }
    }
}
#endif
