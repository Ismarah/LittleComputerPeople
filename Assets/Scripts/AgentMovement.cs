using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentMovement : MonoBehaviour
{
    protected Vector2 targetPos;
    protected GameObject target;
    [SerializeField]
    protected int floor = 0;
    [SerializeField]
    protected float movespeed;
    [SerializeField]
    protected Transform firstStairsLower, firstStairsUpper, secondStairsLower, secondStairsUpper;
    private Animator anim;
    public bool turned;
    private Text actionText;

    public void Start()
    {
        anim = GetComponent<Animator>();
        targetPos = new Vector2();
        actionText = transform.GetComponentInChildren<Canvas>().transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void NewTarget(GameObject newTarget)
    {
        target = newTarget;
        int targetFloor = -1;
        if (target.GetComponent<InteractableItem>() != null)
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        else if (target.GetComponent<AgentMovement>() != null)
            targetFloor = target.GetComponent<AgentMovement>().GetFloor();
        if (targetFloor == floor)
        {
            //already on correct floor
            targetPos = new Vector2(newTarget.transform.position.x, transform.position.y);
            StartCoroutine(MoveToPos(targetPos));
        }
        else StartCoroutine(UseStairs(targetFloor));
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
        if (tag == "Pet")
        {
            if (transform.eulerAngles.y == 0) turned = false;
            else turned = true;
        }
        float prevPos = transform.position.x;
        anim.SetBool("isWalking", true);
        if (tag == "Pet") anim.SetBool("tail", false);
        Vector3 pos = new Vector3(_targetPos.x, _targetPos.y, transform.position.z);
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, movespeed * Time.deltaTime * GameObject.FindGameObjectWithTag("ActionQueue").GetComponent<TimeManager>().GetGameSpeed());
            if (tag == "Pet")
            {
                if (prevPos < transform.position.x && !turned)
                {
                    turned = true;
                    transform.RotateAround(transform.position, Vector3.up, 180);
                }
                if (prevPos > transform.position.x && turned)
                {
                    turned = false;
                    transform.RotateAround(transform.position, Vector3.up, 180);
                }
                prevPos = transform.position.x;
            }
            yield return null;
        }

        if (transform.position.x == target.transform.position.x)
        {
            if (target.GetComponent<InteractableItem>() != null)
            {
                target.GetComponent<InteractableItem>().AgentArrivedAtMyPosition(gameObject);
            }
            if(target.GetComponent<PlayerState>() != null && tag == "Pet")
            {
                target.GetComponent<PlayerState>().PetArrivedAtMyPosition();
            }
            anim.SetBool("isWalking", false);
            if (tag == "Pet") anim.SetBool("tail", true);
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
