using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool up, down, right, left;
    private enum DIR
    {
        Left, Right, Up, Down, None
    }
    private DIR moveTo;
    private DIR currDir;
    private Animator animator;
    public bool action;

    void Start()
    {
        up = down = right = left = false;
        currDir = DIR.Down;
        moveTo = DIR.None;
        animator = GetComponent<Animator>();
        animator.SetBool("isMoving", false);
        animator.SetFloat("DirectionState", 0f);
        //animator.Play("Indle");
    }

    void Update()
    {
        InputControl();
        UpdateDirection();
        UpdateAnimator();
        GetAction();
    }

    private void InputControl()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        right = left = up = down = false;

        if (x > 0 && x >= Mathf.Abs(y)) right = true;
        else if (x < 0 && Mathf.Abs(x) >= Mathf.Abs(y)) left = true;

        if (y > 0 && y >= Mathf.Abs(x)) up = true;
        else if (y < 0 && Mathf.Abs(y) >= Mathf.Abs(x)) down = true;
    }


    private void UpdateDirection()
    {
        if (moveTo == DIR.None)
        {

            if (right)
            {
                moveTo = DIR.Right;
                currDir = DIR.Right;
            }
            else if (left)
            {
                moveTo = DIR.Left;
                currDir = DIR.Left;
            }
            else if (up)
            {
                moveTo = DIR.Up;
                currDir = DIR.Up;
            }
            else if (down)
            {
                moveTo = DIR.Down;
                currDir = DIR.Down;
            }
        }
        else if (!right && !left && !up && !down)
        {
            moveTo = DIR.None;
        }

    }

    private void UpdateAnimator()
    {
        float state;
        if (moveTo != DIR.None)
        {
            if (moveTo == DIR.Left) state = 0f;
            else if (moveTo == DIR.Right) state = 0.33f;
            else if (moveTo == DIR.Up) state = 0.66f;
            else state = 1f;

            animator.SetBool("isMoving", true);
        }
        else
        {
            if (currDir == DIR.Left) state = 0f;
            else if (currDir == DIR.Right) state = 0.33f;
            else if (currDir == DIR.Up) state = 0.66f;
            else state = 1f;

            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("DirectionState", state);
    }

    private void GetAction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            action = true;
        }
        else action = false;
    }
}
