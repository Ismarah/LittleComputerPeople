using System.Collections;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private Vector2 targetPos;
    protected GameObject target;
    [SerializeField]
    protected int floor = 0;
    [SerializeField]
    protected float movespeed;
    [SerializeField]
    protected Transform firstStairsLower, firstStairsUpper, secondStairsLower, secondStairsUpper;
    protected Animator anim;
    protected bool turned;
    protected GameObject manager;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        anim = GetComponent<Animator>();
        targetPos = new Vector2();
        manager = GameObject.FindGameObjectWithTag("ActionQueue");
    }

    public virtual void NewTarget(GameObject newTarget)
    {
        StartCoroutine(Move(newTarget));
    }

    private IEnumerator Move(GameObject newTarget)
    {
        target = newTarget;

        int targetFloor = -1;
        if (target.GetComponent<InteractableItem>() != null)
            targetFloor = target.GetComponent<InteractableItem>().GetFloor();
        else if (target.GetComponent<AgentMovement>() != null)
            targetFloor = target.GetComponent<AgentMovement>().GetFloor();

        while (floor != targetFloor)
        {
            if (target.GetComponent<InteractableItem>() != null)
                targetFloor = target.GetComponent<InteractableItem>().GetFloor();
            else if (target.GetComponent<AgentMovement>() != null)
                targetFloor = target.GetComponent<AgentMovement>().GetFloor();
            yield return StartCoroutine(UseStairs(targetFloor));
        }

        Vector2 targetPos = new Vector2(newTarget.transform.position.x, transform.position.y);

        yield return StartCoroutine(MoveToPos(targetPos));

        if (target.GetComponent<InteractableItem>() != null)
        {
            target.GetComponent<InteractableItem>().AgentArrivedAtMyPosition(gameObject);
        }
        anim.SetBool("isWalking", false);
    }

    public int GetFloor()
    {
        return floor;
    }

    public virtual IEnumerator UseStairs(int targetFloor)
    {
        if (targetFloor > floor)
        {
            if (floor == 0)
            {
                yield return StartCoroutine(MoveToPos(firstStairsLower.position));
                yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
            }
            else if (floor == 1)
            {
                yield return StartCoroutine(MoveToPos(secondStairsLower.position));
                yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
            }
            floor += 1;
        }
        else
        {
            if (floor == 1)
            {
                yield return StartCoroutine(MoveToPos(firstStairsUpper.position));
                yield return StartCoroutine(MoveToPos(firstStairsLower.position));
            }
            else if (floor == 2)
            {
                yield return StartCoroutine(MoveToPos(secondStairsUpper.position));
                yield return StartCoroutine(MoveToPos(secondStairsLower.position));
            }
            floor -= 1;
        }
    }

    public virtual IEnumerator MoveToPos(Vector2 _targetPos)
    {
        float prevPos = transform.position.x;
        anim.SetBool("isWalking", true);
        Vector3 pos = new Vector3(_targetPos.x, _targetPos.y, transform.position.z);
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, movespeed * Time.deltaTime);

            yield return null;
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
