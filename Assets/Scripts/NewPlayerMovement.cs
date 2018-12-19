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
    [SerializeField]
    private Transform firstStairsLower;
    [SerializeField]
    private Transform firstStairsUpper;
    [SerializeField]
    private Transform secondStairsLower;
    [SerializeField]
    private Transform secondStairsUpper;

    void Start()
    {
        targetPos = new Vector2();
        anim = GetComponent<Animator>();
    }

    public void NewTarget(GameObject newTarget)
    {
        target = newTarget;
        int targetFloor = newTarget.GetComponent<InteractableItem>().GetFloor();
        if (targetFloor == floor)
        {
            //player is already on correct floor
            targetPos = new Vector2(newTarget.transform.position.x, transform.position.y);
            StartCoroutine(MoveToPos(targetPos));
        }
        else
        {
            StartCoroutine(UseStairs(targetFloor));
        }
    }

    private IEnumerator UseStairs(int targetFloor)
    {
        while (floor != targetFloor)
        {
            if (targetFloor > floor)
            {
                if (targetFloor == floor + 1)
                {
                    //target is one floor above player
                    if (floor == 0)
                    {
                        yield return StartCoroutine(MoveToPos(firstStairsLower.position));
                        yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
                        floor += 1;
                        targetPos = new Vector2(target.transform.position.x, transform.position.y);
                        yield return StartCoroutine(MoveToPos(targetPos));
                        yield break;
                    }
                    else if (floor == 1)
                    {
                        yield return StartCoroutine(MoveToPos(secondStairsLower.position));
                        yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
                        floor += 1;
                        targetPos = new Vector2(target.transform.position.x, transform.position.y);
                        yield return StartCoroutine(MoveToPos(targetPos));
                        yield break;
                    }

                }
                else if (targetFloor == floor + 2)
                {
                    //target is two floors above player
                    yield return StartCoroutine(MoveToPos(firstStairsLower.position));
                    yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
                    floor += 1;
                    yield return StartCoroutine(MoveToPos(secondStairsLower.position));
                    yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
                    floor += 1;
                    targetPos = new Vector2(target.transform.position.x, transform.position.y);
                    yield return StartCoroutine(MoveToPos(targetPos));
                }
            }
            else
            {
                if (targetFloor == floor - 1)
                {
                    //target is one floor below player

                }
                else if (targetFloor == floor - 2)
                {
                    //target is two floors below player

                }
            }
            yield return null;
        }
        Debug.Log("Reached correct floor.");
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
        Debug.Log("Stop moving to position");
    }
}
