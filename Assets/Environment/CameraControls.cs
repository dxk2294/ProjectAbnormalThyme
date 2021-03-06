﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float speed = 2.0f;
    public float minDistance = 0;
    public float maxDistance = 9.5f;
    ResearchPopUp researchPopUp;

    // Start is called before the first frame update
    void Start()
    {
        var popupGOs = Resources.FindObjectsOfTypeAll(typeof(ResearchPopUp));
        if (popupGOs != null && popupGOs.Length > 0)
        {
            researchPopUp = ((ResearchPopUp)popupGOs[0]);
        }
    }

    // Update is called once per frame
    void Update() { 
        if (Input.GetKeyDown(KeyCode.R) && researchPopUp != null)
        {
            AudioManager.main.PlayButtonClick();
            researchPopUp.ToggleOpen();
        }

        if (Input.GetMouseButtonDown(1))
        {
            AudioManager.main.PlayButtonClick();
            FindObjectOfType<TrackPlacer>()?.SelectBuilding(null);
        }

        for (KeyCode i = KeyCode.Alpha1; i <= KeyCode.Alpha9; i++)
        {
            //inefficient lol
            if (Input.GetKeyDown(i))
            {
                int index = i - KeyCode.Alpha1;
                var buildings = FindObjectOfType<TracksTab>()?.buildings;
                if (buildings != null && buildings.Count > index)
                {
                    AudioManager.main.PlayButtonClick();
                    FindObjectOfType<TrackPlacer>()?.SelectBuilding(buildings[index]);
                }
            }
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(Mathf.Min(speed * Time.deltaTime + transform.position.x, maxDistance), transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(Mathf.Max(-(speed * Time.deltaTime) + transform.position.x, minDistance), transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO(james) - calculate midpoint of traintrack, center camera there?
            AudioManager.main.PlayButtonClick();
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
