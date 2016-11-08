using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CopierAR
{
    public class LocationSelectedEventArgs : EventArgs
    {
        public LocationData locationData;
    }

    public class LocationView : MonoBehaviour
    {
        public delegate void LocationSelectedEventHandler(object sender, LocationSelectedEventArgs args);
        public static event LocationSelectedEventHandler OnLocationSelected;

        public InputField locationField;
        public Text locationComment;
        public Button selectButton;

        public LocationData locationData { get; private set; }

        // Use this for initialization
        void Start()
        {
            locationData = new LocationData();
            Initialize();
            selectButton.onClick.AddListener(SelectLocation);
        }

        void OnDestroy()
        {
            selectButton.onClick.RemoveAllListeners();
        }

        void OnEnable()
        {
            locationField.onEndEdit.AddListener((x) => { locationData.code = x.ToString(); });
            locationField.onEndEdit.AddListener(delegate { ValidatePostalCode(); });
        }

        void OnDisable()
        {
            locationField.onEndEdit.RemoveAllListeners();
        }

        public bool Initialize()
        {
            ClearLocationInputField();
            locationData.Clear();
            HideError();
            return true;
        }

        private void ClearLocationInputField()
        {
            locationField.text = "";
            locationComment.text = "";
        }

        private bool ValidatePostalCode()
        {
            bool valid = true;

            if (locationField.text.Length < 6)
            {
                Response response = new Response(true, "Invalid postal code", ResponseType.InvalidPostalCode);
                ShowError(response);
                valid = false;
            }

            return valid;
        }

        public void ShowError(Response response)
        {
            locationComment.text = response.message;           
        }

        public void HideError()
        {
            locationComment.text = "";
        }

        public bool isValid
        {
            get
            {
                return (ValidatePostalCode());
            }
        }

        public void SelectLocation(bool success = false)
        {
            if (!isValid || !success)
            {
                return;
            }

            HideError();

            if (OnLocationSelected != null)
            {
                OnLocationSelected(this, new LocationSelectedEventArgs
                {
                    locationData = this.locationData             
                });
            }
        }
    }
}
