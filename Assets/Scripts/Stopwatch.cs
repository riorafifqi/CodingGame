using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;


public class Stopwatch : MonoBehaviour
{
    bool isStopwatchActive = false;
    float currentTime;

    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        ResetStopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopwatchActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }

        if(currentTime != 0)
            timeText.text = "Time : " + currentTime.ToString("F3") + "s";
    }

    public void StartStopwatch()
    {
        isStopwatchActive = true;
    }

    public void StopStopwatch()
    {
        isStopwatchActive = false;
    }

    public void ResetStopwatch()
    {
        currentTime = 0;
        timeText.text = "Time : " + currentTime.ToString() + "s" ;
    }

    public float GetTime()
    {
        return Mathf.Round(currentTime * 1000f) / 1000f;
    }
}
