using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentMovement : MonoBehaviour
{
    private Vector2 targetPos;
    private GameObject target;
    [SerializeField]
    private int floor = 0;
    [SerializeField]
    private float movespeed;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform firstStairsLower;
    [SerializeField]
    private Transform firstStairsUpper;
    [SerializeField]
    private Transform secondStairsLower;
    [SerializeField]
    private Transform secondStairsUpper;
    private Text actionText;

    void Start()
    {
        targetPos = new Vector2();
        actionText = transform.GetComponentInChildren<Canvas>().transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void NewTarget(GameObject newTarget)
    {
        target = newTarget;
        int targetFloor = -1;
        if (target.GetComponent<InteractableItem>() != null)
        {
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        }
        else if (target.GetComponent<AgentMovement>() != null)
        {
            targetFloor = target.GetComponent<AgentMovement>().GetFloor();
        }
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

    public int GetFloor()
    {
        return floor;
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
                    if (floor == 1)
                    {
                        yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
                        yield return StartCoroutine(MoveToPos(firstStairsLower.position));
                        floor -= 1;
                        targetPos = new Vector2(target.transform.position.x, transform.position.y);
                        yield return StartCoroutine(MoveToPos(targetPos));
                        yield break;
                    }
                    else if (floor == 2)
                    {
                        yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
                        yield return StartCoroutine(MoveToPos(secondStairsLower.position));
                        floor -= 1;
                        targetPos = new Vector2(target.transform.position.x, transform.position.y);
                        yield return StartCoroutine(MoveToPos(targetPos));
                        yield break;
                    }

                }
                else if (targetFloor == floor - 2)
                {
                    //target is two floors below player
                    yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
                    yield return StartCoroutine(MoveToPos(secondStairsLower.position));
                    floor -= 1;
                    yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
                    yield return StartCoroutine(MoveToPos(firstStairsLower.position));
                    floor -= 1;
                    targetPos = new Vector2(target.transform.position.x, transform.position.y);
                    yield return StartCoroutine(MoveToPos(targetPos));
                }
            }
            yield return null;
        }
    }

    private IEnumerator MoveToPos(Vector2 _targetPos)
    {
        Vector3 prevPos = new Vector3();
        bool turned = false;
        anim.SetBool("isWalking", true);
        Vector3 pos = new Vector3(_targetPos.x, _targetPos.y, transform.position.z);
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, movespeed * Time.deltaTime);
            if (prevPos.x < transform.position.x && !turned)
            {
                turned = true;
                transform.RotateAround(transform.position, Vector3.up, 180);
                actionText.transform.parent.RotateAround(transform.position, Vector3.up, 180);
            }
            else if (prevPos.x > transform.position.x && turned)
            {
                turned = false;
                transform.RotateAround(transform.position, Vector3.up, 180);
                actionText.transform.parent.RotateAround(transform.position, Vector3.up, 180);
            }

            prevPos = transform.position;
            yield return null;
        }

        if (transform.position.x == target.transform.position.x)
        {
            if (target.GetComponent<InteractableItem>() != null)
            {
                target.GetComponent<InteractableItem>().PlayerArrivedAtMyPosition();
            }
            anim.SetBool("isWalking", false);
        }
    }

    public float GetMoveSpeed()
    {
        return movespeed;
    }

    public Transform[] GetStairs()
    {
        Transform[] temp = new Transform[4];
        temp[0] = firstStairsLower;
        temp[1] = firstStairsUpper;
        temp[2] = secondStairsLower;
        temp[3] = secondStairsUpper;

        return temp;
    }
}
