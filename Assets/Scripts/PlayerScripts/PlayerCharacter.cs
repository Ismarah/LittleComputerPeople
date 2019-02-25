using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private string myName;
    private string favoriteAction;
    private Material myClothes;
    private KeyValuePair<WorldState.myStates, bool> myCondition;
    private int favActionIndex;

    private void Start()
    {
        Invoke("Init", 0.1f);
    }

    public void Init()
    {
        GetComponent<PlayerActions>().GetAction(favoriteAction).AddEffect(WorldState.myStates.favoritePlayerAction, true);
        if (myName == "Alex")
        {
            SetMyCondition(WorldState.myStates.playerHasNothingToDo, true);
            favActionIndex = 3;
        }
        else if (myName == "Chris")
        {
            SetMyCondition(WorldState.myStates.playerHasNothingToDo, true);
            favActionIndex = 3;
        }
        else if (myName == "Elliot")
        {
            SetMyCondition(WorldState.myStates.snackInFridge, true);
            favActionIndex = 0;
        }
        else if (myName == "Jesse")
        {
            GetComponent<AgentActions>().GetAction(favoriteAction).RemoveCondition(WorldState.myStates.daytime);
            SetMyCondition(WorldState.myStates.playerIsTired, true);
            favActionIndex = 1;
        }
        GetComponent<PlayerVisuals>().Init();
    }

    public int GetFavActionIndex()
    {
        return favActionIndex;
    }

    public void SetMyCondition(WorldState.myStates condition, bool state)
    {
        myCondition = new KeyValuePair<WorldState.myStates, bool>(condition, state);
    }

    public KeyValuePair<WorldState.myStates, bool> GetMyCondition()
    {
        return myCondition;
    }

    public string GetFavoriteAction()
    {
        return favoriteAction;
    }

    public void SetFavoriteAction(string action)
    {
        favoriteAction = action;
    }

    public string GetName()
    {
        return myName;
    }

    public void SetName(string name)
    {
        myName = name;
    }

    public Material GetClothes()
    {
        return myClothes;
    }

    public void SetClothes(Material clothes)
    {
        myClothes = clothes;
    }
}
