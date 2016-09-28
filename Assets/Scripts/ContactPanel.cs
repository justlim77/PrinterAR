using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
using AndroidGoodiesExamples;
#endif

public class ContactPanel : MonoBehaviour
{
    public string MapLocation;
    public string WebAddress;

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

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
