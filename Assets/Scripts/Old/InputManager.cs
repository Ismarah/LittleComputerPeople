using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject lastObjectClicked;
    private GameObject player;

    private void Start()
    {
        lastObjectClicked = GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
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
                    hit.collider.gameObject.GetComponent<InteractableItem>().OnClick();

                    lastObjectClicked = hit.collider.gameObject.GetComponent<InteractableItem>().GetMyUIObject();
                }
                else if (hit.collider.tag == "Background")
                {
                    int floor = int.Parse(hit.collider.name);
                    player.GetComponent<PlayerMovement>().MoveToPos(hit.point, floor);
                    lastObjectClicked.SetActive(false);
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
                    ActionQueue.instance.GetComponent<ActionQueue>().RemoveFromQueue(hit.collider.gameObject);
                    player.GetComponent<Player>().CancelNextTask();
                }
            }
        }
    }
}
