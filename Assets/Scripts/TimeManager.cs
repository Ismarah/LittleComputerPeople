﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private float time = 8 * 60;
    [SerializeField]
    private Text timer;
    [SerializeField]
    private float factor;
    [SerializeField]
    private float gameSpeed;

    void Update()
    {
        time += Time.deltaTime * factor * gameSpeed;

        string minutes = Mathf.Floor(time / 60).ToString("00");
        float minute = Mathf.Floor(time / 60);
        string seconds = Mathf.Floor(time % 60).ToString("00");
        float second = Mathf.Floor(time % 60);
        if (minute >= 6 && minute < 18) WorldState.state.ChangeState(WorldState.myStates.daytime, true);
        else WorldState.state.ChangeState(WorldState.myStates.daytime, false);
        if (Mathf.Floor(time / 60) >= 23 && Mathf.Floor(time % 60) == 0)
        {
            time = 0;
        }
        timer.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }
}
