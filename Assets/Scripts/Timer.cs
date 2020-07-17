using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Timer : MonoBehaviour
{
    private float time;

    public void SetTime(float time)
    {
        this.time = time;
    }

    public float GetTime()
    {
        return time;
    }

    public void Tick()
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            time = -1;
    }

    public void Reset()
    {
        time = 0;
    }
}