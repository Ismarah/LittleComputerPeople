using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject lastObjectClicked;
    private GameObject actionQueue;
    private GameObject player;

    private void Start()
    {
        lastObjectClicked = new GameObject();
        actionQueue = GameObject.FindGameObjectWithTag("ActionQueue");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<InteractableItem>() != null)
                {
                    lastObjectClicked.SetActive(false);
                    //Debug.Log (hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<InteractableItem>().OnClick();

                    lastObjectClicked = hit.collider.gameObject.GetComponent<InteractableItem>().GetMyUIObject();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name.Contains("Icon"))
                {
                    actionQueue.GetComponent<ActionQueue>().RemoveFromQueue(hit.collider.gameObject);
                    player.GetComponent<Player>().CancelNextTask();
                }
            }
        }
    }
}
