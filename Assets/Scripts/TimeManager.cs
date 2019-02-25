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
        dayTimeChangeDuration = 0.004f;
        Time.timeScale = gameSpeed;
    }

    void Update()
    {
        time += Time.deltaTime * factor;

        string hours = Mathf.Floor(time / 60).ToString("00");
        float hour = Mathf.Floor(time / 60);
        string minutes = Mathf.Floor(time % 60).ToString("00");
        float minute = Mathf.Floor(time % 60);
        if (hour >= 6 && hour < 18)
        {
            if (background.sharedMaterial.color.b < dayTimeColor.b)
            {
                background.sharedMaterial.color += new Color(dayTimeChangeDuration, dayTimeChangeDuration, dayTimeChangeDuration) * Time.deltaTime;
            }
            if (WorldState.state.GetState(WorldState.myStates.daytime) != true)
                WorldState.state.ChangeState(WorldState.myStates.daytime, true);
        }
        else
        {
            if (hour >= 18 && hour < 20)
            {
                if (background.sharedMaterial.color.b > 0.15f)
                {
                    background.sharedMaterial.color -= new Color(dayTimeChangeDuration, dayTimeChangeDuration, dayTimeChangeDuration) * Time.deltaTime;
                }
            }
            if (WorldState.state.GetState(WorldState.myStates.daytime) != false)
                WorldState.state.ChangeState(WorldState.myStates.daytime, false);
        }
        if (hour > 23 && minute == 0)
        {
            time = 0;
        }
        timer.text = string.Format("{0}:{1}", hours, minutes);
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
