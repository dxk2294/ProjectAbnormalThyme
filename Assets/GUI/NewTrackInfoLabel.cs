﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewTrackInfoLabel : MonoBehaviour
{
    TextMeshPro label;

    TrackManager trackManager;


    // Start is called before the first frame update
    void Start()
    {
        trackManager = FindObjectOfType<TrackManager>();
        label = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trackManager.TrackCanBeExtended)
        {
            if (trackManager.CostToExtendTrack > 100_000)
            {
                label.text = "" + string.Format("{0:#.###E+00}", trackManager.CostToExtendTrack);
            }
            else
            {
                label.text = "" + string.Format("{0:#,0}", trackManager.CostToExtendTrack);
            }
        } else
        {
            label.enabled = false;
        }
    }
}
