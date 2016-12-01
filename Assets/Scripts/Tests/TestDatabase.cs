using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestDatabase : MonoBehaviour
{
    public Button queryButton;
    public Button closeButton;
    public Text resultText;

	// Use this for initialization
	void Start () {

        Application.logMessageReceived += Application_logMessageReceived;

        queryButton.onClick.AddListener(() =>
        {
            bool result = false;// CopierAR.DBManager.Initialize();
            Debug.Log("Database connection: " + result);
        });

        closeButton.onClick.AddListener(() =>
        {
            //CopierAR.DBManager.Uninitialize();
            Debug.Log("Database connection: Closed");
        });
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        resultText.text = string.Format("{0} | {1} | {2}", condition, stackTrace, type.ToString());
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
        queryButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }
}
