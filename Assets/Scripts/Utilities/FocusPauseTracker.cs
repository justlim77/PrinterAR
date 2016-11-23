using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FocusPauseTracker : MonoBehaviour
{
    public Image focusImage;
    public Image pauseImage;

    private bool m_isFocused = false;
    private void OnApplicationFocus(bool focus)
    {
        focusImage.color = focus ? Color.green : Color.red;
        m_isFocused = focus;
    }

    private void OnApplicationPause(bool pause)
    {
        pauseImage.color = pause ? Color.red : Color.green;
    }

    private void Update()
    {
        DebugLog.Log("Focused: " + m_isFocused);
    }
}
