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

        public void ShowPrintSpeed(string url)
        {
            Application.OpenURL(url);
        }

        public void AnimateScanner()
        {
            bool state = scannerState == CopierAnimation.ScannerDown
                    ? true
                    : false;

            scannerAnimator.SetBool("animated", state);

            scannerState = scannerState == CopierAnimation.ScannerDown
                ? CopierAnimation.ScannerUp
                : CopierAnimation.ScannerDown;

            Debug.Log("Animating scanner: " + scannerState.ToString());
        }

        public void AnimatePanel()
        {
            bool state = panelState == CopierAnimation.PanelDown
                    ? true
                    : false;

            panelAnimator.SetBool("animated", state);

            panelState = panelState == CopierAnimation.PanelDown
                ? CopierAnimation.PanelUp
                : CopierAnimation.PanelDown;
        }

        public void AnimateToner()
        {
            bool state = tonerState == CopierAnimation.TonerClose
                    ? true
                    : false;

            tonerAnimator.SetBool("animated", state);

            tonerState = tonerState == CopierAnimation.TonerClose
                ? CopierAnimation.TonerOpen
                : CopierAnimation.TonerClose;
        }

        public void AnimatePaperTray()
        {
            bool state = paperTrayState == CopierAnimation.PaperTrayClose
                    ? true
                    : false;

            paperTrayAnimator.SetBool("animated", state);

            paperTrayState = paperTrayState == CopierAnimation.PaperTrayClose
                ? CopierAnimation.PaperTrayOpen
                : CopierAnimation.PaperTrayClose;
        }
        public void AnimateSideTray()
        {
            bool state = sideTrayState == CopierAnimation.SideTrayClose
                    ? true
                    : false;

            sideTrayAnimator.SetBool("animated", state);

            sideTrayState = sideTrayState == CopierAnimation.SideTrayClose
                ? CopierAnimation.SideTrayOpen
                : CopierAnimation.SideTrayClose;
        }


        public IEnumerator ResetCopier()
        {
            scannerState = CopierAnimation.ScannerDown;
            panelState = CopierAnimation.PanelDown;
            tonerState = CopierAnimation.TonerClose;
            paperTrayState = CopierAnimation.PaperTrayClose;
            sideTrayState = CopierAnimation.SideTrayClose;

            yield return new WaitForEndOfFrame();

            scannerAnimator.SetBool("animated", false);
            panelAnimator.SetBool("animated", false);
            tonerAnimator.SetBool("animated", false);
            paperTrayAnimator.SetBool("animated", false);
            sideTrayAnimator.SetBool("animated", false);                        
        }

        public void ResetCopierToDefault()
        {
            scannerAnimator.SetBool("animated", false);
            panelAnimator.SetBool("animated", false);
            tonerAnimator.SetBool("animated", false);
            paperTrayAnimator.SetBool("animated", false);
            sideTrayAnimator.SetBool("animated", false);
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