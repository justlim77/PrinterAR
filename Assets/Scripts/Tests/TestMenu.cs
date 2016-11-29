using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestMenu : MonoBehaviour
{
    public const string const_scene_main = "CopierAR-0-Main";
    public const string const_test_scene_db = "CopierAR-Test-0-Database";
    public const string const_test_scene_tracker= "CopierAR-Test-1-ImageTracking";

    public bool ShowDebug = false;

    public Button MainButton;
    public Button TestDBButton;
    public Button TestTrackingButton;

    // Use this for initialization
    void Start ()
    {
        MainButton.onClick.AddListener(delegate { LoadScene(const_scene_main); });
        TestDBButton.onClick.AddListener(delegate { LoadScene(const_test_scene_db); });
        TestTrackingButton.onClick.AddListener(delegate { LoadScene(const_test_scene_tracker); });

        gameObject.SetActive(ShowDebug);
    }

    void OnDestroy()
    {
        MainButton.onClick.RemoveAllListeners();
        TestDBButton.onClick.RemoveAllListeners();
        TestTrackingButton.onClick.RemoveAllListeners();
    }

    public void LoadScene(string scene)
    {
        #if (UNITY_5_2 || UNITY_5_1 || UNITY_5_0)
        Application.LoadLevel(scene);
        #else // UNITY_5_3 or above
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        #endif
    }
}
