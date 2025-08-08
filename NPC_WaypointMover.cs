using System.Collections;
using UnityEngine;

public class NPC_WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;
    private float localTempMoveSpeed;
    private bool isStopped = false;
    private Animator animator;
    private float LastX, LastY;


    private void Start()
    {
        waypoints = new Transform[waypointParent.childCount];
        for (int i = 0; i< waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Controller_Pause.isGamePaused || isWaiting)
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("LastX", LastX);
            animator.SetFloat("LastY", LastY);

            return;
        }

        MoveToWayPoint(); 
    }

    private void MoveToWayPoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);//Update

        Vector2 direction = (target.position - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            LastX = direction.x;
            LastY = direction.y;
        }
        animator.SetFloat("DirectX", direction.x);
        animator.SetFloat("DirectY", direction.y);
        animator.SetBool("isMoving", direction.magnitude > 0);
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isMoving", false);
        animator.SetFloat("LastX", LastX);
        animator.SetFloat("LastY", LastY);

        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex +1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        isWaiting = false;
    }

    public void StopMove(bool stop = true)
    {
        if (stop && !isStopped)
        {
            localTempMoveSpeed = moveSpeed;
            moveSpeed = 0f;
            isStopped = true;
            isWaiting = true;
        }
        else if (isStopped)
        {
            moveSpeed = localTempMoveSpeed;
            isStopped = false;
            isWaiting = false;
        }
    }

}
