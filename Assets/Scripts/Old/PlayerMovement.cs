//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField]
//    public int currentFloor = 2;
//    private bool walking;
//    private int onStairs = 0;
//    private IEnumerator newPosRoutine;
//    private Animator anim;
//    [SerializeField]
//    private GameObject[] stairs;
//    [SerializeField]
//    private float moveSpeed = 0;
//    private Transform firstStep;
//    private Transform lastStep;
//    private Vector2 newPosition;

//    private float firstFloor;
//    private float secondFloor;
//    private float thirdFloor;

//    private float myZPos;

//    void Start()
//    {
//        newPosRoutine = GoToPos(transform.position, currentFloor);
//        anim = GetComponent<Animator>();
//        newPosition = new Vector2();

//        myZPos = transform.position.z;

//        firstFloor = stairs[0].transform.GetChild(transform.childCount - 1).position.y;
//        thirdFloor = stairs[1].transform.GetChild(transform.childCount - 1).position.y;
        
//        firstFloor = -1f;
//        thirdFloor = 2.29f;
//    }

//    private void LateUpdate()
//    {
//        float height = transform.position.y;
//        if (height >= thirdFloor && currentFloor != 3)
//        {
//            Debug.Log("My height: " + height + " third floor: " + thirdFloor + " Reached 3. floor.");
//            currentFloor = 3;
//        }
//        else if (height < thirdFloor && height > firstFloor && currentFloor != 2)
//        {
//            Debug.Log("My height: " + height + " third floor: " + thirdFloor + " first floor: " + firstFloor + " Reached 2. floor.");
//            currentFloor = 2;
//        }
//        else if (height <= firstFloor && currentFloor != 1)
//        {
//            Debug.Log("My height: " + height + " first floor: " + firstFloor + " Reached 1. floor.");
//            currentFloor = 1;
//        }
//        transform.position = new Vector3(transform.position.x, transform.position.y, myZPos);

//    }

//    public void MoveToPos(Vector2 position, int floor)
//    {
//        StopCoroutine(newPosRoutine);

//        if (onStairs == 0) // not currently on stairs
//        {
//            if (floor == currentFloor) // already on correct floor
//            {
//                newPosition = new Vector2(position.x, transform.position.y);

//                newPosRoutine = GoToPos(newPosition, floor);
//                walking = true;
//                newPosition = position;
//                StartCoroutine(newPosRoutine);
//            }
//            else if (floor < currentFloor) // need to go gownstairs
//            {
//                newPosRoutine = UseStairs(false, position, floor);
//                StartCoroutine(newPosRoutine);
//            }
//            else if (floor > currentFloor) // need to go upstairs
//            {
//                newPosRoutine = UseStairs(true, position, floor);
//                StartCoroutine(newPosRoutine);
//            }
//        }
//        else //on stairs
//        {
//            if (floor <= currentFloor) // already on correct floor or go downstairs
//            {
//                //Debug.Log("On floor: " + currentFloor + " going to: " + floor + " onstairs = " + onStairs);
//                if (onStairs == 1)
//                {
//                    newPosition = new Vector2(firstStep.transform.position.x, firstStep.transform.position.y);
//                }
//                else
//                {
//                    newPosition = new Vector2(lastStep.transform.position.x, lastStep.transform.position.y);
//                }
//            }
//            else if (floor > currentFloor) // need to go upstairs
//            {
//                if (onStairs == 1) //was going up
//                {
//                    newPosition = new Vector2(lastStep.transform.position.x, lastStep.transform.position.y);
//                }
//                else if (onStairs == 2) //was going down
//                {
//                    newPosition = new Vector2(firstStep.transform.position.x, firstStep.transform.position.y);
//                }
//            }
//            newPosRoutine = GoToPos(newPosition, floor);
//            walking = true;
//            newPosition = position;
//            StartCoroutine(newPosRoutine);
//        }
//    }

//    private IEnumerator GoToPos(Vector2 position, int floor)
//    {
//        while (walking)
//        {
//            transform.position = Vector2.MoveTowards(transform.position, position, 3 * Time.deltaTime);
//            anim.SetBool("isWalking", true);
//            if ((Vector2)transform.position == position)
//            {
//                walking = false;
//                newPosRoutine = GoToPos(newPosition, floor);
//                StartCoroutine(newPosRoutine);
//                anim.SetBool("isWalking", false);

//                if (onStairs != 0) //is on stairs
//                {
//                    if (floor > currentFloor)
//                    {
//                        //currentFloor++;
//                        onStairs = 1;
//                    }
//                    else if (floor < currentFloor)
//                    {
//                        //currentFloor--;
//                        onStairs = 2;
//                    }
//                    else onStairs = 0;


//                    MoveToPos(newPosition, currentFloor);
//                }
//                yield break;
//            }
//            yield return null;

//        }
//    }

//    private IEnumerator UseStairs(bool up, Vector2 pos, int floor)
//    {
//        if (up)
//        {
//            firstStep = stairs[currentFloor - 1].transform.GetChild(0);
//            lastStep = stairs[currentFloor - 1].transform.GetChild(stairs[currentFloor - 1].transform.childCount - 1);
//        }
//        else
//        {
//            firstStep = stairs[currentFloor - 2].transform.GetChild(stairs[currentFloor - 2].transform.childCount - 1);
//            lastStep = stairs[currentFloor - 2].transform.GetChild(0);
//        }

//        bool onFirstStep = false;
//        while (!onFirstStep)
//        {
//            if (transform.position.x != firstStep.position.x)
//            {
//                Vector2 target = new Vector2(firstStep.position.x, transform.position.y);
//                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
//            }
//            else
//            {
//                onFirstStep = true;
//            }
//            anim.SetBool("isWalking", true);
//            yield return null;
//        }
//        bool onCorrectFloor = false;
//        while (!onCorrectFloor)
//        {
//            if (up)
//                onStairs = 1;
//            else onStairs = 2;

//            if (transform.position.x != lastStep.position.x)
//            {
//                transform.position = Vector2.MoveTowards(transform.position, lastStep.transform.position, moveSpeed * Time.deltaTime);
//            }
//            else
//            {
//                if (floor == currentFloor)
//                {
//                    onCorrectFloor = true;
//                }
//                onStairs = 0;
//                MoveToPos(pos, floor);
//            }
//            anim.SetBool("isWalking", true);
//            yield return null;
//        }
//    }
//}
