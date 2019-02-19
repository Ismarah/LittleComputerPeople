using System.Collections.Generic;
using UnityEngine;

public class Action
{
    private GameObject agent;
    private float[] actionStats;
    private Dictionary<WorldState.myStates, bool> conditions;
    private Dictionary<WorldState.myStates, bool> effects;
    private GameObject myObject;
    private string name;
    private float time;
    private string animation;
    private bool hasAnimation;
    private bool animationLeft;

    public Action(GameObject _agent, string _name, float _time, float[] _actionStats, Dictionary<WorldState.myStates, bool> _conditions, Dictionary<WorldState.myStates, bool> _effects, GameObject obj)
    {
        agent = _agent;
        actionStats = _actionStats;
        conditions = _conditions;
        effects = _effects;
        myObject = obj;
        name = _name;
        time = _time;
    }

    public Action()
    {

    }

    public GameObject GetAgent()
    {
        return agent;
    }

    public bool HasAnimation()
    {
        return hasAnimation;
    }

    public void AddAnimation(string anim, bool left)
    {
        animationLeft = left;
        animation = anim;
        hasAnimation = true;
    }

    public KeyValuePair<string, bool> GetAnimation()
    {
        return new KeyValuePair<string, bool>(animation, animationLeft);
    }

    public float[] GetStats()
    {
        return actionStats;
    }

    public string GetName()
    {
        return name;
    }

    public float GetTime()
    {
        return time;
    }

    public void AddEffect(WorldState.myStates worldState, bool state)
    {
        effects.Add(worldState, state);
    }

    public Dictionary<WorldState.myStates, bool> GetPreconditions()
    {
        return conditions;
    }

    public Dictionary<WorldState.myStates, bool> GetEffects()
    {
        return effects;
    }

    public GameObject GetObject()
    {
        return myObject;
    }

}
