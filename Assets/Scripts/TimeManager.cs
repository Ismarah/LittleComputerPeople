using System.Collections;
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
    [SerializeField]
    private Renderer background;
    private Color dayTimeColor;
    private float dayTimeChangeDuration;

    private void Start()
    {
        dayTimeColor = background.sharedMaterial.color;
        dayTimeChangeDuration = 0.004f / gameSpeed; 
    }

    void Update()
    {
        time += Time.deltaTime * factor * gameSpeed;

        string minutes = Mathf.Floor(time / 60).ToString("00");
        float minute = Mathf.Floor(time / 60);
        string seconds = Mathf.Floor(time % 60).ToString("00");
        float second = Mathf.Floor(time % 60);
        if (minute >= 6 && minute < 18)
        {
            if (minute < 8)
            {
                if (background.sharedMaterial.color.b < dayTimeColor.b)
                {
                    background.sharedMaterial.color += new Color(dayTimeChangeDuration, dayTimeChangeDuration, dayTimeChangeDuration);
                }
            }
            if (WorldState.state.GetState(WorldState.myStates.daytime) != true)
                WorldState.state.ChangeState(WorldState.myStates.daytime, true);
        }
        else
        {
            if (minute >= 18 && minute < 20)
            {
                if (background.sharedMaterial.color.b > 0.15f)
                {
                    background.sharedMaterial.color -= new Color(dayTimeChangeDuration, dayTimeChangeDuration, dayTimeChangeDuration);
                }
            }
            if (WorldState.state.GetState(WorldState.myStates.daytime) != false)
                WorldState.state.ChangeState(WorldState.myStates.daytime, false);
        }
        if (minute > 23 && second == 0)
        {
            Debug.Log("0 Uhr! " + minutes + " " + seconds);
            time = 0;
        }
        timer.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }

    void OnApplicationQuit()
    {
        background.sharedMaterial.color = dayTimeColor;
    }
}
