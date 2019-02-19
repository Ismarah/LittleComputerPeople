using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private string myName;
    [SerializeField]
    private string favoriteAction;

    private void Start()
    {
        GetComponent<AgentActions>().GetAction(favoriteAction).AddEffect(WorldState.myStates.favoritePlayerAction, true);
    }

    public string GetFavoriteAction()
    {
        return favoriteAction;
    }

    public string GetName()
    {
        return myName;
    }
}
