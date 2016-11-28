using UnityEngine;
using System.Collections;
using System;

namespace CopierAR
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject ARCamera;
        public GameObject ShowcaseCamera;
        public GameObject ImageTracker;

	    // Use this for initialization
	    void OnEnable ()
        {
            ModelViewer.OnViewModeChanged += ModelViewer_OnViewModeChanged;
	    }

        private void ModelViewer_OnViewModeChanged(object sender, ViewModeChangedEventArgs args)
        {
            switch (args.ViewMode)
            {
                case ViewMode.Showcase:
                    ShowcaseMode();
                    break;
                case ViewMode.LifeScale:
                    ARMode();
                    break;
            }
        }

        private void ARMode()
        {
            ShowcaseCamera.SetActive(false);

            ARCamera.SetActive(true);
            ImageTracker.SetActive(true);
        }

        private void ShowcaseMode()
        {
            ImageTracker.SetActive(false);
            ARCamera.SetActive(false);

            ShowcaseCamera.SetActive(true);
        }

        private void OnDisable()
        {
            ModelViewer.OnViewModeChanged -= ModelViewer_OnViewModeChanged;

        }
    }
}
