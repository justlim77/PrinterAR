using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugLog : MonoBehaviour
{
    public Text label;

    private static DebugLog m_instance = null;

	void Awake ()
    {
        if(m_instance == null)
            m_instance = this;
	}

    void Start()
    {
        DebugLog.m_instance.ClearMessages();
    }

    public static void Log(string message)
    {
        DebugLog.m_instance.AddMessage(message);
    }

    public static void Clear()
    {
        DebugLog.m_instance.ClearMessages();
    }

    private void AddMessage(string message)
    {
        label.text = message;
    }

    private void ClearMessages()
    {
        label.text = "";
    }
}
