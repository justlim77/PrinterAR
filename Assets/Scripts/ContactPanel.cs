using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
using AndroidGoodiesExamples;
#elif UNITY_IOS
// Add iOS libraries
#endif

public class ContactPanel : MonoBehaviour
{
    public string MapLocation;
    public string WebAddress;
    public string EmailAddress;

    public void OnOpenMapAddress()
    {
        if (AndroidGoodiesExampleUtils.IfNotAndroid())
        {
            return;
        }

        AndroidMaps.OpenMapLocation(MapLocation);
    }

    public void OnOpenWebsiteAddress()
    {
        Application.OpenURL(WebAddress);
    }

    public void OnSendEmailClick()
    {
        if (AndroidGoodiesExampleUtils.IfNotAndroid())
        {
            return;
        }

        AndroidShare.SendEmail(new[] { EmailAddress }, "", "");
    }
}
