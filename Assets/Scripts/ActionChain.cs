using System.Collections;
using System.Collections.Generic;

public class ActionChain
{
    List<Action> myList;
    float wayTime;
    string chainName;

    public ActionChain()
    {
        myList = new List<Action>();
    }

    public ActionChain(Action newAction)
    {
        myList = new List<Action>();
        myList.Add(newAction);
    }

    public void Add(Action newAction)
    {
        myList.Add(newAction);
    }

    public List<Action> GetActions()
    {
        return myList;
    }

    public void SetName(string name)
    {
        chainName = name;
    }

    public string GetName()
    {
        return chainName;
    }

    //public float GetChainStateChange()
    //{
    //    float chainCost = 0;
    //    for (int j = 0; j < myList.Count; j++)
    //    {
    //        chainCost += myList[j].GetStateChange();
    //    }
    //    return chainCost;
    //}

    public float GetChainDuration()
    {
        float chainDuration = 0;

        for (int i = 0; i < myList.Count; i++)
        {
            chainDuration += myList[i].GetTime();
        }
        chainDuration += wayTime;
        return chainDuration;
    }

    public bool ContainsAction(Action action)
    {
        bool contains = false;

        for (int i = 0; i < myList.Count; i++)
        {
            if(myList[i] == action)
            {
                contains = true;
            }
        }

        return contains;
    }

    public void AddWalkTime(float time)
    {
        wayTime += time;
    }
}
