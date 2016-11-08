using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LocationView : MonoBehaviour
{
    public Dropdown LocationDropdown;

	// Use this for initialization
	void Start ()
    {
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnEnable()
    {
        LocationDropdown.onValueChanged.AddListener(delegate { DropdownValueChangedHandler(LocationDropdown); });
    }

    void OnDisable()
    {
        LocationDropdown.onValueChanged.RemoveAllListeners();
    }

    private void DropdownValueChangedHandler(Dropdown locationDropdown)
    {
        
    }
}
