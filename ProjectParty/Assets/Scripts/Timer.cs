using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    public event EventHandler timerStop;


    public void start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timerStop?.Invoke(this, EventArgs.Empty);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
}
