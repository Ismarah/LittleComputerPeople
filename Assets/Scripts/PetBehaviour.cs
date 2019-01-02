using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBehaviour : MonoBehaviour
{

    void Start()
    {
        GetComponent<AgentMovement>().NewTarget(GameObject.FindGameObjectWithTag("Player"));
    }

}
