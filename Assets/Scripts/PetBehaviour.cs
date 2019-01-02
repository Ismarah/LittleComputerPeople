using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<AgentMovement>().NewTarget(GameObject.FindGameObjectWithTag("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
