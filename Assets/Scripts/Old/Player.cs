//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    private Vector2 target;
//    private Queue<GameObject> nextTargets;
//    private GameObject actionQueue;

//    [SerializeField]
//    private float moveSpeed = 0;
//    private Animator anim;
//    private int currentFloor;
//    private bool onCorrectFloor;
//    private GameObject stairs;
//    private bool useStairs;
//    [SerializeField]
//    private Material[] myMaterials;
//    private bool waitingForActionFinished;
//    private bool finished;
//    private bool nothingToDo;
//    //private IEnumerator bored;

//    void Start()
//    {
//        nextTargets = new Queue<GameObject>();
//        anim = GetComponent<Animator>();
//        stairs = GameObject.FindGameObjectWithTag("Stairs");
//        actionQueue = GameObject.FindGameObjectWithTag("ActionQueue");
//        //bored = GettingBored();

//        currentFloor = 1;

//        Invoke("StartDoingStuff", 2);
//    }

//    void StartDoingStuff()
//    {
//        StartCoroutine(NextTask());
//    }

//    private IEnumerator NextTask()
//    {
//        while (true)
//        {
//            if (nextTargets.Count != 0)
//            {
//                //StopCoroutine(bored);
//                int floor = nextTargets.Peek().GetComponent<InteractableItem>().GetFloor();

//                if (floor != currentFloor)
//                {
//                    onCorrectFloor = false;
//                    if (!useStairs)
//                    {
//                        if (floor < currentFloor)
//                            StartCoroutine(GoDownStairs());
//                        else
//                        {
//                            StartCoroutine(GoUpStairs());
//                        }
//                        useStairs = true;
//                    }
//                }
//                else
//                {
//                    if (transform.position.x != nextTargets.Peek().transform.position.x)
//                    {
//                        Vector3 newTarget = new Vector3(nextTargets.Peek().transform.position.x, transform.position.y, transform.position.z);
//                        transform.position = Vector3.MoveTowards(transform.position, newTarget, moveSpeed * Time.deltaTime);
//                        anim.SetBool("isWalking", true);
//                    }
//                    else
//                    {
//                        if (!waitingForActionFinished)
//                        {
//                            nextTargets.Peek().GetComponent<InteractableItem>().PlayerMightBeNear();

//                            anim.SetBool("isWalking", false);

//                            waitingForActionFinished = true;
//                        }
//                        else
//                        {
//                            if (finished)
//                            {
//                                nextTargets.Dequeue();
//                                ActionQueue.instance.GetComponent<ActionQueue>().FinishedAction();
//                                waitingForActionFinished = false;
//                                finished = false;
//                            }
//                        }

//                    }
//                }
//            }
//            else
//            {
//                //if (!nothingToDo)
//                //{
//                //    StartCoroutine(bored);
//                //    nothingToDo = true;
//                //}
//            }
//            yield return null;
//        }
//    }

//    //private IEnumerator GettingBored()
//    //{
//    //    Debug.Log("Getting bored...");
//    //    yield return new WaitForSeconds(3);

//    //    if (nextTargets.Count == 0)
//    //    {
//    //        Debug.Log("Nothing to do....");
//    //        EventManager.TriggerEvent("playerIsBored");
//    //    }
//    //}

//    public void ActionFinished()
//    {
//        finished = true;
//    }

//    public void CancelNextTask()
//    {
//        nextTargets.Dequeue();
//        GetComponent<PlayerNeeds>().StopEating();
//        anim.SetBool("isWalking", false);
//        waitingForActionFinished = false;
//    }

//    public void SetTarget(GameObject _target)
//    {
//        if (_target.GetComponent<InteractableItem>() != null)
//        {
//            nextTargets.Enqueue(_target);
//            nothingToDo = false;
//        }
//    }

//    public void ChangeClothes()
//    {
//        int rand = Random.Range(0, myMaterials.Length);

//        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
//        {
//            rend.sharedMaterial = myMaterials[rand];
//        }
//        ActionFinished();
//    }

//    private IEnumerator GoDownStairs()
//    {
//        Transform firstStep = stairs.transform.GetChild(stairs.transform.childCount - 1);
//        Transform lastStep = stairs.transform.GetChild(0);
//        bool onFirstStep = false;
//        while (!onFirstStep)
//        {
//            if (transform.position.x != firstStep.position.x)
//            {
//                Vector2 target = new Vector2(firstStep.position.x, transform.position.y);
//                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
//                anim.SetBool("isWalking", true);
//            }
//            else
//            {
//                onFirstStep = true;
//            }
//            yield return null;
//        }

//        while (!onCorrectFloor)
//        {
//            if (transform.position.x != lastStep.position.x)
//            {
//                transform.position = Vector2.MoveTowards(transform.position, lastStep.transform.position, moveSpeed * Time.deltaTime);
//            }
//            else
//            {
//                onCorrectFloor = true;
//                currentFloor = 0;
//                useStairs = false;
//            }
//            yield return null;
//        }
//    }

//    private IEnumerator GoUpStairs()
//    {
//        Transform firstStep = stairs.transform.GetChild(0);
//        Transform lastStep = stairs.transform.GetChild(stairs.transform.childCount - 1);
//        bool onFirstStep = false;
//        while (!onFirstStep)
//        {
//            if (transform.position.x != firstStep.position.x)
//            {
//                Vector2 target = new Vector2(firstStep.position.x, transform.position.y);
//                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
//                anim.SetBool("isWalking", true);
//            }
//            else
//            {
//                onFirstStep = true;
//            }
//            yield return null;
//        }

//        while (!onCorrectFloor)
//        {
//            if (transform.position.x != lastStep.position.x)
//            {
//                transform.position = Vector2.MoveTowards(transform.position, lastStep.transform.position, moveSpeed * Time.deltaTime);
//            }
//            else
//            {
//                onCorrectFloor = true;
//                currentFloor = 1;
//                useStairs = false;
//            }
//            yield return null;
//        }
//    }
//}
