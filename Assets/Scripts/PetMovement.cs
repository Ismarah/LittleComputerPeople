using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PetMovement : AgentMovement
{
    private GameObject player;

    void Start()
    {
        base.Init();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void NewTarget(GameObject newTarget)
    {
        StartCoroutine(Move(newTarget));
    }

    private IEnumerator Move(GameObject newTarget)
    {
        target = newTarget;
        while (Vector2.Distance(transform.position, target.transform.position) >= 0.3f)
        {
            int targetFloor = -1;
            if (target.GetComponent<InteractableItem>() != null)
                targetFloor = target.GetComponent<InteractableItem>().GetFloor();
            else if (target.GetComponent<AgentMovement>() != null)
                targetFloor = target.GetComponent<AgentMovement>().GetFloor();

            if (targetFloor == floor)
            {
                Vector2 targetPos = new Vector2(newTarget.transform.position.x, transform.position.y);
                yield return StartCoroutine(MoveToPos(targetPos));
            }
            else yield return StartCoroutine(UseStairs(targetFloor));

            yield return null;
        }

        if (target.GetComponent<InteractableItem>() != null)
        {
            target.GetComponent<InteractableItem>().AgentArrivedAtMyPosition(gameObject);
        }
        if (target.GetComponent<PlayerState>() != null)
        {
            target.GetComponent<PlayerState>().PetArrivedAtMyPosition();
            GetComponent<PetState>().ManipulateNeedChange(GetComponent<PetActions>().GetAction("Food\nplease"));
        }
        anim.SetBool("isWalking", false);
        if (tag == "Pet") anim.SetBool("tail", true);
    }

    public override IEnumerator UseStairs(int targetFloor)
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

    public override IEnumerator MoveToPos(Vector2 _targetPos)
    {
        if (transform.eulerAngles.y == 0) turned = false;
        else turned = true;
        float prevPos = transform.position.x;
        anim.SetBool("isWalking", true);
        if (tag == "Pet") anim.SetBool("tail", false);
        Vector3 pos = new Vector3(_targetPos.x, _targetPos.y, transform.position.z);
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, movespeed * Time.deltaTime * GameObject.FindGameObjectWithTag("ActionQueue").GetComponent<TimeManager>().GetGameSpeed());

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

            yield return null;
        }
    }
}
