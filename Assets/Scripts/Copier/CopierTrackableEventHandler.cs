/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using CopierAR;
using System;

namespace Vuforia
{
    public class TrackingStateChangedEventArgs : EventArgs
    {
        public TrackableBehaviour.Status Status;
    }

    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class CopierTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;

        #endregion // PRIVATE_MEMBER_VARIABLES

        #region PUBLIC_STATIC_VARIABLES

        public static bool IsTracked = false;

        #endregion // PUBLIC_STATIC_VARIABLES

        public delegate void TrackingStateChangedEventHandler(object sender, TrackingStateChangedEventArgs args);
        public static event TrackingStateChangedEventHandler OnTrackingStateChanged;

        #region UNITY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        public TrackableBehaviour.Status GetCurrentStatus()
        {
            return mTrackableBehaviour.CurrentStatus;
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            //Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            //Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            //// Enable rendering:
            //foreach (Renderer component in rendererComponents)
            //{
            //    component.enabled = true;
            //}

            //// Enable colliders:
            //foreach (Collider component in colliderComponents)
            //{
            //    component.enabled = true;
            //}

            IsTracked = true;

            if (OnTrackingStateChanged != null)
            {
                OnTrackingStateChanged(this, new TrackingStateChangedEventArgs() { Status = TrackableBehaviour.Status.TRACKED });
            }
            //ModelViewer.OnTrackingFound();

            Debug.Log(Time.time.ToString() +  " Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            //Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            //Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            //// Disable rendering:
            //foreach (Renderer component in rendererComponents)
            //{
            //    component.enabled = false;
            //}

            //// Disable colliders:
            //foreach (Collider component in colliderComponents)
            //{
            //    component.enabled = false;
            //}

            IsTracked = false;

            if (OnTrackingStateChanged != null)
            {
                OnTrackingStateChanged(this, new TrackingStateChangedEventArgs() { Status = TrackableBehaviour.Status.NOT_FOUND });
            }

            //ModelViewer.OnTrackingLost();

            Debug.Log(Time.time.ToString() + " Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        private void Update()
        {
            Debug.Log(mTrackableBehaviour.CurrentStatus);
        }

        #endregion // PRIVATE_METHODS
    }
}
