using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public int currentFloor = 2;
    private bool walking;
    private bool onStairs;
    private IEnumerator newPosRoutine;
    private Animator anim;
    [SerializeField]
    private GameObject[] stairs;
    private bool useStairs;
    [SerializeField]
    private float moveSpeed = 0;
    private Transform firstStep;
    private Transform lastStep;
    private Vector3 newPosition;

    void Start()
    {
        newPosRoutine = GoToPos(transform.position);
        anim = GetComponent<Animator>();
        newPosition = new Vector3();
    }

    public void MoveToPos(Vector3 position, int floor)
    {
        StopCoroutine(newPosRoutine);

        if (!onStairs) // not currently on stairs
        {
            if (floor == currentFloor) // already on correct floor
            {
                newPosition = new Vector3(position.x, transform.position.y, transform.position.z);

                newPosRoutine = GoToPos(newPosition);
                walking = true;
                newPosition = position;
                StartCoroutine(newPosRoutine);
            }
            else if (floor < currentFloor) // need to go gownstairs
            {
                newPosRoutine = UseStairs(false, position, floor);
                StartCoroutine(newPosRoutine);
            }
            else if (floor > currentFloor) // need to go upstairs
            {
                newPosRoutine = UseStairs(true, position, floor);
                StartCoroutine(newPosRoutine);
            }
        }
        else //on stairs
        {
            if (floor <= currentFloor) // already on correct floor or go downstairs
            {
                newPosition = new Vector3(firstStep.transform.position.x, firstStep.transform.position.y, transform.position.z);
            }
            else if (floor > currentFloor) // need to go upstairs
            {
                newPosition = new Vector3(lastStep.transform.position.x, firstStep.transform.position.y, transform.position.z);
            }
            newPosRoutine = GoToPos(newPosition);
            walking = true;
            newPosition = position;
            StartCoroutine(newPosRoutine);

        }
    }

    private IEnumerator GoToPos(Vector3 position)
    {
        while (walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 3 * Time.deltaTime);
            anim.SetBool("isWalking", true);
            if (transform.position == position)
            {
                walking = false;
                newPosRoutine = GoToPos(newPosition);
                StartCoroutine(newPosRoutine);
                anim.SetBool("isWalking", false);

                if (onStairs)
                {
                    onStairs = false;
                    MoveToPos(newPosition, currentFloor);
                }
                //Debug.Log("stop walking");
                yield break;
            }
            //Debug.Log("walking");
            yield return null;

        }
    }

    private IEnumerator UseStairs(bool up, Vector3 pos, int floor)
    {
        if (up)
        {
            firstStep = stairs[currentFloor - 1].transform.GetChild(0);
            lastStep = stairs[currentFloor - 1].transform.GetChild(stairs[currentFloor - 1].transform.childCount - 1);
            Debug.Log("up");
        }
        else
        {
            firstStep = stairs[currentFloor - 2].transform.GetChild(stairs[currentFloor - 2].transform.childCount - 1);
            lastStep = stairs[currentFloor - 2].transform.GetChild(0);
            Debug.Log("down");
        }

        bool onFirstStep = false;
        while (!onFirstStep)
        {
            if (transform.position.x != firstStep.position.x)
            {
                Vector2 target = new Vector2(firstStep.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                onFirstStep = true;
            }
            anim.SetBool("isWalking", true);
            yield return null;
        }
        bool onCorrectFloor = false;
        if (!up) currentFloor--;
        while (!onCorrectFloor)
        {
            onStairs = true;
            if (transform.position.x != lastStep.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, lastStep.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                if (up) currentFloor++;

                Debug.Log("On correct floor now: " + (floor == currentFloor));
                if (floor == currentFloor)
                {
                    onCorrectFloor = true;
                }
                onStairs = false;
                MoveToPos(pos, floor);
            }
            anim.SetBool("isWalking", true);
            yield return null;
        }
    }
}
