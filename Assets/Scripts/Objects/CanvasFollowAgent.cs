using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowAgent : MonoBehaviour
{
    [SerializeField]
    private bool playerCanvas;
    private GameObject agent;
    private Vector3 offset;

    void Start()
    {
        if (!playerCanvas)
            agent = GameObject.FindGameObjectWithTag("Pet");
        else agent = GameObject.FindGameObjectWithTag("Player");

        offset = transform.position - agent.transform.position;
    }

    void LateUpdate()
    {
        transform.position = agent.transform.position + offset;
    }
}
