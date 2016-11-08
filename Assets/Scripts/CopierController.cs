using UnityEngine;
using System.Collections;

public class CopierController : MonoBehaviour
{
    public Animator scannerAnimator;
    public Animator panelAnimator;
    public Animator tonerAnimator;
    public Animator sideTrayAnimator;
    public Animator paperTrayAnimator;

    // Default animator states
    public CopierAnimation scannerState = CopierAnimation.ScannerDown;
    public CopierAnimation panelState = CopierAnimation.PanelDown;
    public CopierAnimation tonerState = CopierAnimation.TonerClose;
    public CopierAnimation sideTrayState = CopierAnimation.SideTrayClose;
    public CopierAnimation paperTrayState = CopierAnimation.PaperTrayClose;

    public Vuforia.DefaultTrackableEventHandler trackableEventHandler;

    public ButtonGroup printSpeedButton;
    public string printSpeedURL;
    public ButtonGroup scannerButton;
    public ButtonGroup panelButton;
    public ButtonGroup tonerButton;
    public ButtonGroup sideTrayButton;
    public ButtonGroup paperTrayButton;

    private string openTrigger = "open";
    private string closeTrigger = "close";
    private string upTrigger = "up";
    private string downTrigger = "down";

    private CanvasGroup m_canvasGroup;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (m_canvasGroup == null)
            {
                m_canvasGroup = GetComponent<CanvasGroup>();
                if (m_canvasGroup == null)
                {
                    m_canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return m_canvasGroup;
        }
    }

    public bool Initialize()
    {
        if (trackableEventHandler != null)
        {
            Transform model = trackableEventHandler.transform.GetChildByName("model");
            scannerAnimator = model.GetChildByName("scanner-pivot").GetComponent<Animator>();
            panelAnimator = model.GetChildByName("panel-pivot").GetComponent<Animator>();
            tonerAnimator = model.GetChildByName("toner-pivot").GetComponent<Animator>();
            sideTrayAnimator = model.GetChildByName("side-tray-pivot").GetComponent<Animator>();
            paperTrayAnimator = model.GetChildByName("paper-tray-pivot").GetComponent<Animator>();
        }

        return true;
    }

    void Start()
    {
        Initialize();
    }

    void OnEnable()
    {
        printSpeedButton.onClick.AddListener(ShowPrintSpeed);
        scannerButton.onClick.AddListener(AnimateScanner);
        scannerButton.onClick.AddListener(scannerButton.ToggleState);
        panelButton.onClick.AddListener(AnimatePanel);
        panelButton.onClick.AddListener(panelButton.ToggleState);
        tonerButton.onClick.AddListener(AnimateToner);
        tonerButton.onClick.AddListener(tonerButton.ToggleState);
        sideTrayButton.onClick.AddListener(AnimateSideTray);
        sideTrayButton.onClick.AddListener(sideTrayButton.ToggleState);
        paperTrayButton.onClick.AddListener(AnimatePaperTray);
        paperTrayButton.onClick.AddListener(paperTrayButton.ToggleState);
    }

    void OnDisable()
    {
        printSpeedButton.onClick.RemoveAllListeners();
        scannerButton.onClick.RemoveAllListeners();
        panelButton.onClick.RemoveAllListeners();
        tonerButton.onClick.RemoveAllListeners();
        sideTrayButton.onClick.RemoveAllListeners();
        paperTrayButton.onClick.RemoveAllListeners();
    }

    public void ShowPrintSpeed()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL(printSpeedURL);
#elif UNITY_IOS
        Handheld.PlayFullScreenMovie(printSpeedURL);
#endif
    }

    public void AnimateScanner()
    {
        string state = scannerState == CopierAnimation.ScannerDown
                ? upTrigger
                : downTrigger;

        scannerAnimator.SetTrigger(state);

        scannerState = scannerState == CopierAnimation.ScannerDown
            ? CopierAnimation.ScannerUp
            : CopierAnimation.ScannerDown;
    }

    public void AnimatePanel()
    {
        string state = panelState == CopierAnimation.PanelDown
                ? upTrigger
                : downTrigger;

        panelAnimator.SetTrigger(state);

        panelState = panelState == CopierAnimation.PanelDown
            ? CopierAnimation.PanelUp
            : CopierAnimation.PanelDown;
    }

    public void AnimateToner()
    {
        string state = tonerState == CopierAnimation.TonerClose
                ? openTrigger
                : closeTrigger;

        tonerAnimator.SetTrigger(state);

        tonerState = tonerState == CopierAnimation.TonerClose
            ? CopierAnimation.TonerOpen
            : CopierAnimation.TonerClose;
    }

    public void AnimateSideTray()
    {
        string state = sideTrayState == CopierAnimation.SideTrayClose
                ? openTrigger
                : closeTrigger;

        sideTrayAnimator.SetTrigger(state);

        sideTrayState = sideTrayState == CopierAnimation.SideTrayClose
            ? CopierAnimation.SideTrayOpen
            : CopierAnimation.SideTrayClose;
    }

    public void AnimatePaperTray()
    {
        string state = paperTrayState == CopierAnimation.PaperTrayClose
                ? openTrigger
                : closeTrigger;

        paperTrayAnimator.SetTrigger(state);

        paperTrayState = paperTrayState == CopierAnimation.PaperTrayClose
            ? CopierAnimation.PaperTrayOpen
            : CopierAnimation.PaperTrayClose;
    }
}

[System.Serializable]
public enum CopierAnimation
{
    ScannerUp,
    ScannerDown,
    PanelUp,
    PanelDown,
    TonerOpen,
    TonerClose,
    SideTrayOpen,
    SideTrayClose,
    PaperTrayOpen,
    PaperTrayClose
}