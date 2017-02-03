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

        [SerializeField]
        CameraSettings cameraSettings;

        [SerializeField]
        TapHandler tapHandler;

        // Use this for initialization
        void OnEnable ()
        {
            ModelViewer.OnViewModeChanged += ModelViewer_OnViewModeChanged;

            cameraSettings.enabled = false;
            tapHandler.enabled = false;
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

            cameraSettings.enabled = true;
            tapHandler.enabled = true;
        }

        private void ShowcaseMode()
        {
            ImageTracker.SetActive(false);
            ARCamera.SetActive(false);

            ShowcaseCamera.SetActive(true);

            cameraSettings.enabled = false;
            tapHandler.enabled = false;
        }

        private void OnDisable()
        {
            ModelViewer.OnViewModeChanged -= ModelViewer_OnViewModeChanged;

            cameraSettings.enabled = false;
            tapHandler.enabled = false;
        }
    }
}
