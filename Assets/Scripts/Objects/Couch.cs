using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : InteractableItem
{
    [SerializeField]
    private GameObject tv;
    private Renderer tvScreen;
    [SerializeField]
    private Material[] screens;
    private Material black;

    void Start()
    {
        myFloor = 1;
        Init();
        tvScreen = tv.GetComponent<Renderer>();
        black = tvScreen.sharedMaterial;
    }

    public override void AgentArrivedAtMyPosition(GameObject agent)
    {
        base.AgentArrivedAtMyPosition(agent);

        StartCoroutine(AnimateScreen());
    }

    private IEnumerator AnimateScreen()
    {
        float time = 0;
        float gameSpeed = GameObject.FindGameObjectWithTag("ActionQueue").GetComponent<TimeManager>().GetGameSpeed();
        float duration = player.GetComponent<PlayerActions>().GetAction("Watch TV").GetTime() / gameSpeed;
        float delay = 0.1f;

        while (time <= duration)
        {
            time += Time.deltaTime * gameSpeed;
            time += delay;
            int rand = Random.Range(0, screens.Length);
            tvScreen.sharedMaterial = screens[rand];
            yield return new WaitForSeconds(delay);
            yield return null;
        }
        tvScreen.sharedMaterial = black;
    }
}
