using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class CopierController : MonoBehaviour
    {
        [SerializeField]
        Animator scannerAnimator;
        [SerializeField]
        Animator panelAnimator;
        [SerializeField]
        Animator tonerAnimator;
        [SerializeField]
        Animator paperTrayAnimator;
        [SerializeField]
        Animator sideTrayAnimator;

        // Default animator states
        public CopierAnimation scannerState = CopierAnimation.ScannerDown;
        public CopierAnimation panelState = CopierAnimation.PanelDown;
        public CopierAnimation tonerState = CopierAnimation.TonerClose;
        public CopierAnimation paperTrayState = CopierAnimation.PaperTrayClose;
        public CopierAnimation sideTrayState = CopierAnimation.SideTrayClose;

        [SerializeField]
        string openTrigger = "open";
        [SerializeField]
        string closeTrigger = "close";
        [SerializeField]
        string upTrigger = "up";
        [SerializeField]
        string downTrigger = "down";

        public void ShowPrintSpeed(string url)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            Application.OpenURL(url);
#elif UNITY_IOS
        Handheld.PlayFullScreenMovie(url);
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


        public void ResetCopier()
        {
            scannerState = CopierAnimation.ScannerDown;
            panelState = CopierAnimation.PanelDown;
            tonerState = CopierAnimation.TonerClose;
            paperTrayState = CopierAnimation.PaperTrayClose;
            sideTrayState = CopierAnimation.SideTrayClose;

            scannerAnimator.SetTrigger(downTrigger);
            panelAnimator.SetTrigger(downTrigger);
            tonerAnimator.SetTrigger(closeTrigger);
            paperTrayAnimator.SetTrigger(closeTrigger);
            sideTrayAnimator.SetTrigger(closeTrigger);
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
        PaperTrayOpen,
        PaperTrayClose,
        SideTrayOpen,
        SideTrayClose
    }
}