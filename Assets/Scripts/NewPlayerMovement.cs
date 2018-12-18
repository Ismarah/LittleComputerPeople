using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    private Vector2 targetPos;
    private GameObject target;
    [SerializeField]
    private int floor = 0;
    [SerializeField]
    private float movespeed;
    private Animator anim;

    void Start()
    {
        targetPos = new Vector2();
        anim = GetComponent<Animator>();
    }

    public void NewTarget(GameObject newTarget)
    {
        target = newTarget;
        if (newTarget.GetComponent<InteractableItem>().GetFloor() == floor)
        {
            targetPos = new Vector2(newTarget.transform.position.x, transform.position.y);
            StartCoroutine(MoveToPos(targetPos));
        }
    }

    private IEnumerator MoveToPos(Vector2 _targetPos)
    {
        anim.SetBool("isWalking", true);
        while ((Vector2)transform.position != _targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, movespeed * Time.deltaTime);
            yield return null;
        }

        if (transform.position.x == target.transform.position.x)
        {
            target.GetComponent<InteractableItem>().PlayerArrivedAtMyPosition();
            anim.SetBool("isWalking", false);
        }
    }
}
